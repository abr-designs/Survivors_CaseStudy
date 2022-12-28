using System;
using System.Collections.Generic;
using System.Linq;
using Survivors.Weapons;
using Survivors.Weapons.Enums;
using Survivors.Player;
using Survivors.ScriptableObjets.Attacks.Items;
using UnityEngine;

namespace Survivors.Managers
{
    public class WeaponManager : MonoBehaviour
    {
        private static Transform _playerTransform;

        [SerializeField]
        private WeaponProfileScriptableObject[] weaponProfiles;
        private Dictionary<WEAPON_TYPE, int> _weaponIndicies;

        private bool _ready;
        private HashSet<WEAPON_TYPE> _activeWeaponTypes;
        private List<WeaponBase_v2> _activeWeapons;

        //Unity Functions
        //============================================================================================================//
        private void OnEnable()
        {
            PassiveManager.OnScaleChanged += OnScaleChanged;
            _playerTransform = FindObjectOfType<PlayerHealth>().transform;
            
            //Setup Attack Profile Library
            //------------------------------------------------//
            _weaponIndicies = new Dictionary<WEAPON_TYPE, int>(weaponProfiles.Length);

            for (var i = 0; i < weaponProfiles.Length; i++)
            {
                var attackProfile = weaponProfiles[i];
                _weaponIndicies.Add(attackProfile.type, i);
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
            
            for (var i = 0; i < _activeWeapons.Count; i++)
            {
                var attackBase = _activeWeapons[i];
                
                attackBase.ManualUpdate(deltaTime);
                attackBase.PostUpdate();
            }
        }

        private void OnDisable()
        {
            PassiveManager.OnScaleChanged -= OnScaleChanged;
        }

        //============================================================================================================//

        private void AddNewAttack(in WEAPON_TYPE weaponType)
        {
            if (_activeWeapons == null)
            {
                _activeWeapons = new List<WeaponBase_v2>();
                _activeWeaponTypes = new HashSet<WEAPON_TYPE>();
                _ready = true;
            }

            //------------------------------------------------//
            
            if (_activeWeaponTypes.Contains(weaponType))
            {
                LevelUpWeapon(weaponType);
                return;
            }
            
            //------------------------------------------------//

            var profileIndex = _weaponIndicies[weaponType];
            var attackProfile = weaponProfiles[profileIndex];
            
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
            
            _activeWeapons.Add(newWeapon);
            _activeWeaponTypes.Add(weaponType);
        }
        
        //============================================================================================================//

        private void LevelUpWeapon(in WEAPON_TYPE weaponType)
        {
            var weapon = FindWeapon(weaponType);
            weapon?.LevelUp();
        }

        private WeaponBase_v2 FindWeapon(in WEAPON_TYPE weaponType)
        {
            switch (weaponType)
            {
                case WEAPON_TYPE.AXE:
                    return _activeWeapons.FirstOrDefault(a => a is AxeWeapon);
                case WEAPON_TYPE.CROSS:
                    return _activeWeapons.FirstOrDefault(a => a is CrossWeapon);
                case WEAPON_TYPE.RADIUS:
                    return _activeWeapons.FirstOrDefault(a => a is RadiusWeapon);
                case WEAPON_TYPE.WHIP:
                    return _activeWeapons.FirstOrDefault(a => a is WhipWeapon);
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
        }

        //Callbacks
        //============================================================================================================//
        
        private void OnScaleChanged(float newScale)
        {
            for (int i = 0; i < _activeWeapons.Count; i++)
            {
                _activeWeapons[i].OnScaleChanged(newScale);
            }
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