using System;
using GameResources.Control.Scripts;

namespace GameResources.Control.Builder.Scripts
{
    [Serializable]
    public class BuildingSlot
    {
        public BuildingData Data;
        public int Amount;

        public BuildingSlot(BuildingData data, int amount)
        {
            Data = data;
            Amount = amount;
        }
    }
}