using Survivors.ScriptableObjets.Animation;
using UnityEngine;

namespace Survivors.ScriptableObjets.Enemies
{
    public enum COLLIDER_TYPE
    {
        NONE,
        BOX,
        CIRCLE
    }
    
    [CreateAssetMenu(fileName = "Enemy Profile", menuName = "ScriptableObjects/Enemy Profile")]
    public class EnemyProfileScriptableObject : ScriptableObject
    {
        [Header("Basics")]
        public string name;
        public Sprite defaultSprite;
        public AnimationProfileScriptableObject animationProfile;
        public STATE defaultState;
        public float shadowOffset;

        [Header("Drops")]
        public int xpDrop;

        [Header("Stats")]
        public float baseSpeed;
        public float baseHealth;
        public float baseDamage;
        
        [Header("Collider")]
        public COLLIDER_TYPE colliderType;
        public Vector2 colliderOffset;
        [Min(0f)]
        public float circleColliderRadius;
        [Min(0f)]
        public Vector2 boxColliderSize;
    }
}