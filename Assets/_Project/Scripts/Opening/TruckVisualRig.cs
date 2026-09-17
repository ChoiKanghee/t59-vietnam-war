using UnityEngine;

namespace T59VietnamWar.Opening
{
    public sealed class TruckVisualRig : MonoBehaviour
    {
        [SerializeField] private Transform bodyBob;
        [SerializeField] private Transform rearWheel;
        [SerializeField] private Transform frontWheel;
        [SerializeField] private SpriteRenderer dustRenderer;
        [SerializeField] private Sprite[] dustFrames;

        [Header("Motion")]
        [SerializeField, Min(0.01f)] private float rearWheelRadius = 0.82f;
        [SerializeField, Min(0.01f)] private float frontWheelRadius = 0.82f;
        [SerializeField, Min(0f)] private float bobAmplitude = 0.03f;
        [SerializeField, Min(0f)] private float bobFrequency = 2.4f;
        [SerializeField, Min(1f)] private float dustFramesPerSecond = 11f;

        private Vector3 bodyOrigin;
        private Quaternion rearWheelOrigin;
        private Quaternion frontWheelOrigin;

        private void Awake()
        {
            if (bodyBob != null) bodyOrigin = bodyBob.localPosition;
            if (rearWheel != null) rearWheelOrigin = rearWheel.localRotation;
            if (frontWheel != null) frontWheelOrigin = frontWheel.localRotation;
        }

        public void SetMotion(double elapsedTime, double travelDistance)
        {
            float time = (float)elapsedTime;
            if (bodyBob != null)
            {
                Vector3 position = bodyOrigin;
                position.y += Mathf.Sin(time * bobFrequency * Mathf.PI * 2f) * bobAmplitude;
                bodyBob.localPosition = position;
            }

            SetWheelRotation(rearWheel, rearWheelOrigin, travelDistance, rearWheelRadius);
            SetWheelRotation(frontWheel, frontWheelOrigin, travelDistance, frontWheelRadius);

            if (dustRenderer != null && dustFrames != null && dustFrames.Length > 0)
            {
                int frame = Mathf.FloorToInt(time * dustFramesPerSecond) % dustFrames.Length;
                dustRenderer.sprite = dustFrames[frame];
                dustRenderer.enabled = true;
            }
        }

        private static void SetWheelRotation(Transform wheel, Quaternion origin, double distance, float radius)
        {
            if (wheel == null || radius <= 0f) return;
            float angle = (float)(-(distance / radius) * Mathf.Rad2Deg % 360d);
            wheel.localRotation = origin * Quaternion.Euler(0f, 0f, angle);
        }
    }
}
