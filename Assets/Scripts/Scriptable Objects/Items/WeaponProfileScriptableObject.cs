using Survivors.ScriptableObjets.Items;
using Survivors.Weapons.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Survivors.ScriptableObjets.Attacks.Items
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

    }
}