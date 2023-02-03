using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameResources.Economy.Resources.Stone
{
    public sealed class StoneView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI resourceName;

        [SerializeField]
        private TextMeshProUGUI value;

        private StoneResourceHandler _handler;

        private bool _isSubscribed;
        private bool _isRed;

        [Inject]
        private void Construct(StoneResourceHandler stoneHandler)
        {
            _handler = stoneHandler;

            Subscribe();

            ShowValueChanged();

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

            ShowValueChanged();
        }

        private void OnDisable() => Unsubscribe();

        private void Subscribe()
        {
            if (_isSubscribed)
            {
                return;
            }

            _handler.OnValueChanged += ShowValueChanged;
            _handler.OnNotEnough += TurnRed;

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (_handler != null)
            {
                _handler.OnValueChanged -= ShowValueChanged;
                _handler.OnNotEnough -= TurnRed;
            }

            _isSubscribed = false;
        }

        private void ShowValueChanged()
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
