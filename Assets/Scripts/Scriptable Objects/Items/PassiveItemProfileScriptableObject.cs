using Survivors.Passives.Enums;
using UnityEngine;

namespace Survivors.ScriptableObjets.Items
{
    [CreateAssetMenu(fileName = "Passive Profile", menuName = "ScriptableObjects/Passive Profile")]
    public class PassiveItemProfileScriptableObject : ItemBaseScriptableObject
    {
        public PASSIVE_TYPE type;
        [Min(0f)]
        public float value;
    }
}