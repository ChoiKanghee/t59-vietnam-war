using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace T59VietnamWar.Opening
{
    public sealed class OpeningTruckDecelerationController : MonoBehaviour
    {
        public enum DecelerationState
        {
            WaitingForGateApproach,
            TruckDeceleration,
            TruckStopped
        }

        [Header("Gate approach source")]
        [SerializeField] private OpeningCampArrivalController campArrival;

        [Header("Shared truck motion")]
        [SerializeField] private OpeningTruckController truckController;
        [SerializeField] private Transform campEntranceRoot;
        [SerializeField, Min(0f)] private float decelerationDuration = 3.5f;

        [Header("Next arrival step hook")]
        [SerializeField] private UnityEvent truckStopped;

        private DecelerationState state = DecelerationState.WaitingForGateApproach;
        private bool decelerationStarted;
        private bool completionEmitted;

        public DecelerationState State => state;
        public event Action<DecelerationState> StateChanged;
        public event Action TruckStopped;

        private void OnEnable()
        {
            if (campArrival == null || truckController == null)
            {
                Debug.LogError("Truck deceleration is missing its camp-arrival or shared-motion source. Run the M1A builder to repair the wiring.", this);
                return;
            }

            campArrival.GateApproachCompleted -= HandleGateApproachCompleted;
            campArrival.GateApproachCompleted += HandleGateApproachCompleted;

            if (campArrival.State == OpeningCampArrivalController.ArrivalState.GateApproachComplete)
                HandleGateApproachCompleted();
        }

        private void OnDisable()
        {
            if (campArrival != null)
                campArrival.GateApproachCompleted -= HandleGateApproachCompleted;
        }

        private void HandleGateApproachCompleted()
        {
            if (decelerationStarted) return;
            decelerationStarted = true;
            StartCoroutine(RunDeceleration());
        }

        private IEnumerator RunDeceleration()
        {
            SetState(DecelerationState.TruckDeceleration);
            float duration = Mathf.Max(0f, decelerationDuration);
            float startingMultiplier = truckController.SpeedMultiplier;
            double startingDistance = truckController.TravelDistance;
            Vector3 entranceOrigin = campEntranceRoot != null
                ? campEntranceRoot.localPosition
                : Vector3.zero;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 1f;
                float easedProgress = Mathf.SmoothStep(0f, 1f, progress);
                truckController.SetSpeedMultiplier(Mathf.Lerp(startingMultiplier, 0f, easedProgress));
                if (campEntranceRoot != null)
                {
                    Vector3 position = entranceOrigin;
                    position.x -= (float)(truckController.TravelDistance - startingDistance);
                    campEntranceRoot.localPosition = position;
                }
                yield return null;
            }

            truckController.SetSpeedMultiplier(0f);
            if (campEntranceRoot != null)
            {
                Vector3 position = entranceOrigin;
                position.x -= (float)(truckController.TravelDistance - startingDistance);
                campEntranceRoot.localPosition = position;
            }
            SetState(DecelerationState.TruckStopped);
            EmitStoppedOnce();
        }

        private void EmitStoppedOnce()
        {
            if (completionEmitted) return;
            completionEmitted = true;
            TruckStopped?.Invoke();
            truckStopped?.Invoke();
        }

        private void SetState(DecelerationState nextState)
        {
            state = nextState;
            StateChanged?.Invoke(state);
        }
    }
}
