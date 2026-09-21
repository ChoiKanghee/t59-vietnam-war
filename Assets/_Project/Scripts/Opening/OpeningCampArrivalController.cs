using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace T59VietnamWar.Opening
{
    public sealed class OpeningCampArrivalController : MonoBehaviour
    {
        public enum ArrivalState
        {
            WaitingForRoute,
            GateApproach,
            GateApproachComplete
        }

        [Header("Route source")]
        [SerializeField] private OpeningJourneyRouteController journeyRoute;

        [Header("Gate approach visuals")]
        [SerializeField] private Transform gateApproachRoot;
        [SerializeField] private Transform gateFocalTransform;
        [SerializeField] private SpriteRenderer[] forestEdgeRenderers;
        [SerializeField] private SpriteRenderer[] fenceRenderers;
        [SerializeField] private SpriteRenderer[] watchtowerRenderers;
        [SerializeField] private SpriteRenderer[] gateRenderers;

        [Header("Unscaled gate approach timing")]
        [SerializeField, Min(0f)] private float approachDuration = 5.75f;
        [SerializeField] private float approachStartOffset = 17.5f;
        [SerializeField, Range(0f, 1f)] private float forestFadeStart = 0.62f;
        [SerializeField, Range(0f, 1f)] private float forestFinalAlpha = 0.32f;
        [SerializeField, Range(0.5f, 1f)] private float gateFocusStartScale = 0.92f;

        [Header("Next arrival step hook")]
        [SerializeField] private UnityEvent gateApproachCompleted;

        private ArrivalState state = ArrivalState.WaitingForRoute;
        private Vector3 gateRestPosition;
        private Vector3 gateRestScale;
        private bool approachStarted;
        private bool completionEmitted;

        public ArrivalState State => state;
        public event Action<ArrivalState> StateChanged;
        public event Action GateApproachCompleted;

        private void Awake()
        {
            if (gateApproachRoot != null)
            {
                gateRestPosition = gateApproachRoot.localPosition;
                gateApproachRoot.localPosition = gateRestPosition + Vector3.right * approachStartOffset;
            }

            if (gateFocalTransform != null)
            {
                gateRestScale = gateFocalTransform.localScale;
                gateFocalTransform.localScale = gateRestScale * gateFocusStartScale;
            }

            SetAlpha(fenceRenderers, 0f);
            SetAlpha(watchtowerRenderers, 0f);
            SetAlpha(gateRenderers, 0f);
            WarnIfVisualsMissing();
        }

        private void OnEnable()
        {
            if (journeyRoute == null)
            {
                Debug.LogError("Camp arrival is missing its OpeningJourneyRouteController source. Run the M1A builder to repair the wiring.", this);
                return;
            }

            journeyRoute.RouteCompleted -= HandleRouteCompleted;
            journeyRoute.RouteCompleted += HandleRouteCompleted;

            if (journeyRoute.State == OpeningJourneyRouteController.RouteState.RouteComplete)
                HandleRouteCompleted();
        }

        private void OnDisable()
        {
            if (journeyRoute != null)
                journeyRoute.RouteCompleted -= HandleRouteCompleted;
        }

        private void HandleRouteCompleted()
        {
            if (approachStarted) return;
            approachStarted = true;
            StartCoroutine(RunGateApproach());
        }

        private IEnumerator RunGateApproach()
        {
            SetState(ArrivalState.GateApproach);

            float duration = Mathf.Max(0f, approachDuration);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 1f;
                float movementProgress = Mathf.SmoothStep(0f, 1f, progress);
                float fenceProgress = SegmentProgress(progress, 0.02f, 0.32f);
                float watchtowerProgress = SegmentProgress(progress, 0.18f, 0.52f);
                float gateProgress = SegmentProgress(progress, 0.42f, 0.88f);
                float forestFadeProgress = SegmentProgress(progress, forestFadeStart, 1f);

                if (gateApproachRoot != null)
                    gateApproachRoot.localPosition = gateRestPosition +
                        Vector3.right * Mathf.Lerp(approachStartOffset, 0f, movementProgress);
                if (gateFocalTransform != null)
                    gateFocalTransform.localScale = gateRestScale *
                        Mathf.Lerp(gateFocusStartScale, 1f, gateProgress);

                SetAlpha(fenceRenderers, fenceProgress);
                SetAlpha(watchtowerRenderers, watchtowerProgress);
                SetAlpha(gateRenderers, gateProgress);
                SetAlpha(forestEdgeRenderers, Mathf.Lerp(1f, forestFinalAlpha, forestFadeProgress));
                yield return null;
            }

            if (gateApproachRoot != null) gateApproachRoot.localPosition = gateRestPosition;
            if (gateFocalTransform != null) gateFocalTransform.localScale = gateRestScale;
            SetAlpha(forestEdgeRenderers, forestFinalAlpha);
            SetAlpha(fenceRenderers, 1f);
            SetAlpha(watchtowerRenderers, 1f);
            SetAlpha(gateRenderers, 1f);
            SetState(ArrivalState.GateApproachComplete);
            EmitCompletionOnce();
        }

        private static float SegmentProgress(float progress, float start, float end)
        {
            if (end <= start) return progress >= end ? 1f : 0f;
            return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((progress - start) / (end - start)));
        }

        private void EmitCompletionOnce()
        {
            if (completionEmitted) return;
            completionEmitted = true;
            GateApproachCompleted?.Invoke();
            gateApproachCompleted?.Invoke();
        }

        private void SetState(ArrivalState nextState)
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

        private void WarnIfVisualsMissing()
        {
            if (gateApproachRoot == null)
                Debug.LogWarning("Camp gate approach has no presentation root assigned.", this);
            if (fenceRenderers == null || fenceRenderers.Length == 0 ||
                watchtowerRenderers == null || watchtowerRenderers.Length == 0 ||
                gateRenderers == null || gateRenderers.Length == 0)
                Debug.LogWarning("Camp gate approach is missing one or more staged B2 visual groups.", this);
        }
    }
}
