using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Damage.Resistance
{
    [Serializable]
    public class StateDamageResistance
    {
        [SerializeField] private int _stateId;
        [SerializeField] private DamageResistance _resistance = new();

        public int StateId => _stateId;
        public DamageResistance Resistance => _resistance;
    }

    [Serializable]
    public class DamageResistanceSheet
    {
        [SerializeField] private bool _hasDefault;
        [SerializeField] private DamageResistance _default = new();
        [SerializeField] private List<StateDamageResistance> _byState = new();

        public DamageResistance GetFor(int stateId)
        {
            for (int i = 0; i < _byState.Count; i++)
            {
                StateDamageResistance entry = _byState[i];
                if (entry.StateId == stateId)
                {
                    return entry.Resistance;
                }
            }

            return _hasDefault ? _default : DamageResistance.None;
        }
    }
}
