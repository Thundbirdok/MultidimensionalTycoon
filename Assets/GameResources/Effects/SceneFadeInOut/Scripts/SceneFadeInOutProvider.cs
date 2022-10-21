using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Effects.SceneFadeInOut.Scripts
{
    public sealed class SceneFadeInOutProvider : MonoBehaviour
    {
        [SerializeField]
        private Camera overlayCamera;

        [SerializeField]
        private GameObject blackImage;

        [SerializeField]
        private AssetReferenceGameObject sceneFadeInOutReference;

        private SceneFadeInOut _sceneFadeInOut;

        private void OnEnable()
        {
            blackImage.SetActive(true);
            overlayCamera.gameObject.SetActive(true);
        }

        public async Task FadeIn()
        {
            overlayCamera.gameObject.SetActive(true);

            if (_sceneFadeInOut == null)
            {
                await InitFadeIn();
            }

            blackImage.SetActive(false);

            await _sceneFadeInOut.FadeIn();

            Release();

            overlayCamera.gameObject.SetActive(false);
        }

        public async Task FadeOut()
        {
            blackImage.SetActive(false);
            overlayCamera.gameObject.SetActive(true);

            if (_sceneFadeInOut == null)
            {
                await InitFadeOut();
            }

            await _sceneFadeInOut.FadeOut();
        }

        private async Task InitFadeIn()
        {
            await Instantiate();

            _sceneFadeInOut.InitFadeIn();
        }

        private async Task InitFadeOut()
        {
            await Instantiate();

            _sceneFadeInOut.InitFadeOut();
        }

        private async Task Instantiate()
        {
            var sceneFadeInOutGameObject = await sceneFadeInOutReference
                .InstantiateAsync().Task;

            _sceneFadeInOut = sceneFadeInOutGameObject.GetComponent<SceneFadeInOut>();
        }

        private void Release()
        {
            Addressables.Release(_sceneFadeInOut.gameObject);

            _sceneFadeInOut = null;
        }
    }
}