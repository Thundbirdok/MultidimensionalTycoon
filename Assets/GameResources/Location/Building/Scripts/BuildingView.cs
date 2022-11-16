using System;
using System.Collections.Generic;
using System.Text;
using GameResources.Economy.Resources.Scripts;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.Location.Building.Scripts
{
    public class BuildingView : MonoBehaviour
    {
        public event Action<BuildingViewData> OnButtonClick;
        
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI priceText;

        [SerializeField]
        private LeanButton button;

        private BuildingViewData _data;
        
        private void OnEnable() => button.OnClick.AddListener(InvokeOnButtonClick);

        private void OnDisable() => button.OnClick.RemoveListener(InvokeOnButtonClick);

        public void Set(BuildingViewData data)
        {
            _data = data;
            
            image.sprite = _data.Icon;

            SetPriceText(_data.Price);
        }

        private void SetPriceText(List<ResourceValue> price)
        {
            if (price == null || price.Count == 0)
            {
                priceText.text = "Free";
                
                return;
            }
            
            var sb = new StringBuilder();

            foreach (var resource in price)
            {
                sb.Append(resource.Type.Key);
                sb.Append(": ");
                sb.Append(resource.Value);
                sb.Append(" ");
            }

            priceText.text = sb.ToString();
        }
        
        private void InvokeOnButtonClick() => OnButtonClick?.Invoke(_data);
    }
}
