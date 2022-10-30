using System;

namespace GameResources.Economy.Resources.Scripts
{
    public sealed class IntValueHandler
    {
        public event Action OnChange;
        public event Action OnNotEnough;

        private int _value;

        public int Value
        {
            get => _value;

            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;

                OnChange?.Invoke();
            }
        }

        public IntValueHandler(int startValue)
        {
            _value = startValue;
        }

        public void Add(int b) => Value += b;

        public bool Subtract(int b)
        {
            if (Value - b < 0)
            {
                OnNotEnough?.Invoke();

                return false;
            }

            Value -= b;

            return true;
        }

        public void ChangeWithoutNotify(int newValue) => _value = newValue;
    }
}
