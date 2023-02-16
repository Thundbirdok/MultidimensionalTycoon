using GameResources.Control.Economy.Resources.Scripts;

namespace GameResources.Control.ResourceObjects.Scripts
{
    public interface IResourceObjectData
    {
        public string Key { get; }

        public int Size { get; }

        public bool TryGetInteractionValue(IResourceObjectData data, out Resource value);
    }
}
