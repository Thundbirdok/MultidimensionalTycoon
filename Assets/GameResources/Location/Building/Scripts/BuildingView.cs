using System;
using System.Collections.Generic;
using System.Text;
using GameResources.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.Location.Building.Scripts
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI priceText;

        public void Set(Sprite icon, List<ResourceValue> price)
        {
            image.sprite = icon;

            SetPriceText(price);
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
    }
}
