using System;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.UI.GameUI.Location.Scripts
{
    public sealed class BuildingView : MonoBehaviour
    {
        public event Action<BuildingViewData> OnButtonClick;

        public BuildingViewData Data { get; private set; }

        private int _amount;
        public int Amount
        {
            get
            {
                return _amount;
            }

            set
            {
                if (_amount == value)
                {
                    return;
                }
                
                _amount = value;
                
                SetAmountText(_amount);
            }
        }

        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI amountText;

        [SerializeField]
        private LeanButton button;

        private void OnEnable() => button.OnClick.AddListener(InvokeOnButtonClick);

        private void OnDisable() => button.OnClick.RemoveListener(InvokeOnButtonClick);

        public void Set(BuildingViewData data, int amount)
        {
            Data = data;
            Amount = amount;
            
            image.sprite = Data.Icon;

            SetAmountText(amount);
        }

        private void SetAmountText(int amount)
        {
            amountText.text = amount.ToString();
        }

        private void InvokeOnButtonClick() => OnButtonClick?.Invoke(Data);
    }
}
