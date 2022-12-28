using System;
using UnityEngine;

namespace Survivors.Managers
{
    public class PassiveManager : MonoBehaviour
    {
        public static event Action<float> OnScaleChanged;
        public static event Action<float> OnMaxHealthChanged; 

        public static float Damage = 1f;
        public static int DamageReduction;
        public static float MaxHealth = 1f;
        public static float Cooldown = 1f;
        public static float AttackArea = 1f;
        public static float ProjectileSpeed = 1f;
        public static int ProjectileAdd;
        public static float MoveSpeed = 1f;

        private void Reset()
        {
            Damage = 1f;
            DamageReduction = 0;
            MaxHealth = 1f;
            Cooldown = 1f;
            AttackArea = 1f;
            ProjectileSpeed = 1f;
            ProjectileAdd = 0;
            MoveSpeed = 1f;
        }
        
    }
}