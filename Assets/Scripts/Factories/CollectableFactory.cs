using System.Collections.Generic;
using Survivors.Base;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Factories
{
    public class CollectableFactory : FactoryBase<CollectableBase>
    {
        private readonly Dictionary<string, CollectableProfileScriptableObject> _itemProfiles;
        
        public CollectableFactory(CollectableBase prefab, IEnumerable<CollectableProfileScriptableObject> itemProfiles) : base(prefab)
        {
            _itemProfiles = new Dictionary<string, CollectableProfileScriptableObject>();
            foreach (var itemProfile in itemProfiles)
            {
                _itemProfiles.Add(itemProfile.name, itemProfile);
            }
        }

        //============================================================================================================//

        public void CreateXpCollectable(in int xpAmount, in Vector2 worldPos, in Transform parent = null)
        {
            if(xpAmount <= 2)
                CreateCollectable("SmXp", worldPos, parent, xpAmount);
            else if(xpAmount > 2 && xpAmount < 9)
                CreateCollectable("MdXp", worldPos, parent, xpAmount);
            else 
                CreateCollectable("LgXp", worldPos, parent, xpAmount);
        }
        
        //FIXME I should be using a GUID or something
        public void CreateCollectable(in string name, in Vector2 worldPosition, in Transform parent = null, in int value = 0)
        {
            if (_itemProfiles.TryGetValue(name, out var itemProfile) == false)
                throw new KeyNotFoundException($"No enemy with name {name}");

            var itemBase = Create(worldPosition, parent);
            
            itemBase.name = $"{name}_Instance";
            itemBase.Init(itemProfile, value);
        }
    }
}