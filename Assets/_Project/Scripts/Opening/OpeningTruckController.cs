using UnityEngine;

namespace T59VietnamWar.Opening
{
    public sealed class OpeningTruckController : MonoBehaviour
    {
        [SerializeField] private ParallaxLayer2D[] parallaxLayers;
        [SerializeField] private TruckVisualRig truckVisualRig;
        [SerializeField, Min(0f)] private float baseScrollSpeed = 4.2f;

        private double elapsedTime;

        private void Update()
        {
            elapsedTime += Time.unscaledDeltaTime;
            double travelDistance = elapsedTime * baseScrollSpeed;

            if (parallaxLayers != null)
            {
                for (int i = 0; i < parallaxLayers.Length; i++)
                    if (parallaxLayers[i] != null) parallaxLayers[i].SetTravelDistance(travelDistance);
            }

            if (truckVisualRig != null) truckVisualRig.SetMotion(elapsedTime, travelDistance);
        }
    }
}
