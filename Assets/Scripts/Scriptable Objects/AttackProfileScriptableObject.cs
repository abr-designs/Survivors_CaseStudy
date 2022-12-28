﻿using Survivors.Attacks.Enums;
using UnityEngine;

namespace Survivors.ScriptableObjets.Attacks
{
    [CreateAssetMenu(fileName = "Attack Profile", menuName = "ScriptableObjects/Attack Profile")]
    public class AttackProfileScriptableObject : ScriptableObject
    {
        public Sprite sprite;
        public Color spriteColor = Color.white;
        public ATTACK_TYPE type;
        
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