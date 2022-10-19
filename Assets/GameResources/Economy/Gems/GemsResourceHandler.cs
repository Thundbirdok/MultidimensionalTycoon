using System;
using GameResources.Utility.Scripts;

namespace GameResources.Economy.Gems
{
    public sealed class GemsResourceHandler : IResourceHandler
    {
        public event Action OnChangeValue;
        public event Action OnNotEnough;

        public int Value => handler.Value;

        public string Key => "Gems";

        private IntValueHandler handler;

        public GemsResourceHandler()
        {
            handler = new IntValueHandler(0);

            handler.OnChange += OnChangeValueInvoke;
            handler.OnNotEnough += OnNotEnoughInvoke;
        }

        ~GemsResourceHandler()
        {
            handler.OnChange -= OnChangeValueInvoke;
            handler.OnNotEnough -= OnNotEnoughInvoke;

            handler = null;
        }

        public void Add(int value) => handler.Add(value);

        public bool Spend(int value) => handler.Subtract(value);

        public void ChangeWithoutNotify(int value) => handler.ChangeWithoutNotify(value);

        private void OnChangeValueInvoke() => OnChangeValue?.Invoke();

        private void OnNotEnoughInvoke() => OnNotEnough?.Invoke();
    }
}