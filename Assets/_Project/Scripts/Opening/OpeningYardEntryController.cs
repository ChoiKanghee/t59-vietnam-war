using UnityEngine;

namespace T59VietnamWar.Opening
{
    public sealed class OpeningYardEntryController : MonoBehaviour
    {
        [Header("Future on-foot gameplay area")]
        [SerializeField] private Transform yardEntryRoot;
        [SerializeField] private SpriteRenderer[] yardRenderers;

        public Transform YardEntryRoot => yardEntryRoot;

        private void Awake()
        {
            SetVisible(false);
            if (yardEntryRoot == null || yardRenderers == null || yardRenderers.Length == 0)
                Debug.LogWarning("Future YardEntry gameplay presentation is incomplete. Run the M1A builder to repair the wiring.", this);
        }

        public void SetVisible(bool visible)
        {
            float alpha = visible ? 1f : 0f;
            if (yardRenderers == null) return;
            for (int i = 0; i < yardRenderers.Length; i++)
            {
                SpriteRenderer renderer = yardRenderers[i];
                if (renderer == null) continue;
                Color color = renderer.color;
                color.a = alpha;
                renderer.color = color;
            }
        }
    }
}
