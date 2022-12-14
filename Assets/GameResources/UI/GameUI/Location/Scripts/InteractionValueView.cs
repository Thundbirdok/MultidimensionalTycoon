using GameResources.Control.Building.Scripts;
using TMPro;
using UnityEngine;

namespace GameResources.UI.GameUI.Location.Scripts
{
    public sealed class InteractionValueView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public BuildingsInteractionEventData BuildingsInteractionEventData { get; private set; }

        public void SetValue(BuildingsInteractionEventData buildingsInteractionEventData)
        {
            BuildingsInteractionEventData = buildingsInteractionEventData;
            
            text.text = buildingsInteractionEventData.Value.ToString();
        }
    }
}
