using System;

namespace GameResources.Control.Economy.Resources.Scripts
{
    public interface IResourceHandler
    {
        public event Action OnChangedValue;
        public event Action OnNotEnough;

        public Resource Resource { get; }
        public IResourceType ResourceType { get; }
        public int Value { get; }

        public void Add(int value);
        public bool Spend(int value);

        public void ChangeWithoutNotify(int value);
    }
}
