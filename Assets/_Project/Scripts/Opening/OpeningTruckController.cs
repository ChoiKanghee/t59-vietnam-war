using UnityEngine;

namespace T59VietnamWar.Opening
{
    public sealed class OpeningTruckController : MonoBehaviour
    {
        [SerializeField] private ParallaxLayer2D[] parallaxLayers;
        [SerializeField] private TruckVisualRig truckVisualRig;
        [SerializeField, Min(0f)] private float baseScrollSpeed = 4.2f;

        private double motionTime;
        private double travelDistance;
        private float speedMultiplier = 1f;

        public float SpeedMultiplier => speedMultiplier;
        public double TravelDistance => travelDistance;

        public void SetSpeedMultiplier(float value)
        {
            speedMultiplier = Mathf.Clamp01(value);
            if (truckVisualRig != null)
                truckVisualRig.SetMotion(motionTime, travelDistance, speedMultiplier);
        }

        private void Update()
        {
            float deltaTime = Time.unscaledDeltaTime;
            float currentSpeed = baseScrollSpeed * speedMultiplier;
            travelDistance += currentSpeed * deltaTime;
            motionTime += deltaTime * speedMultiplier;

            if (parallaxLayers != null)
            {
                for (int i = 0; i < parallaxLayers.Length; i++)
                    if (parallaxLayers[i] != null) parallaxLayers[i].SetTravelDistance(travelDistance);
            }

            if (truckVisualRig != null)
                truckVisualRig.SetMotion(motionTime, travelDistance, speedMultiplier);
        }
    }
}
