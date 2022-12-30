using Survivors.Passives.Enums;
using Survivors.ScriptableObjets.Animation;
using Survivors.Weapons.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Survivors.ScriptableObjets
{
    [CreateAssetMenu(fileName = "Player Profile", menuName = "ScriptableObjects/Player Profile")]
    public class PlayerProfileScriptableObject : ScriptableObject
    {
        public string name;
        
        public WEAPON_TYPE[] startingWeapons;
        [FormerlySerializedAs("startingPassive")] public PASSIVE_TYPE[] startingPassives;

        public AnimationProfileScriptableObject animationProfile;
        public float startingHealth;
        public float moveSpeed;
        public float pickupRadius;
        public float shadowOffset;
        

    }
}
