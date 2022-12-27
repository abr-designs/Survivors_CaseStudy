using UnityEngine;

namespace Survivors.Attacks
{
    //TODO Consider changing this to be a ScriptableObject
    public abstract class AttackBase : MonoBehaviour
    {
        protected Vector2 PlayerPosition;

        [SerializeField, Header("Attack Base")]
        public bool isActive;
        [SerializeField, Min(1)]
        protected int level = 1;
        
        [SerializeField, Min(0)]
        protected float range;

        [SerializeField, Min(0)]
        protected float damage;

        [SerializeField, Min(0)]
        protected float cooldown;
        private float _cooldownTimer;

        //============================================================================================================//
        public abstract void Setup();
        protected abstract void LevelUp();

        public void ManualUpdate(in Vector2 playerPosition, in float deltaTime)
        {
            PlayerPosition = playerPosition;

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

        public virtual void SetActive(in bool state)
        {
            isActive = state;
        }
        //Unity Editor Functions
        //============================================================================================================//
#if UNITY_EDITOR

        [ContextMenu("Toggle Attack Active")]
        private void Toggle()
        {
            SetActive(!isActive);
        }

#endif
    }
}
