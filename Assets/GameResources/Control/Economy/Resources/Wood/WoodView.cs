using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameResources.Control.Economy.Resources.Wood
{
    public sealed class WoodView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI resourceName;

        [SerializeField]
        private TextMeshProUGUI value;

        private WoodResourceHandler _handler;

        private bool _isSubscribed;
        private bool _isRed;

        [Inject]
        private void Construct(WoodResourceHandler woodHandler)
        {
            _handler = woodHandler;

            Subscribe();

            ShowValue();

            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            if (_handler == null)
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
            if (_isSubscribed)
            {
                return;
            }

            _handler.OnChangedValue += ShowValue;
            _handler.OnNotEnough += TurnRed;

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (_handler != null)
            {
                _handler.OnChangedValue -= ShowValue;
                _handler.OnNotEnough -= TurnRed;
            }

            _isSubscribed = false;
        }

        private void ShowValue()
        {
            value.text = _handler.Value.ToString();
        }

        private async void TurnRed()
        {
            if (_isRed)
            {
                return;
            }

            _isRed = true;

            var valueDefaultColor = value.color;
            value.color = Color.red;

            var nameDefaultColor = resourceName.color;
            resourceName.color = Color.red;

            await Task.Delay(500);

            value.color = valueDefaultColor;
            resourceName.color = nameDefaultColor;

            _isRed = false;
        }
    }
}
