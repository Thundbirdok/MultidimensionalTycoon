using System;
using GameResources.Control.Economy.Resources.Scripts;

namespace GameResources.Control.Economy.Resources.Wood
{
    public sealed class WoodResourceHandler : IResourceHandler
    {
        public event Action OnChangedValue;
        public event Action OnNotEnough;

        public Resource Resource => new Resource(new Wood(), _handler.Value);
        
        public int Value => _handler.Value;
        
        public IResourceType ResourceType { get; } = new Wood();

        private IntValueHandler _handler;

        public WoodResourceHandler()
        {
            _handler = new IntValueHandler(0);

            _handler.OnChange += OnChangeValueInvoke;
            _handler.OnNotEnough += OnNotEnoughInvoke;
        }

        ~WoodResourceHandler()
        {
            _handler.OnChange -= OnChangeValueInvoke;
            _handler.OnNotEnough -= OnNotEnoughInvoke;

            _handler = null;
        }

        public void Add(int value) => _handler.Add(value);

        public bool Spend(int value) => _handler.Subtract(value);

        public void ChangeWithoutNotify(int value) => _handler.ChangeWithoutNotify(value);

        private void OnChangeValueInvoke() => OnChangedValue?.Invoke();

        private void OnNotEnoughInvoke() => OnNotEnough?.Invoke();
    }
}
