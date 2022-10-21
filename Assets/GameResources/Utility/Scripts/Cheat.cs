using UnityEngine;
using UnityEngine.UI;

namespace GameResources.Utility.Scripts
{
    public sealed class Cheat : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private Button open;

        [SerializeField]
        private Button close;

        private const int CLICKS_TO_OPEN = 5;

        private int _clickCounter;

        private float _lastClickTime;

        private void OnEnable()
        {
            OpenPanel(false);

            open.onClick.AddListener(CountClick);
            close.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            open.onClick.RemoveListener(CountClick);
            close.onClick.RemoveListener(Close);
        }

        private void CountClick()
        {
            if (Time.time - _lastClickTime > 1)
            {
                _clickCounter = 0;
            }

            _lastClickTime = Time.time;

            ++_clickCounter;

            if (_clickCounter < CLICKS_TO_OPEN)
            {
                return;
            }

            _clickCounter = 0;

            OpenPanel(true);
        }

        private void Close()
        {
            OpenPanel(false);
        }

        private void OpenPanel(bool isPanelOpen)
        {
            panel.SetActive(isPanelOpen);
            open.gameObject.SetActive(isPanelOpen == false);
        }
    }
}
