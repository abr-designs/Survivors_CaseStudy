using Survivors.ScriptableObjets.Animation;
using UnityEngine;

namespace Survivors.ScriptableObjets.Enemies
{

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
    }
}