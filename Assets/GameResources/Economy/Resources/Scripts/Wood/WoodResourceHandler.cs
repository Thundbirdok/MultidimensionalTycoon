using System;

namespace GameResources.Economy.Resources.Scripts.Wood
{
    public sealed class WoodResourceHandler : IResourceHandler
    {
        public event Action OnChangeValue;
        public event Action OnNotEnough;

        public Type ResourceType => typeof(Wood);
        
        public int Value => _handler.Value;

        public string Key => "Wood";

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

        private void OnChangeValueInvoke() => OnChangeValue?.Invoke();

        private void OnNotEnoughInvoke() => OnNotEnough?.Invoke();
    }
}
