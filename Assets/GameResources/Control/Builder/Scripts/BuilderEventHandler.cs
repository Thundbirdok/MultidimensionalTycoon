using System;
using GameResources.Control.Building.Scripts;
using UnityEngine;

namespace GameResources.Control.Builder.Scripts
{
    [CreateAssetMenu(fileName = "BuilderEventHandler", menuName = "Builder/BuilderEventHandler")]
    public sealed class BuilderEventHandler : ScriptableObject
    {
        public event Action<BuildingData> OnStartBuilding;
        public event Action OnStopBuilding;
        
        public event Action<BuildingData> OnChooseBuilding;
        public event Action OnAccept;
        public event Action OnCancel;
        public event Action<bool> OnValidPosition;
        public event Action<BuildingSlot> OnBuild;
        
        public event Action OnAddPack;
        public event Action OnRequestAddPack;
        public event Action OnNoBuildings;

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

        public void InvokeBuild(BuildingSlot buildingSlot) => OnBuild?.Invoke(buildingSlot);

        public void InvokeAddPack() => OnAddPack?.Invoke();
        public void InvokeRequestAddPack() => OnRequestAddPack?.Invoke();
        
        public void InvokeNoBuildings() => OnNoBuildings?.Invoke();
    }
}
