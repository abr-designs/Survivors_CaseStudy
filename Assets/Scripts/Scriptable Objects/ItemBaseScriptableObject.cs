using UnityEngine;

namespace Scriptable_Objects
{
    public abstract class ItemBaseScriptableObject : ScriptableObject
    {
        public string name;
        public Sprite sprite;

        public string description;
    }
}