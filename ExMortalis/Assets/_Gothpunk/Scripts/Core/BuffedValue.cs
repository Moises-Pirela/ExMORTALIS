using System;
using UnityEngine;

namespace Transendence.Core
{
    [Serializable]
    public struct BuffedValue<T>
    {
        public T BaseValue;
        public T FlatBonus;
        public T PercentBonus;

        public BuffedValue(T baseValue)
        {
            BaseValue = baseValue;
            FlatBonus = default(T);
            PercentBonus = default(T);
        }

        public T CalculateValue()
        {
            if (!(BaseValue is IConvertible && FlatBonus is IConvertible && PercentBonus is IConvertible))
            {
                throw new InvalidOperationException("Type must support arithmetic operations.");
            }

            double baseVal = Convert.ToDouble(BaseValue);
            double flatBon = Convert.ToDouble(FlatBonus);
            double percentBon = Convert.ToDouble(PercentBonus);

            double calculatedValue = baseVal + flatBon + (baseVal * percentBon / 100.0);

            return (T)Convert.ChangeType(calculatedValue, typeof(T));
        }
    }
}


