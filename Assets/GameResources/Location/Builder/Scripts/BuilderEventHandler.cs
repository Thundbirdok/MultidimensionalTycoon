using System;
using GameResources.Location.Building.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Builder.Scripts
{
    [CreateAssetMenu(fileName = "BuilderEventHandler", menuName = "Builder/BuilderEventHandler")]
    public class BuilderEventHandler : ScriptableObject
    {
        public event Action<BuildingData> OnStartBuilding;
        public event Action OnStopBuilding;
        
        public event Action<BuildingData> OnChooseBuilding;
        public event Action OnAccept;
        public event Action OnCancel;
        public event Action<bool> OnValidPosition;

        public void InvokeStartBuilding(BuildingData building)
        {
            OnStartBuilding?.Invoke(building);
        }

        public void InvokeStopBuilding()
        {
            OnStopBuilding?.Invoke();
        }
        
        public void InvokeChooseBuilding(BuildingData building)
        {
            OnChooseBuilding?.Invoke(building);
        }

        public void InvokeAccept() => OnAccept?.Invoke();

        public void InvokeCancel() => OnCancel?.Invoke();

        public void InvokeValidPosition(bool isValid) => OnValidPosition?.Invoke(isValid);
    }
}
