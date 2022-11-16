namespace GameResources.Economy.Resources.Scripts
{
    public class ResourceValue
    {
        public int Value { get; }

        public IResource Type { get; }

        public ResourceValue(IResource type, int value)
        {
            Type = type;
            Value = value;
        }
    }
}
