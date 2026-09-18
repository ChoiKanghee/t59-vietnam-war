using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace T59VietnamWar.UI
{
    [DefaultExecutionOrder(10000)]
    public sealed class MainMenuDisclaimer : MonoBehaviour, IPointerDownHandler
    {
        [Header("Required references")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button[] menuButtons;
        [SerializeField] private Button preferredSelection;

        [Header("Timing (unscaled seconds)")]
        [SerializeField, Min(0f)] private float minimumDuration = 1f;
        [SerializeField, Min(0f)] private float fadeDuration = 0.8f;

        private static bool shownThisSession;

        private bool[] previousInteractableStates;
        private float shownAt;
        private bool dismissing;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetSessionState() => shownThisSession = false;

        private void Awake()
        {
            if (shownThisSession)
            {
                gameObject.SetActive(false);
                return;
            }

            shownThisSession = true;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void OnEnable()
        {
            shownAt = Time.unscaledTime;
            previousInteractableStates = new bool[menuButtons.Length];
            for (int i = 0; i < menuButtons.Length; i++)
            {
                Button button = menuButtons[i];
                if (button == null) continue;
                previousInteractableStates[i] = button.interactable;
                button.interactable = false;
            }

            ClearSelection();
        }

        private void Update()
        {
            if (dismissing) return;

            float elapsed = Time.unscaledTime - shownAt;
            if (elapsed < minimumDuration) return;

            bool keyboardPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
            bool mousePressed = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            bool touchPressed = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
            if (keyboardPressed || mousePressed || touchPressed)
                BeginDismissal();
        }

        private void LateUpdate()
        {
            if (!dismissing) ClearSelection();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Time.unscaledTime - shownAt >= minimumDuration)
                BeginDismissal();
            eventData.Use();
        }

        private void BeginDismissal()
        {
            if (dismissing) return;
            dismissing = true;
            StartCoroutine(FadeAndUnlock());
        }

        private IEnumerator FadeAndUnlock()
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            RestoreMenuInput();
            gameObject.SetActive(false);
        }

        private void RestoreMenuInput()
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                Button button = menuButtons[i];
                if (button != null) button.interactable = previousInteractableStates[i];
            }

            Button selection = preferredSelection != null && preferredSelection.interactable
                ? preferredSelection
                : FirstInteractableButton();
            if (selection != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(selection.gameObject);
        }

        private Button FirstInteractableButton()
        {
            foreach (Button button in menuButtons)
                if (button != null && button.interactable) return button;
            return null;
        }

        private static void ClearSelection()
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
