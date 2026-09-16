using UnityEngine;
using UnityEngine.UI;

namespace T59VietnamWar.UI
{
    public sealed class MenuAmbience : MonoBehaviour
    {
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Image[] smokePuffs;
        [SerializeField] private Sprite[] smokeFrames;
        private Vector2 foregroundOrigin;
        private Vector2[] smokeOrigins;
        private float elapsed;

        private void Awake()
        {
            foregroundOrigin = foreground.anchoredPosition;
            smokeOrigins = new Vector2[smokePuffs.Length];
            for (int i = 0; i < smokePuffs.Length; i++)
                smokeOrigins[i] = smokePuffs[i].rectTransform.anchoredPosition;
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            foreground.anchoredPosition = foregroundOrigin + new Vector2(Mathf.Sin(elapsed * Mathf.PI / 5f) * 3f, 0f);
            for (int i = 0; i < smokePuffs.Length; i++)
            {
                // Staggered six-second wisps; swap sprite only while fully transparent.
                float age = elapsed / 6f + (float)i / smokePuffs.Length;
                float phase = Mathf.Repeat(age, 1f);
                Image puff = smokePuffs[i];
                puff.rectTransform.anchoredPosition = smokeOrigins[i] + new Vector2(phase * 18f, phase * 42f);
                puff.rectTransform.localScale = Vector3.one * Mathf.Lerp(0.75f, 1.15f, phase);
                puff.color = new Color(0.86f, 0.88f, 0.84f, Mathf.Sin(phase * Mathf.PI) * 0.16f);
                if (smokeFrames.Length > 0)
                    puff.sprite = smokeFrames[((int)age + i) % smokeFrames.Length];
            }
        }

        private void OnDisable()
        {
            if (foreground != null) foreground.anchoredPosition = foregroundOrigin;
            if (smokeOrigins == null) return;
            for (int i = 0; i < smokePuffs.Length; i++)
            {
                smokePuffs[i].rectTransform.anchoredPosition = smokeOrigins[i];
                smokePuffs[i].rectTransform.localScale = Vector3.one;
            }
        }
    }
}
