using UnityEngine;

namespace Survivors.ScriptableObjets
{
    public enum PICKUP
    {
        NONE,
        EXP,
        HEAL
    }
    
    [CreateAssetMenu(fileName = "Collectable Profile", menuName = "ScriptableObjects/Collectable Profile")]
    public class CollectableProfileScriptableObject : ScriptableObject
    {
        public string name;
        public Sprite sprite;

        public PICKUP type;
        public int value;
    }
}