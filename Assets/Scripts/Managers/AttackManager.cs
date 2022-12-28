using System;
using System.Collections.Generic;
using Survivors.Attacks;
using Survivors.Attacks.Enums;
using Survivors.Player;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Managers
{
    public class AttackManager : MonoBehaviour
    {
        private static Transform _playerTransform;

        [SerializeField]
        private AttackProfileScriptableObject[] _attackProfiles;
        private Dictionary<ATTACK_TYPE, int> _attackIndicies;

        private bool _ready;
        private HashSet<ATTACK_TYPE> _activeAttackTypes;
        private List<AttackBase_v2> _activeAttacks;

        //Unity Functions
        //============================================================================================================//
        private void OnEnable()
        {
            _playerTransform = FindObjectOfType<PlayerHealth>().transform;
            
            //Setup Attack Profile Library
            //------------------------------------------------//
            _attackIndicies = new Dictionary<ATTACK_TYPE, int>(_attackProfiles.Length);

            for (var i = 0; i < _attackProfiles.Length; i++)
            {
                var attackProfile = _attackProfiles[i];
                _attackIndicies.Add(attackProfile.type, i);
            }

            //Setup Attack Base
            //------------------------------------------------//
            AttackBase_v2.CoroutineController = this;
        }

        private void Update()
        {
            if (_ready == false)
                return;
            
            AttackBase_v2.PlayerPosition = _playerTransform.position;
            var deltaTime = Time.deltaTime;
            
            for (var i = 0; i < _activeAttacks.Count; i++)
            {
                var attackBase = _activeAttacks[i];
                
                attackBase.ManualUpdate(deltaTime);
                attackBase.PostUpdate();
            }
        }
        //============================================================================================================//

        private void AddNewAttack(in ATTACK_TYPE attackType)
        {
            if (_activeAttacks == null)
            {
                _activeAttacks = new List<AttackBase_v2>();
                _activeAttackTypes = new HashSet<ATTACK_TYPE>();
                _ready = true;
            }

            if (_activeAttackTypes.Contains(attackType))
                throw new Exception();

            var profileIndex = _attackIndicies[attackType];
            var attackProfile = _attackProfiles[profileIndex];
            
            AttackBase_v2 newAttack;
            switch (attackType)
            {
                case ATTACK_TYPE.AXE:
                    newAttack = new AxeAttack(attackProfile);
                    break;
                case ATTACK_TYPE.CROSS:
                    newAttack = new CrossAttack(attackProfile);
                    break;
                case ATTACK_TYPE.RADIUS:
                    newAttack = new RadiusAttack(attackProfile);
                    break;
                case ATTACK_TYPE.WHIP:
                    newAttack = new WhipAttack(attackProfile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
            }
            
            _activeAttacks.Add(newAttack);
            _activeAttackTypes.Add(attackType);
        }
        
        //============================================================================================================//

#if UNITY_EDITOR

        [SerializeField, Header("Debugging")]
        private ATTACK_TYPE attackToAdd;

        [ContextMenu("Add Test Attack")]
        private void AddTestAttack()
        {
            AddNewAttack(attackToAdd);
        }
        
#endif
    }
}