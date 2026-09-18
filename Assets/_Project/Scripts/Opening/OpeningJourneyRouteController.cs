using System;
using System.Collections;
using T59VietnamWar.UI;
using UnityEngine;
using UnityEngine.Events;

namespace T59VietnamWar.Opening
{
    public sealed class OpeningJourneyRouteController : MonoBehaviour
    {
        public enum RouteState
        {
            WaitingForJourney,
            Village,
            RiceField,
            ForestEdge,
            RouteComplete
        }

        [Header("Journey source")]
        [SerializeField] private MainMenuJourneyTransition journeyTransition;

        [Header("Phase visuals")]
        [SerializeField] private SpriteRenderer[] skyRenderers;
        [SerializeField] private SpriteRenderer[] mountainRenderers;
        [SerializeField] private SpriteRenderer[] villageRenderers;
        [SerializeField] private SpriteRenderer[] riceFieldRenderers;
        [SerializeField] private SpriteRenderer[] ruralLifeRenderers;
        [SerializeField] private SpriteRenderer[] forestEdgeRenderers;

        [Header("Unscaled phase timing")]
        [SerializeField, Min(0f)] private float villageDuration = 9f;
        [SerializeField, Min(0f)] private float riceFieldDuration = 9f;
        [SerializeField, Min(0f)] private float forestEdgeDuration = 10f;
        [SerializeField, Min(0f)] private float crossFadeDuration = 1.5f;

        [Header("Future B2 arrival hook")]
        [SerializeField] private UnityEvent routeCompleted;

        private RouteState state = RouteState.WaitingForJourney;
        private bool routeStarted;

        public RouteState State => state;
        public event Action<RouteState> StateChanged;
        public event Action RouteCompleted;

        private void Awake()
        {
            SetAlpha(forestEdgeRenderers, 0f);
            SetAlpha(skyRenderers, 1f);
            SetAlpha(mountainRenderers, 1f);
            SetAlpha(villageRenderers, 1f);
            SetAlpha(riceFieldRenderers, 1f);
            SetAlpha(ruralLifeRenderers, 1f);
            WarnIfVisualsMissing(villageRenderers, "Village");
            WarnIfVisualsMissing(riceFieldRenderers, "RiceField");
            WarnIfVisualsMissing(forestEdgeRenderers, "ForestEdge (B07)");
        }

        private void OnEnable()
        {
            if (journeyTransition == null)
            {
                Debug.LogError("Journey route is missing its MainMenuJourneyTransition source. Run the M1A builder to repair the wiring.", this);
                return;
            }

            journeyTransition.JourneyRouteEntered -= HandleJourneyRouteEntered;
            journeyTransition.JourneyRouteEntered += HandleJourneyRouteEntered;

            if (journeyTransition.State == MainMenuJourneyTransition.TransitionState.JourneyRoute)
                HandleJourneyRouteEntered();
        }

        private void OnDisable()
        {
            if (journeyTransition != null)
                journeyTransition.JourneyRouteEntered -= HandleJourneyRouteEntered;
        }

        private void HandleJourneyRouteEntered()
        {
            if (routeStarted) return;
            routeStarted = true;
            StartCoroutine(RunRoute());
        }

        private IEnumerator RunRoute()
        {
            SetState(RouteState.Village);
            if (villageDuration > 0f)
                yield return new WaitForSecondsRealtime(villageDuration);

            SetState(RouteState.RiceField);
            if (riceFieldDuration > 0f)
                yield return new WaitForSecondsRealtime(riceFieldDuration);

            SetState(RouteState.ForestEdge);
            yield return CrossFadeThenHold(forestEdgeDuration,
                Combine(skyRenderers, mountainRenderers, villageRenderers,
                    riceFieldRenderers, ruralLifeRenderers), forestEdgeRenderers);

            SetState(RouteState.RouteComplete);
            RouteCompleted?.Invoke();
            routeCompleted?.Invoke();
        }

        private IEnumerator CrossFadeThenHold(float phaseDuration,
            SpriteRenderer[] fadeOut, SpriteRenderer[] fadeIn)
        {
            float fadeTime = Mathf.Min(crossFadeDuration, phaseDuration);
            float holdTime = Mathf.Max(0f, phaseDuration - fadeTime);

            if (fadeTime <= 0f)
            {
                SetAlpha(fadeOut, 0f);
                SetAlpha(fadeIn, 1f);
            }
            else
            {
                float elapsed = 0f;
                while (elapsed < fadeTime)
                {
                    elapsed += Time.unscaledDeltaTime;
                    float progress = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / fadeTime));
                    SetAlpha(fadeOut, 1f - progress);
                    SetAlpha(fadeIn, progress);
                    yield return null;
                }

                SetAlpha(fadeOut, 0f);
                SetAlpha(fadeIn, 1f);
            }

            if (holdTime > 0f) yield return new WaitForSecondsRealtime(holdTime);
        }

        private void SetState(RouteState nextState)
        {
            state = nextState;
            StateChanged?.Invoke(state);
        }

        private static void SetAlpha(SpriteRenderer[] renderers, float alpha)
        {
            if (renderers == null) return;
            for (int i = 0; i < renderers.Length; i++)
            {
                SpriteRenderer renderer = renderers[i];
                if (renderer == null) continue;
                Color color = renderer.color;
                color.a = alpha;
                renderer.color = color;
            }
        }

        private static SpriteRenderer[] Combine(params SpriteRenderer[][] groups)
        {
            int count = 0;
            for (int i = 0; i < groups.Length; i++)
                if (groups[i] != null) count += groups[i].Length;

            var combined = new SpriteRenderer[count];
            int offset = 0;
            for (int i = 0; i < groups.Length; i++)
            {
                SpriteRenderer[] group = groups[i];
                if (group == null) continue;
                Array.Copy(group, 0, combined, offset, group.Length);
                offset += group.Length;
            }
            return combined;
        }

        private void WarnIfVisualsMissing(SpriteRenderer[] renderers, string phase)
        {
            if (renderers == null || renderers.Length == 0)
                Debug.LogWarning($"Journey route phase '{phase}' has no visual renderers assigned.", this);
        }
    }
}
