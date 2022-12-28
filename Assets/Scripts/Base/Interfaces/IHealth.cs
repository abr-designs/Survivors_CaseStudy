using UnityEngine;

namespace Survivors.Base.Interfaces
{
    public interface IHealth
    {
        Transform transform { get; }

        float StartingHealth { get; }
        float MaxHealth { get; }
        float CurrentHealth { get; }
        bool ShowHealthDamage { get; }
        bool ShowHealthBar { get; }
        bool ShowDamageEffect { get; }

        void ChangeHealth(in float healthDelta);

        void Kill();
    }
}