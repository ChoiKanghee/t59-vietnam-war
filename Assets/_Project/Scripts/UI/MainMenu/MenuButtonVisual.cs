using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace T59VietnamWar.UI
{
    // Presentation only: never changes selection, interactability, or click listeners.
    [RequireComponent(typeof(Button))]
    public sealed class MenuButtonVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Text label;
        [SerializeField] private Image focusIcon;
        private Button button;
        private bool hovered;

        private void Awake() => button = GetComponent<Button>();
        private void OnEnable() => hovered = false;
        private void OnDisable()
        {
            hovered = false;
            if (focusIcon != null) focusIcon.enabled = false;
        }
        public void OnPointerEnter(PointerEventData eventData) => hovered = true;
        public void OnPointerExit(PointerEventData eventData) => hovered = false;

        private void LateUpdate()
        {
            bool available = button.IsInteractable();
            bool focused = available && (hovered || (EventSystem.current != null &&
                EventSystem.current.currentSelectedGameObject == gameObject));
            if (focusIcon != null && focusIcon.enabled != focused) focusIcon.enabled = focused;
            if (label != null)
            {
                Color color = focused ? new Color(1f, 0.91f, 0.66f) : new Color(0.86f, 0.88f, 0.78f);
                color.a = available ? 1f : 0.45f;
                if (label.color != color) label.color = color;
            }
        }
    }
}
