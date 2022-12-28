using System;
using System.Collections.Generic;
using Survivors.Base;
using UnityEngine;

using Survivors.Enemies;
using Survivors.ScriptableObjets;
using Survivors.ScriptableObjets.Enemies;

namespace Survivors.Factories
{
    public class FactoryManager : MonoBehaviour
    {
        //============================================================================================================//
        [SerializeField, Header("Enemies")] 
        private EnemyStateController enemyStateControllerCirclePrefab;
        [SerializeField] 
        private EnemyStateController enemyStateControllerBoxPrefab;
        [SerializeField]
        private List<EnemyProfileScriptableObject> enemyProfiles;
        
        [SerializeField, Header("Items")] 
        private ItemBase itemPrefab;
        [SerializeField]
        private List<CollectableProfileScriptableObject> itemProfiles;
        
        [SerializeField, Header("Projectiles")] 
        private SpriteRenderer projectilePrefab;

        //------------------------------------------------//

        private static FactoryManager _instance;
        private Dictionary<Type, IFactory> _factories;

        //============================================================================================================//

        private void Awake()
        {
            _instance = this;
        }

        //============================================================================================================//
        
        public static T GetFactory<T>() where T : IFactory
        {
            return _instance.TryGetFactory<T>();
        }
        
        private T TryGetFactory<T>() where T : IFactory
        {
            if (_factories == null)
                _factories = new Dictionary<Type, IFactory>();
            
            var type = typeof(T);

            if (_factories.TryGetValue(type, out var factory))
                return (T)factory;
            
            //------------------------------------------------//
            
            IFactory newFactory;
            if (type == typeof(EnemyFactory))
            {
                newFactory = new EnemyFactory(enemyStateControllerCirclePrefab, enemyStateControllerBoxPrefab, enemyProfiles);
            }
            else if (type == typeof(ItemFactory))
            {
                newFactory = new ItemFactory(itemPrefab, itemProfiles);
            }
            else if (type == typeof(ProjectileFactory))
            {
                newFactory = new ProjectileFactory(projectilePrefab);
            }
            else
                throw new NotImplementedException($"No Factory support for {type.Name}");

            //------------------------------------------------//
            
            _factories.Add(type, newFactory);

            return (T)newFactory;
        }
        //============================================================================================================//
    }
}