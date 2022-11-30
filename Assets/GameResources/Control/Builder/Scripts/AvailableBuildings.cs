using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameResources.Control.Builder.Scripts
{
    [CreateAssetMenu(fileName = "AvailableBuildings", menuName = "Builder/AvailableBuildings")]
    public class AvailableBuildings : ScriptableObject
    {
        public event Action OnAddPack;
        
        private List<BuildingSlot> _slots = new List<BuildingSlot>();
        public IReadOnlyList<BuildingSlot> Slots => _slots;
        
        public void AddPack(in BuildingsPack pack)
        {
            var tmpPack = pack;
            
            foreach (var slot in _slots)
            {
                for (var i = 0; i < tmpPack.Slots.Count; )
                {
                    if (tmpPack.Slots[i].Data == slot.Data)
                    {
                        slot.Amount += tmpPack.Slots[i].Amount;

                        tmpPack.Slots.RemoveAt(i);
                        
                        break;
                    }

                    ++i;
                }
            }
            
            _slots.AddRange(tmpPack.Slots);
            
            OnAddPack?.Invoke();
        }
    }
}
