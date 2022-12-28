using System;
using System.Collections.Generic;
using Survivors.Weapons;
using Survivors.Weapons.Enums;
using Survivors.Player;
using Survivors.ScriptableObjets.Attacks;
using UnityEngine;

namespace Survivors.Managers
{
    public class AttackManager : MonoBehaviour
    {
        private static Transform _playerTransform;

        [SerializeField]
        private WeaponProfileScriptableObject[] _attackProfiles;
        private Dictionary<WEAPON_TYPE, int> _attackIndicies;

        private bool _ready;
        private HashSet<WEAPON_TYPE> _activeAttackTypes;
        private List<WeaponBase_v2> _activeAttacks;

        //Unity Functions
        //============================================================================================================//
        private void OnEnable()
        {
            _playerTransform = FindObjectOfType<PlayerHealth>().transform;
            
            //Setup Attack Profile Library
            //------------------------------------------------//
            _attackIndicies = new Dictionary<WEAPON_TYPE, int>(_attackProfiles.Length);

            for (var i = 0; i < _attackProfiles.Length; i++)
            {
                var attackProfile = _attackProfiles[i];
                _attackIndicies.Add(attackProfile.type, i);
            }

            //Setup Attack Base
            //------------------------------------------------//
            WeaponBase_v2.CoroutineController = this;
        }

        private void Update()
        {
            if (_ready == false)
                return;
            
            WeaponBase_v2.PlayerPosition = _playerTransform.position;
            var deltaTime = Time.deltaTime;
            
            for (var i = 0; i < _activeAttacks.Count; i++)
            {
                var attackBase = _activeAttacks[i];
                
                attackBase.ManualUpdate(deltaTime);
                attackBase.PostUpdate();
            }
        }
        //============================================================================================================//

        private void AddNewAttack(in WEAPON_TYPE weaponType)
        {
            if (_activeAttacks == null)
            {
                _activeAttacks = new List<WeaponBase_v2>();
                _activeAttackTypes = new HashSet<WEAPON_TYPE>();
                _ready = true;
            }

            if (_activeAttackTypes.Contains(weaponType))
                throw new Exception();

            var profileIndex = _attackIndicies[weaponType];
            var attackProfile = _attackProfiles[profileIndex];
            
            WeaponBase_v2 newWeapon;
            switch (weaponType)
            {
                case WEAPON_TYPE.AXE:
                    newWeapon = new AxeWeapon(attackProfile);
                    break;
                case WEAPON_TYPE.CROSS:
                    newWeapon = new CrossWeapon(attackProfile);
                    break;
                case WEAPON_TYPE.RADIUS:
                    newWeapon = new RadiusWeapon(attackProfile);
                    break;
                case WEAPON_TYPE.WHIP:
                    newWeapon = new WhipWeapon(attackProfile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
            
            _activeAttacks.Add(newWeapon);
            _activeAttackTypes.Add(weaponType);
        }
        
        //============================================================================================================//

#if UNITY_EDITOR

        [SerializeField, Header("Debugging")]
        private WEAPON_TYPE weaponToAdd;

        [ContextMenu("Add Test Attack")]
        private void AddTestAttack()
        {
            AddNewAttack(weaponToAdd);
        }
        
#endif
    }
}