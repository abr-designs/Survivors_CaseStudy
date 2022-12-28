using System;
using System.Collections.Generic;
using Survivors.Passives;
using Survivors.Passives.Enums;
using Survivors.ScriptableObjets.Items;
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
        
        //============================================================================================================//
        
        [SerializeField]
        private PassiveItemProfileScriptableObject[] passiveProfiles;
        private Dictionary<PASSIVE_TYPE, int> _passiveItemIndicies;

        private HashSet<PASSIVE_TYPE> _activePassiveTypes;
        private List<PassiveBase> _activePassives;
        
        //============================================================================================================//

        private void OnEnable()
        {
            //Setup Attack Profile Library
            //------------------------------------------------//
            _passiveItemIndicies = new Dictionary<PASSIVE_TYPE, int>(passiveProfiles.Length);

            for (var i = 0; i < passiveProfiles.Length; i++)
            {
                var attackProfile = passiveProfiles[i];
                _passiveItemIndicies.Add(attackProfile.type, i);
            }
        }
        
        //============================================================================================================//

        public void AddPassive(PASSIVE_TYPE passiveType)
        {
            if (_activePassives == null)
            {
                _activePassives = new List<PassiveBase>();
                _activePassiveTypes = new HashSet<PASSIVE_TYPE>();
            }

            //------------------------------------------------//
            
            if (_activePassiveTypes.Contains(passiveType))
            {
                var index = _activePassives
                    .FindIndex(x => x.type == passiveType);

                _activePassives[index].count++;
                
                UpdateValue(_activePassives[index]);
                return;
            }
            
            //------------------------------------------------//

            var profileIndex = _passiveItemIndicies[passiveType];
            var passiveProfile = passiveProfiles[profileIndex];

            var newPassive = new PassiveBase(passiveProfile);
            UpdateValue(newPassive);

            _activePassives.Add(newPassive);
            _activePassiveTypes.Add(passiveType);
        }

        private static void UpdateValue(in PassiveBase passiveBase)
        {
            switch (passiveBase.type)
            {
                case PASSIVE_TYPE.DAMAGE:
                    Damage = 1f + (passiveBase.count * passiveBase.value);
                    break;
                case PASSIVE_TYPE.DAMAGE_REDUCE:
                    break;
                case PASSIVE_TYPE.MAX_HEALTH:
                    MaxHealth = 1f + (passiveBase.count * passiveBase.value);
                    OnMaxHealthChanged?.Invoke(MaxHealth);
                    break;
                case PASSIVE_TYPE.COOLDOWN:
                    Cooldown = 1f - (passiveBase.count * passiveBase.value);
                    break;
                case PASSIVE_TYPE.ATTACK_AREA:
                    AttackArea = 1f + (passiveBase.count * passiveBase.value);
                    OnScaleChanged?.Invoke(AttackArea);
                    break;
                case PASSIVE_TYPE.PROJECTILE_SPEED:
                    ProjectileSpeed = 1f + (passiveBase.count * passiveBase.value);
                    break;
                case PASSIVE_TYPE.DURATION:
                    break;
                case PASSIVE_TYPE.PROJECTILE_COUNT:
                    ProjectileAdd = passiveBase.count;
                    break;
                case PASSIVE_TYPE.MOVE_SPEED:
                    MoveSpeed = 1f + (passiveBase.count * passiveBase.value);
                    break;
                case PASSIVE_TYPE.PICKUP_RANGE:
                    break;
            }
        }
        
        //============================================================================================================//
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
        
        //============================================================================================================//

#if UNITY_EDITOR

        [SerializeField, Header("Debugging")]
        private PASSIVE_TYPE passiveToAdd;

        [ContextMenu("Add Test Attack")]
        private void AddTestAttack()
        {
            AddPassive(passiveToAdd);
        }
        
#endif
    }
}