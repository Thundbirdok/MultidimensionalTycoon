using GameResources.Control.Building.Scripts;
using TMPro;
using UnityEngine;

namespace GameResources.UI.GameUI.Location.Scripts
{
    using System.Text;

    public sealed class InteractionValueView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public BuildingsInteractionEventData BuildingsInteractionEventData { get; private set; }

        public void SetValue(BuildingsInteractionEventData buildingsInteractionEventData)
        {
            BuildingsInteractionEventData = buildingsInteractionEventData;
            
            var sb = new StringBuilder();

            var resources = 
                buildingsInteractionEventData
                .Value
                .Resources;

            foreach (var resource in resources)
            {
                sb.Append(resource.Key.Key[0]);
                sb.Append(' ');
                sb.Append(resource.Value.Value);
                sb.Append('\n');
            }
            
            text.text = sb.ToString();
        }
    }
}
