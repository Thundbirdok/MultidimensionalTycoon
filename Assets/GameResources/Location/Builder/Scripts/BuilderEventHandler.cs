using System;
using GameResources.Location.Building.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Builder.Scripts
{
    [CreateAssetMenu(fileName = "BuilderEventHandler", menuName = "Builder/BuilderEventHandler")]
    public class BuilderEventHandler : ScriptableObject
    {
        public event Action<BuildingData> OnChooseBuilding;

        public void InvokeChooseBuilding(BuildingData building)
        {
            OnChooseBuilding?.Invoke(building);
        }
    }
}
