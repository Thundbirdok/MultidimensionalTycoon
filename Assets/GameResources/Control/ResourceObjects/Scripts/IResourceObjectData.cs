namespace GameResources.Control.ResourceObjects.Scripts
{
    public interface IResourceObjectData
    {
        public string Key { get; }

        public int Size { get; }

        public bool TryGetValue(IResourceObjectData data, out int value);
    }
}
