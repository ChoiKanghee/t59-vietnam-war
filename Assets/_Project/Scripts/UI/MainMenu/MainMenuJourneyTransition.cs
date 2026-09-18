using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace T59VietnamWar.UI
{
    public sealed class MainMenuJourneyTransition : MonoBehaviour
    {
        public enum TransitionState
        {
            MenuLoop,
            Transitioning,
            JourneyRoute
        }

        [Header("Required references")]
        [SerializeField] private MainMenuController menuController;
        [SerializeField] private CanvasGroup menuCanvasGroup;

        [Header("Timing")]
        [SerializeField, Min(0f)] private float fadeDelay;
        [SerializeField, Min(0f)] private float fadeDuration = 0.5f;

        [Header("Future authored route hook")]
        [SerializeField] private UnityEvent journeyRouteEntered;

        private TransitionState state = TransitionState.MenuLoop;

        public TransitionState State => state;
        public bool IsInputLocked => state != TransitionState.MenuLoop;
        public event Action<TransitionState> StateChanged;
        public event Action JourneyRouteEntered;

        private void Awake()
        {
            if (menuCanvasGroup == null)
                Debug.LogError("M1A transition is missing its menu CanvasGroup. Run the M1A builder to repair the menu wiring.", this);
            if (menuController == null)
                Debug.LogError("M1A transition is missing its MainMenuController. Run the M1A builder to repair the menu wiring.", this);
        }

        public bool TryBeginTransition()
        {
            if (state != TransitionState.MenuLoop) return false;
            if (menuController == null || menuCanvasGroup == null)
            {
                Debug.LogError("Start Journey transition cannot run because required references are missing.", this);
                return false;
            }

            SetState(TransitionState.Transitioning);
            menuController.SetTransitionInputLocked(true);
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
            StartCoroutine(FadeMenuAndEnterRoute());
            return true;
        }

        private IEnumerator FadeMenuAndEnterRoute()
        {
            if (fadeDelay > 0f)
                yield return new WaitForSecondsRealtime(fadeDelay);

            float initialAlpha = menuCanvasGroup.alpha;
            if (fadeDuration > 0f)
            {
                float elapsed = 0f;
                while (elapsed < fadeDuration)
                {
                    elapsed += Time.unscaledDeltaTime;
                    menuCanvasGroup.alpha = Mathf.Lerp(initialAlpha, 0f, Mathf.Clamp01(elapsed / fadeDuration));
                    yield return null;
                }
            }

            menuCanvasGroup.alpha = 0f;
            SetState(TransitionState.JourneyRoute);
            JourneyRouteEntered?.Invoke();
            journeyRouteEntered?.Invoke();
        }

        private void SetState(TransitionState nextState)
        {
            state = nextState;
            StateChanged?.Invoke(state);
        }
    }
}
