using System.Collections;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Attacks
{
    public abstract class AttackBase_v2
    {
        internal static MonoBehaviour CoroutineController;
        internal static Vector2 PlayerPosition;
        private int Level = 1;

        
        protected float range;
        protected float damage;

        private float cooldown;
        private float _cooldownTimer;

        

        //============================================================================================================//
        internal AttackBase_v2(in AttackProfileScriptableObject attackProfile)
        {
            range = attackProfile.range;
            damage = attackProfile.damage;
            cooldown = attackProfile.cooldown;
        }
        
        protected abstract void LevelUp();

        public void ManualUpdate(in float deltaTime)
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= deltaTime;
                return;
            }

            TriggerAttack();
            _cooldownTimer = cooldown;
        }

        public abstract void PostUpdate();

        protected abstract void TriggerAttack();

        protected static Coroutine StartCoroutine(in IEnumerator coroutine) => CoroutineController.StartCoroutine(coroutine);
    }
}