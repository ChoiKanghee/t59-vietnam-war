using UnityEngine;

namespace T59VietnamWar.UI
{
    [ExecuteAlways]
    public sealed class MainMenuCampaignVisualState : MonoBehaviour
    {
        public enum DevelopmentPreview
        {
            TruckOpening,
            ExistingTankFallback
        }

        [SerializeField] private DevelopmentPreview preview = DevelopmentPreview.TruckOpening;
        [SerializeField] private GameObject truckOpening;
        [SerializeField] private GameObject existingTankFallback;

        public DevelopmentPreview Preview => preview;

        private void Awake() => Apply();
        private void OnEnable() => Apply();

#if UNITY_EDITOR
        private void OnValidate() => Apply();
#endif

        public void Apply()
        {
            bool showTruck = preview == DevelopmentPreview.TruckOpening;
            if (truckOpening != null && truckOpening.activeSelf != showTruck)
                truckOpening.SetActive(showTruck);
            if (existingTankFallback != null && existingTankFallback.activeSelf == showTruck)
                existingTankFallback.SetActive(!showTruck);
        }
    }
}
