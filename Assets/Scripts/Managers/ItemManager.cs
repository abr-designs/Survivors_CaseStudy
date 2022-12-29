using Survivors.Player;
using Survivors.ScriptableObjets.Attacks.Items;
using Survivors.ScriptableObjets.Items;
using Survivors.UI;
using Survivors.Weapons;
using UnityEngine;

namespace Survivors.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class ItemManager : MonoBehaviour
    {
        private static WeaponManager _weaponManager;
        private static PassiveManager _passiveManager;
        
        [SerializeField, Header("Weapons")]
        private WeaponProfileScriptableObject[] weaponProfiles;
        
        [SerializeField, Header("Passives")]
        private PassiveItemProfileScriptableObject[] passiveProfiles;
        
        //Unity Functions
        //============================================================================================================//

        private void Awake()
        {
            var playerTransform = FindObjectOfType<PlayerHealth>().transform;
            _weaponManager = new WeaponManager(playerTransform, weaponProfiles);
            
            _passiveManager = new PassiveManager(passiveProfiles);
            
            //Setup Attack Base
            //------------------------------------------------//
            WeaponBase_v2.CoroutineController = this;
        }

        private void OnEnable()
        {
            UIManager.OnItemSelected += OnItemSelected;
            _weaponManager.OnEnable();
            _passiveManager.OnEnable();
        }

        private void Update()
        {
            _weaponManager.Update();
        }

        private void OnDisable()
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
                    return level >= 1 ? _weaponManager.GetLevelUpText(weapon.type, level) : item.description;
                default:
                    return item.description;
            }
        }
        
        //Callbacks
        //============================================================================================================//

        private static void OnItemSelected(ItemBaseScriptableObject item)
        {
            switch (item)
            {
                case WeaponProfileScriptableObject weapon:
                    _weaponManager.AddNewAttack(weapon.type);
                    break;
                case PassiveItemProfileScriptableObject passive:
                    _passiveManager.AddNewPassive(passive.type);
                    break;
            }
        }
        
        //============================================================================================================//
    }
}