using System.Collections;
using System.Collections.Generic;
using Survivors.Player;
using UnityEngine;

namespace Survivors.Attacks
{
    public abstract class AttackBase : MonoBehaviour
    {
        protected static Transform PlayerTransform
        {
            get
            {
                if (_foundPlayer)
                    return _playerTransform;

                _playerTransform = FindObjectOfType<PlayerHealth>().transform;
                _foundPlayer = true;
                return _playerTransform;
            }
        }
        private static Transform _playerTransform;
        private static bool _foundPlayer;

        [SerializeField]
        protected bool IsActive;
        [SerializeField]
        protected int Level;

        [SerializeField]
        protected float Damage;

        [SerializeField]
        protected float Cooldown;
        private float _cooldownTimer;

        public void ManualUpdate(in float deltaTime)
        {
            if (IsActive == false)
                return;

            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= deltaTime;
                return;
            }
        }

        protected abstract void TriggerAttack();
    }
}
