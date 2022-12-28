using Survivors.Passives.Enums;
using Survivors.ScriptableObjets.Items;

namespace Survivors.Passives
{
    public class PassiveBase
    {
        public int count;
        public PASSIVE_TYPE type;
        public float value;
        
        public PassiveBase(in PassiveItemProfileScriptableObject passiveItemProfile)
        {
            count = 1;
            type = passiveItemProfile.type;
            value = passiveItemProfile.value;
        }
    }
}