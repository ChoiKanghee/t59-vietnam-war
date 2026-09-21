using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace T59VietnamWar.Opening
{
    public sealed class OpeningLamDisembarkController : MonoBehaviour
    {
        public enum DisembarkState
        {
            WaitingForTruckStopped,
            GetOffTruck,
            Landing,
            LamGrounded
        }

        [Header("Truck stop source")]
        [SerializeField] private OpeningTruckDecelerationController truckDeceleration;

        [Header("Lâm opening presentation")]
        [SerializeField] private SpriteRenderer lamRenderer;
        [SerializeField] private Sprite[] getOffTruckFrames;
        [SerializeField] private Sprite[] landingFrames;

        [Header("Truck-local root motion")]
        [SerializeField] private Vector3 getOffTruckStartLocalPosition = new Vector3(-4.95f, 0.25f, 0f);
        [SerializeField] private Vector3 landingLocalPosition = new Vector3(-6.25f, -2.8f, 0f);
        [SerializeField] private AnimationCurve rootMotionEasing =
            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Unscaled animation timing")]
        [SerializeField, Min(1f)] private float getOffTruckFramesPerSecond = 11f;
        [SerializeField, Min(1f)] private float landingFramesPerSecond = 11f;

        [Header("Next opening step hook")]
        [SerializeField] private UnityEvent lamGrounded;

        private DisembarkState state = DisembarkState.WaitingForTruckStopped;
        private Coroutine sequence;
        private bool sequenceStarted;
        private bool completionEmitted;

        public DisembarkState State => state;
        public event Action<DisembarkState> StateChanged;
        public event Action LamGrounded;

        private void Awake()
        {
            if (lamRenderer != null)
            {
                lamRenderer.transform.localPosition = getOffTruckStartLocalPosition;
                lamRenderer.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (truckDeceleration == null || lamRenderer == null)
            {
                Debug.LogError("Lâm disembark is missing its truck-stop source or presentation renderer. Run the M1A builder to repair the wiring.", this);
                return;
            }

            truckDeceleration.TruckStopped -= HandleTruckStopped;
            truckDeceleration.TruckStopped += HandleTruckStopped;

            if (truckDeceleration.State == OpeningTruckDecelerationController.DecelerationState.TruckStopped)
                HandleTruckStopped();
        }

        private void OnDisable()
        {
            if (truckDeceleration != null)
                truckDeceleration.TruckStopped -= HandleTruckStopped;

            if (sequence != null)
            {
                StopCoroutine(sequence);
                sequence = null;
            }

            if (!completionEmitted)
            {
                sequenceStarted = false;
                state = DisembarkState.WaitingForTruckStopped;
                if (lamRenderer != null)
                {
                    lamRenderer.transform.localPosition = getOffTruckStartLocalPosition;
                    lamRenderer.enabled = false;
                }
            }
        }

        private void HandleTruckStopped()
        {
            if (sequenceStarted || completionEmitted)
                return;

            if (!HasValidFrames())
            {
                Debug.LogError("Lâm disembark requires exactly 8 GetOffTruck frames and 4 Landing frames.", this);
                return;
            }

            sequenceStarted = true;
            sequence = StartCoroutine(RunDisembark());
        }

        private IEnumerator RunDisembark()
        {
            lamRenderer.transform.localPosition = getOffTruckStartLocalPosition;
            lamRenderer.enabled = true;

            SetState(DisembarkState.GetOffTruck);
            yield return PlayGetOffTruck();

            SetState(DisembarkState.Landing);
            lamRenderer.transform.localPosition = landingLocalPosition;
            yield return PlayFrames(landingFrames, landingFramesPerSecond);

            // PlayFrames leaves the last assigned sprite visible. Future idle/player
            // integration can replace it after observing LamGrounded.
            SetState(DisembarkState.LamGrounded);
            sequence = null;
            EmitGroundedOnce();
        }

        private IEnumerator PlayGetOffTruck()
        {
            float framesPerSecond = Mathf.Max(1f, getOffTruckFramesPerSecond);
            float duration = getOffTruckFrames.Length / framesPerSecond;
            float elapsed = 0f;
            int displayedFrame = -1;

            while (elapsed < duration)
            {
                int frame = Mathf.Min(Mathf.FloorToInt(elapsed * framesPerSecond),
                    getOffTruckFrames.Length - 1);
                if (frame != displayedFrame)
                {
                    displayedFrame = frame;
                    lamRenderer.sprite = getOffTruckFrames[frame];
                }

                float progress = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 1f;
                float easedProgress = rootMotionEasing != null
                    ? Mathf.Clamp01(rootMotionEasing.Evaluate(progress))
                    : Mathf.SmoothStep(0f, 1f, progress);
                lamRenderer.transform.localPosition = Vector3.LerpUnclamped(
                    getOffTruckStartLocalPosition, landingLocalPosition, easedProgress);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            lamRenderer.sprite = getOffTruckFrames[getOffTruckFrames.Length - 1];
            lamRenderer.transform.localPosition = landingLocalPosition;
        }

        private IEnumerator PlayFrames(Sprite[] frames, float framesPerSecond)
        {
            float frameDuration = 1f / Mathf.Max(1f, framesPerSecond);
            for (int i = 0; i < frames.Length; i++)
            {
                lamRenderer.sprite = frames[i];
                yield return new WaitForSecondsRealtime(frameDuration);
            }
        }

        private bool HasValidFrames()
        {
            if (getOffTruckFrames == null || getOffTruckFrames.Length != 8 ||
                landingFrames == null || landingFrames.Length != 4)
                return false;

            for (int i = 0; i < getOffTruckFrames.Length; i++)
                if (getOffTruckFrames[i] == null) return false;
            for (int i = 0; i < landingFrames.Length; i++)
                if (landingFrames[i] == null) return false;
            return true;
        }

        private void EmitGroundedOnce()
        {
            if (completionEmitted) return;
            completionEmitted = true;
            LamGrounded?.Invoke();
            lamGrounded?.Invoke();
        }

        private void SetState(DisembarkState nextState)
        {
            state = nextState;
            StateChanged?.Invoke(state);
        }
    }
}
