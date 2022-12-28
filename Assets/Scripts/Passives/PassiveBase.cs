using Survivors.Passives.Enums;
using Survivors.ScriptableObjets.Items;

namespace Survivors.Passives
{
    public class PassiveBase
    {
        public int Level;
        public PASSIVE_TYPE Type;
        public float Value;
        
        public PassiveBase(in PassiveItemProfileScriptableObject passiveItemProfile)
        {
            Level = 1;
            Type = passiveItemProfile.type;
            Value = passiveItemProfile.value;
        }
    }
}