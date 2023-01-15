using System;
using System.Collections.Generic;
using System.Linq;
using Survivors.Managers.MonoBehaviours;
using Survivors.Player;
using Survivors.Weapons;
using Survivors.Weapons.Enums;
using Survivors.ScriptableObjets.Weapons.Items;
using UnityEngine;

namespace Survivors.Managers
{
    public class WeaponManager
    {
        private readonly WeaponProfileScriptableObject[] weaponProfiles;
        private Dictionary<WEAPON_TYPE, int> _weaponIndicies;

        private bool _ready;
        private HashSet<WEAPON_TYPE> _activeWeaponTypes;
        private List<WeaponBase_v2> _activeWeapons;

        private Transform _playerTransform;
        private SpriteRenderer _playerSpriteRenderer;

        public WeaponManager(in WeaponProfileScriptableObject[] weaponProfiles)
        {
            this.weaponProfiles = weaponProfiles;
            PlayerManager.OnPlayerCreated += OnPlayerCreated;
        }

        //Unity Functions
        //============================================================================================================//
        public void OnEnable()
        {
            PassiveManager.OnScaleChanged += OnScaleChanged;
            
            //Setup Attack Profile Library
            //------------------------------------------------//
            _weaponIndicies = new Dictionary<WEAPON_TYPE, int>(weaponProfiles.Length);

            for (var i = 0; i < weaponProfiles.Length; i++)
            {
                var weaponProfile = weaponProfiles[i];
                _weaponIndicies.Add(weaponProfile.type, i);
            }
        }

        public void Update()
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

        public void OnDisable()
        {
            PassiveManager.OnScaleChanged -= OnScaleChanged;
            PlayerManager.OnPlayerCreated -= OnPlayerCreated;
        }

        //============================================================================================================//

        public int GetWeaponLevel(in WEAPON_TYPE weaponType)
        {
            var weapon = FindWeapon(weaponType);
            
            return weapon == null ? 0 : weapon.Level;
        }

        public string GetLevelUpText(in WEAPON_TYPE weaponType, in int nextLevel)
        {
            var weapon = FindWeapon(weaponType);

            if (weapon == null)
                throw new Exception();

            var index = _weaponIndicies[weaponType];

            return weaponProfiles[index].GetLevelUpText(nextLevel);
        }

        public void AddNewWeapon(in WEAPON_TYPE weaponType)
        {
            if (_activeWeapons == null)
            {
                _activeWeapons = new List<WeaponBase_v2>();
                _activeWeaponTypes = new HashSet<WEAPON_TYPE>();
                //_ready = true;
            }

            //------------------------------------------------//
            
            if (_activeWeaponTypes.Contains(weaponType))
            {
                var weapon = FindWeapon(weaponType);
                weapon.Level++;
                weapon.LevelUp();
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
                    newWeapon = new WhipWeapon(_playerSpriteRenderer, attackProfile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
            
            _activeWeapons.Add(newWeapon);
            _activeWeaponTypes.Add(weaponType);
        }
        
        //============================================================================================================//

        private WeaponBase_v2 FindWeapon(in WEAPON_TYPE weaponType)
        {
            if (_activeWeapons == null)
                return default;
            
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
        
        private void OnPlayerCreated(Transform playerTransform, PlayerHealth _, SpriteRenderer playerSpriteRenderer)
        {
            _playerTransform = playerTransform;
            _playerSpriteRenderer = playerSpriteRenderer;
            _ready = true;
        }
        
        private void OnScaleChanged(float newScale)
        {
            if(_activeWeapons == null)
                return;
            
            for (int i = 0; i < _activeWeapons.Count; i++)
            {
                _activeWeapons[i].OnScaleChanged(newScale);
            }
        }
        
        //============================================================================================================//
    }
}