using System;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.UI.GameUI.Location.Scripts
{
    public class BuildingView : MonoBehaviour
    {
        public event Action<BuildingViewData> OnButtonClick;
        
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI amountText;

        [SerializeField]
        private LeanButton button;

        private BuildingViewData _data;
        
        private void OnEnable() => button.OnClick.AddListener(InvokeOnButtonClick);

        private void OnDisable() => button.OnClick.RemoveListener(InvokeOnButtonClick);

        public void Set(BuildingViewData data, int amount)
        {
            _data = data;
            
            image.sprite = _data.Icon;

            amountText.text = amount.ToString();
        }

        private void InvokeOnButtonClick() => OnButtonClick?.Invoke(_data);
    }
}
