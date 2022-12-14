using System;
using System.Collections.Generic;
using GameResources.Control.Building.Scripts;
using UnityEngine;

namespace GameResources.Control.Builder.Scripts
{
    [CreateAssetMenu(fileName = "BuildingsInteractedValuesHandler", menuName = "Builder/BuildingsInteractedValuesHandler")]
    public sealed class BuildingsInteractedValuesHandler : ScriptableObject
    {
        public event Action OnUpdateObjects;
        
        [NonSerialized]
        private List<BuildingsInteractionEventData> _eventsData 
            = new List<BuildingsInteractionEventData>();

        public List<BuildingsInteractionEventData> EventsData
        {
            get
            {
                return _eventsData;
            }

            set
            {
                _eventsData = value;
                
                OnUpdateObjects?.Invoke();
            }
        }

        public void Clear()
        {
            _eventsData.Clear();
            
            OnUpdateObjects?.Invoke();
        }
    }
}
