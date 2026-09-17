using System;
using UnityEngine;

namespace T59VietnamWar.Opening
{
    public sealed class ParallaxLayer2D : MonoBehaviour
    {
        [SerializeField] private Transform[] tiles;
        [SerializeField, Min(0f)] private float speedMultiplier = 1f;
        [SerializeField, Min(0.01f)] private float tileSpan = 19.2f;

        private Vector3[] origins;

        private void Awake() => CacheOrigins();

        public void SetTravelDistance(double travelDistance)
        {
            if (tiles == null || tiles.Length == 0 || tileSpan <= 0f) return;
            if (origins == null || origins.Length != tiles.Length) CacheOrigins();

            double scaledDistance = travelDistance * speedMultiplier;
            float phase = (float)(scaledDistance - Math.Floor(scaledDistance / tileSpan) * tileSpan);
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == null) continue;
                Vector3 position = origins[i];
                position.x -= phase;
                tiles[i].localPosition = position;
            }
        }

        private void CacheOrigins()
        {
            if (tiles == null)
            {
                origins = Array.Empty<Vector3>();
                return;
            }

            origins = new Vector3[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
                origins[i] = tiles[i] != null ? tiles[i].localPosition : Vector3.zero;
        }
    }
}
