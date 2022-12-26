using UnityEngine;

namespace Survivors.ScriptableObjets
{
    public enum PICKUP
    {
        NONE,
        EXP,
        HEAL
    }
    
    [CreateAssetMenu(fileName = "Item Profile", menuName = "ScriptableObjects/Item Profile")]
    public class ItemProfileScriptableObject : ScriptableObject
    {
        public string name;
        public Sprite sprite;

        public PICKUP type;
        public int value;
    }
}