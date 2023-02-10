using System;
using GameResources.Control.Economy.Resources.Scripts;

namespace GameResources.Control.Economy.Resources.Stone
{
    public sealed class StoneResourceHandler : IResourceHandler
    {
        public event Action OnValueChanged;
        public event Action OnNotEnough;

        public int Value => _handler.Value;

        public IResourceType ResourceType { get; } = new Stone();

        private IntValueHandler _handler;

        public StoneResourceHandler()
        {
            _handler = new IntValueHandler(0);

            _handler.OnChange += OnChangeValueInvoke;
            _handler.OnNotEnough += OnNotEnoughInvoke;
        }

        ~StoneResourceHandler()
        {
            _handler.OnChange -= OnChangeValueInvoke;
            _handler.OnNotEnough -= OnNotEnoughInvoke;

            _handler = null;
        }

        public void Add(int value) => _handler.Add(value);

        public bool Spend(int value) => _handler.Subtract(value);

        public void ChangeWithoutNotify(int value) => _handler.ChangeWithoutNotify(value);

        private void OnChangeValueInvoke() => OnValueChanged?.Invoke();

        private void OnNotEnoughInvoke() => OnNotEnough?.Invoke();
    }
}
