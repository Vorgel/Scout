﻿using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace Scout.Helpers
{
    public static class PropertyChangedBaseExtensions
    {
        public static bool Set<T>(this PropertyChangedBase propertyChangedBase, ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(oldValue, newValue))
            {
                return false;
            }

            oldValue = newValue;
            propertyChangedBase.NotifyOfPropertyChange(propertyName);

            return true;
        }
    }
}
