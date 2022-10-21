using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameResources.Economy.Gems
{
    public sealed class GemsView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI resourceName;

        [SerializeField]
        private TextMeshProUGUI value;

        private GemsResourceHandler handler;

        private bool isSubscribed;
        private bool isRed;

        [Inject]
        private void Construct(GemsResourceHandler gemsHandler)
        {
            handler = gemsHandler;

            Subscribe();

            ShowValue();

            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            if (handler == null)
            {
                gameObject.SetActive(false);

                return;
            }

            Subscribe();

            ShowValue();
        }

        private void OnDisable() => Unsubscribe();

        private void Subscribe()
        {
            if (isSubscribed)
            {
                return;
            }

            handler.OnChangeValue += ShowValue;
            handler.OnNotEnough += TurnRed;

            isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (handler != null)
            {
                handler.OnChangeValue -= ShowValue;
                handler.OnNotEnough -= TurnRed;
            }

            isSubscribed = false;
        }

        private void ShowValue()
        {
            value.text = handler.Value.ToString();
        }

        private async void TurnRed()
        {
            if (isRed)
            {
                return;
            }

            isRed = true;

            var valueDefaultColor = value.color;
            value.color = Color.red;

            var nameDefaultColor = resourceName.color;
            resourceName.color = Color.red;

            await Task.Delay(500);

            value.color = valueDefaultColor;
            resourceName.color = nameDefaultColor;

            isRed = false;
        }
    }
}
