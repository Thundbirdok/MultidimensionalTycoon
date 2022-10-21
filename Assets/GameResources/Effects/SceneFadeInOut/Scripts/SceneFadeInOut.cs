using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.Effects.SceneFadeInOut.Scripts
{
    public sealed class SceneFadeInOut : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        private const float DURATION = 1.5f;

        private readonly Color _fadeOut = Color.black;
        private readonly Color _fadeIn = new(0, 0, 0, 0);

        public void InitFadeIn()
        {
            transform.SetAsLastSibling();
            image.color = _fadeOut;
        }

        public void InitFadeOut()
        {
            transform.SetAsLastSibling();
            image.color = _fadeIn;
        }

        public async Task FadeIn()
        {
            await ChangeColor(_fadeOut, _fadeIn, DURATION);
        }

        public async Task FadeOut()
        {
            await ChangeColor(_fadeIn, _fadeOut, DURATION);
        }

        private async Task ChangeColor(Color begin, Color end, float duration)
        {
            var t = duration;

            do
            {
                if (image == false)
                {
                    return;
                }
                
                var d = Mathf.Clamp01(t / duration);

                image.color = begin * d + end * (1 - d);

                await Task.Yield();

                t -= Time.deltaTime;
            }
            while (t > 0);
        }
    }
}