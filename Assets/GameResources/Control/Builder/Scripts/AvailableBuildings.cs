using System;
using System.Collections.Generic;
using System.Linq;
using GameResources.Control.Building.Scripts;
using UnityEngine;

namespace GameResources.Control.Builder.Scripts
{
    using GameResources.Control.BuildingPacks;

    [CreateAssetMenu(fileName = "AvailableBuildings", menuName = "Builder/AvailableBuildings")]
    public sealed class AvailableBuildings : ScriptableObject
    {
        [SerializeField]
        private BuilderEventHandler builderEventHandler;

        [NonSerialized]
        private readonly List<BuildingSlot> _slots = new List<BuildingSlot>();
        public IReadOnlyList<BuildingSlot> Slots => _slots;
        
        public void AddPack(BuildingsPack pack)
        {
            for (var i = 0; i < pack.Slots.Count; )
            {
                if (TryAddToExistedSlot(pack, i) == false)
                {
                    var packSlot = pack.Slots[i];
                        
                    var newSlot = new BuildingSlot(packSlot.Data, packSlot.Amount);    
                        
                    _slots.Add(newSlot);
                }
                
                ++i;
            }
            
            builderEventHandler.InvokeAddPack();
        }

        public void SpendBuilding(BuildingData building, out int restAmount)
        {
            var slot = _slots.FirstOrDefault(x => x.Data == building);

            if (slot == null)
            {
                restAmount = 0;
                
                return;
            }
            
            --slot.Amount;

            restAmount = slot.Amount;
            
            if (slot.Amount <= 0)
            {
                _slots.Remove(slot);
            }
            
            builderEventHandler.InvokeBuild(slot);
            
            if (_slots.Count > 0)
            {
                return;
            }

            if (IsBuildingsAvailable())
            {
                return;
            }

            builderEventHandler.InvokeNoBuildings();
        }

        private bool IsBuildingsAvailable()
        {
            return _slots.Any(availableSlot => availableSlot.Amount > 0);
        }

        private bool TryAddToExistedSlot(BuildingsPack pack, int i)
        {
            var isFound = false;
            
            foreach (var slot in _slots)
            {
                if (pack.Slots[i].Data != slot.Data)
                {
                    continue;
                }

                slot.Amount += pack.Slots[i].Amount;

                isFound = true;

                break;
            }

            return isFound;
        }
    }
}
