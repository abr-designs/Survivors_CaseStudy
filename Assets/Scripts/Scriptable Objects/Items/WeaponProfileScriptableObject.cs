using System;
using System.Collections.Generic;
using Survivors.ScriptableObjets.Items;
using Survivors.Weapons.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Survivors.ScriptableObjets.Weapons.Items
{
    [CreateAssetMenu(fileName = "Weapon Profile", menuName = "ScriptableObjects/Weapon Profile")]
    public class WeaponProfileScriptableObject : ItemBaseScriptableObject
    {
        [Space(10f)]
        public Sprite projectileSprite;
        [FormerlySerializedAs("spriteColor")] 
        public Color projectileSpriteColor = Color.white;
        public WEAPON_TYPE type;
        
        [Min(0), Header("Basics")]
        public float range;
        [Min(0)]
        public float damage;
        [Min(0)]
        public float cooldown;
        [Min(1)]
        public int maxHitCount;
        
        [Min(1), Header("Projectiles")]
        public int projectileCount = 1;
        [Min(0)]
        public float projectileInterval;
        [Min(0f)]
        public float projectileRadius;
        
        [Space(10f)]
        public float launchSpeed;
        public float acceleration;
        public float spinSpeed;

        [NonReorderable]
        public WeaponLevel[] levelUpStats = new WeaponLevel[7];

        public string GetLevelUpText(in int nextLevel)
        {
            if (nextLevel <= 1 || nextLevel > 8)
                throw new Exception();

            return levelUpStats[nextLevel - 2].name;
        }
        
        public IEnumerable<WeaponLevelStat> GetLevelUpChanges(in int nextLevel)
        {
            if (nextLevel <= 1 || nextLevel > 8)
                throw new Exception();

            return levelUpStats[nextLevel - 2].levelUpStats;
        }

        //============================================================================================================//
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < 7; i++)
            {
                var weaponLevel = levelUpStats[i];

                weaponLevel.name = $"Level {i + 2} ";
                
                if(weaponLevel.levelUpStats == null || weaponLevel.levelUpStats.Length ==0)
                    continue;

                for (var ii = 0; ii < weaponLevel.levelUpStats.Length; ii++)
                {
                    var levelStat = weaponLevel.levelUpStats[ii];
                    levelStat.name = levelStat.ToString();
                    weaponLevel.levelUpStats[ii] = levelStat;
                }

                weaponLevel.name += string.Join(" ", weaponLevel.levelUpStats);
            }   
        }
#endif
    }

    public enum STAT_TYPE
    {
        NONE,
        DAMAGE,
        PROJECTILE,
        AREA,
        COOLDOWN,
        PASSTHROUGH,
        SPEED
    }

    [Serializable]
    public class WeaponLevel
    {
        public string name;
        [NonReorderable]
        public WeaponLevelStat[] levelUpStats;
    }

    [Serializable]
    public struct WeaponLevelStat
    {
        public string name;
        public STAT_TYPE type;
        public float change;

        public override string ToString()
        {
            switch (type)
            {
                case STAT_TYPE.DAMAGE:
                    return $"Base damage up by {change:#0}.";
                case STAT_TYPE.PROJECTILE:
                    return $"Fires {change:#0} more projectile.";
                case STAT_TYPE.AREA:
                    return $"Base area up by {change:P0}.";
                case STAT_TYPE.COOLDOWN:
                    return $"Cooldown reduced by {change:#0.0} seconds.";
                case STAT_TYPE.PASSTHROUGH:
                    return $"Passes through {change:#0} more enemies.";
                case STAT_TYPE.SPEED:
                    return $"Base speed up by {change:P0}.";
                default:
                    return string.Empty;
                    
            }
        }
    }
}