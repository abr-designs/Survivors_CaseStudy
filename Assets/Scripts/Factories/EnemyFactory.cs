using System;
using System.Collections.Generic;
using Survivors.ScriptableObjets.Enemies;
using UnityEngine;

namespace Survivors.Factories
{
    public class EnemyFactory : FactoryBase<SpriteRenderer>
    {
        private readonly SpriteRenderer _enemyBoxPrefab;
        private readonly Dictionary<string, EnemyProfileScriptableObject> _enemyProfiles;

        //Constructor
        //============================================================================================================//
        public EnemyFactory(
            SpriteRenderer enemyCirclePrefab, 
            SpriteRenderer enemyBoxPrefab,
            IEnumerable<EnemyProfileScriptableObject> enemyProfiles) : base(enemyCirclePrefab)
        {
            _enemyProfiles = new Dictionary<string, EnemyProfileScriptableObject>();
            foreach (var enemyProfile in enemyProfiles)
            {
                _enemyProfiles.Add(enemyProfile.name, enemyProfile);
            }

            _enemyBoxPrefab = enemyBoxPrefab;
        }

        //============================================================================================================//
        //FIXME I should be using a GUID or something
        public (SpriteRenderer, Collider2D) CreateEnemy(in string name, in Vector2 worldPosition, in Transform parent = null)
        {
            if (_enemyProfiles.TryGetValue(name, out var enemyProfile) == false)
                throw new KeyNotFoundException($"No enemy with name {name}");

            SpriteRenderer newEnemySpriteRenderer;
            switch (enemyProfile.colliderType)
            {
                case COLLIDER_TYPE.BOX:
                    newEnemySpriteRenderer = Create(_enemyBoxPrefab, worldPosition, parent);
                    break;
                case COLLIDER_TYPE.CIRCLE:
                    newEnemySpriteRenderer = Create(worldPosition, parent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enemyProfile.colliderType), enemyProfile.colliderType, null);
            }

            newEnemySpriteRenderer.name = $"{name}_Instance";

            //FIXME There has to be a better way of getting the Collider
            return (newEnemySpriteRenderer, newEnemySpriteRenderer.GetComponent<Collider2D>());
        }
    }
}