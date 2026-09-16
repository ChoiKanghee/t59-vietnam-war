using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace T59VietnamWar.UI
{
    public sealed class MenuPanel : MonoBehaviour, ICancelHandler
    {
        [SerializeField] private MainMenuController menu;
        [SerializeField] private Button closeButton;

        private void OnEnable() => closeButton.onClick.AddListener(Close);
        private void OnDisable() => closeButton.onClick.RemoveListener(Close);
        public void Close() => menu.ClosePanel();
        public void OnCancel(BaseEventData eventData)
        {
            Close();
            eventData.Use();
        }

        public void Focus()
        {
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
        }
    }
}
