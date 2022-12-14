using System;
using GameResources.Control.Building.Scripts;

namespace GameResources.Control.Builder.Scripts
{
    [Serializable]
    public sealed class BuildingSlot
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