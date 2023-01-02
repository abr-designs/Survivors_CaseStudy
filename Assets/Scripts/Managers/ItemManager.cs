using System.Collections.Generic;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using Survivors.Passives.Enums;
using Survivors.Player;
using Survivors.ScriptableObjets.Weapons.Items;
using Survivors.ScriptableObjets.Items;
using Survivors.UI;
using Survivors.Weapons;
using Survivors.Weapons.Enums;
using UnityEngine;

namespace Survivors.Managers
{
    public class ItemManager : ManagerBase, IEnable, IUpdate
    {
        public static Transform PlayerTransform
        {
            get
            {
                if (_playerTransform == null)
                {
                    var player = Object.FindObjectOfType<PlayerHealth>();
                    _playerTransform = player?.transform;
                }

                return _playerTransform;
            }
        }
        private static Transform _playerTransform;
        
        
        private static WeaponManager _weaponManager;
        private static PassiveManager _passiveManager;

        private static WeaponProfileScriptableObject[] WeaponProfiles{get; set;}
        public static PassiveItemProfileScriptableObject[] PassiveProfiles{get; private set;}
        
        //Unity Functions
        //============================================================================================================//

        public ItemManager(in MonoBehaviour coroutineController, WeaponProfileScriptableObject[] weaponProfiles, PassiveItemProfileScriptableObject[] passiveProfiles)
        {
            WeaponProfiles = weaponProfiles;
            PassiveProfiles = passiveProfiles;
            
            _weaponManager = new WeaponManager(weaponProfiles);
            _passiveManager = new PassiveManager(passiveProfiles);
            
            //Setup Attack Base
            //------------------------------------------------//
            WeaponBase_v2.CoroutineController = coroutineController;
        }

        public void OnEnable()
        {
            UIManager.OnItemSelected += OnItemSelected;
            _weaponManager.OnEnable();
            _passiveManager.OnEnable();
        }

        public void Update()
        {
            _weaponManager.Update();
        }

        public void OnDisable()
        {
            UIManager.OnItemSelected -= OnItemSelected;
            
            _weaponManager.OnDisable();
            PassiveManager.Reset();
        }

        //============================================================================================================//
        
        public static string GetItemAltTitleText(in ItemBaseScriptableObject item)
        {
            var level = 0;
            switch (item)
            {
                case WeaponProfileScriptableObject weapon when level > 1:
                    level = _weaponManager.GetWeaponLevel(weapon.type);
                    break;
                case WeaponProfileScriptableObject weapon:
                    level = _weaponManager.GetWeaponLevel(weapon.type);
                    break;
                case PassiveItemProfileScriptableObject passive:
                    level = _passiveManager.GetPassiveLevel(passive.type);
                    break;
            }

            return level == 0 ? "<color=yellow>new!</color>" : $"level: {level + 1}";
        }
        
        public static string GetItemAltDescriptionText(in ItemBaseScriptableObject item)
        {
            switch (item)
            {
                case WeaponProfileScriptableObject weapon:
                    var level = _weaponManager.GetWeaponLevel(weapon.type);
                    return level >= 1 ? _weaponManager.GetLevelUpText(weapon.type, level + 1) : item.description;
                default:
                    return item.description;
            }
        }
        
        //Callbacks
        //============================================================================================================//

        public static void AddStarters(
            in IEnumerable<WEAPON_TYPE> startingWeapons,
            in IEnumerable<PASSIVE_TYPE> startingPassives)
        {
            foreach (var startingWeapon in startingWeapons)
            {
                _weaponManager.AddNewWeapon(startingWeapon);
            }
            foreach (var startingPassive in startingPassives)
            {
                _passiveManager.AddNewPassive(startingPassive);
            }
        }

        private static void OnItemSelected(ItemBaseScriptableObject item)
        {
            switch (item)
            {
                case WeaponProfileScriptableObject weapon:
                    _weaponManager.AddNewWeapon(weapon.type);
                    break;
                case PassiveItemProfileScriptableObject passive:
                    _passiveManager.AddNewPassive(passive.type);
                    break;
            }
        }
        
        //============================================================================================================//
    }
}