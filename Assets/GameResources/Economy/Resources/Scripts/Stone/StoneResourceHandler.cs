using System;

namespace GameResources.Economy.Resources.Scripts.Stone
{
    public sealed class StoneResourceHandler : IResourceHandler
    {
        public event Action OnChangeValue;
        public event Action OnNotEnough;

        public int Value => _handler.Value;

        public Type ResourceType => typeof(Stone);
        public string Key => "Gems";

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

        private void OnChangeValueInvoke() => OnChangeValue?.Invoke();

        private void OnNotEnoughInvoke() => OnNotEnough?.Invoke();
    }
}
