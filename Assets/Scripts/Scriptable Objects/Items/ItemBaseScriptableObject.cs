using UnityEngine;

namespace Survivors.ScriptableObjets.Items
{
    public abstract class ItemBaseScriptableObject : ScriptableObject
    {
        public string name;
        public Sprite sprite;

        public string description;
    }
}