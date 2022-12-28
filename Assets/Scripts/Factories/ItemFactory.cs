using System.Collections.Generic;
using Survivors.Base;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Factories
{
    public class ItemFactory : FactoryBase<ItemBase>
    {
        private readonly Dictionary<string, CollectableProfileScriptableObject> _itemProfiles;
        
        public ItemFactory(ItemBase prefab, IEnumerable<CollectableProfileScriptableObject> itemProfiles) : base(prefab)
        {
            _itemProfiles = new Dictionary<string, CollectableProfileScriptableObject>();
            foreach (var itemProfile in itemProfiles)
            {
                _itemProfiles.Add(itemProfile.name, itemProfile);
            }
        }
        
        //FIXME I should be using a GUID or something
        public void CreateItem(in string name, in Vector2 worldPosition, in Transform parent = null)
        {
            if (_itemProfiles.TryGetValue(name, out var itemProfile) == false)
                throw new KeyNotFoundException($"No enemy with name {name}");

            var itemBase = Create(worldPosition, parent);
            
            itemBase.name = $"{name}_Instance";
            itemBase.Init(itemProfile);
        }
    }
}