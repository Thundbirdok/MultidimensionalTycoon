namespace GameResources.Control.BuildingPacks
{
    using System;
    using System.Collections.Generic;
    using GameResources.Control.Builder.Scripts;

    [Serializable]
    public class BuildingsPack
    {
        public List<BuildingSlot> Slots = new List<BuildingSlot>();
    }
}
