using System;
using System.Collections.Generic;

namespace UnderworldExporter.Game
{
    public abstract class PrefsValue <T>
    {
        private T _currentValue;
        private readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;
        protected readonly string _prefsKey;

        protected PrefsValue(string prefsKey)
        {
            
            if (string.IsNullOrEmpty(prefsKey))
            {
                throw new ArgumentNullException(nameof(prefsKey));
            }
            
            _prefsKey = prefsKey;
        }

        public T Value
        {
            get
            {
                if (_comparer.Equals(_currentValue, default))
                {
                    _currentValue = GetSharedPrefsValue();
                }

                return _currentValue;
            }

            set
            {
                _currentValue = value;
                SetSharedPrefsValue(value);
            }
        }
        
        protected abstract T GetSharedPrefsValue();

        protected abstract void SetSharedPrefsValue(T value);
    }
}