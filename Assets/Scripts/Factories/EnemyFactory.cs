using System;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Player;
using Survivors.ScriptableObjets.Enemies;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Survivors.Factories
{
    public class EnemyFactory : FactoryBase<EnemyStateController>
    {
        private readonly Transform _playerTransform;
        
        private readonly EnemyStateController _boxColliderEnemyStateControllerPrefab;
        private readonly Dictionary<string, EnemyProfileScriptableObject> _enemyProfiles;

        //Constructor
        //============================================================================================================//
        public EnemyFactory(
            EnemyStateController circleColliderEnemyStateControllerPrefab, 
            EnemyStateController boxColliderEnemyStateControllerPrefab,
            IEnumerable<EnemyProfileScriptableObject> enemyProfiles) : base(circleColliderEnemyStateControllerPrefab)
        {
            _enemyProfiles = new Dictionary<string, EnemyProfileScriptableObject>();
            foreach (var enemyProfile in enemyProfiles)
            {
                _enemyProfiles.Add(enemyProfile.name, enemyProfile);
            }

            _boxColliderEnemyStateControllerPrefab = boxColliderEnemyStateControllerPrefab;

            _playerTransform = Object.FindObjectOfType<PlayerHealth>().transform;
        }

        //============================================================================================================//
        //FIXME I should be using a GUID or something
        public void CreateEnemy(in string name, in Vector2 worldPosition, in float difficultyMultiplier = 1f, in Transform parent = null)
        {
            if (_enemyProfiles.TryGetValue(name, out var enemyProfile) == false)
                throw new KeyNotFoundException($"No enemy with name {name}");

            EnemyStateController newEnemyStateController;
            switch (enemyProfile.colliderType)
            {
                case COLLIDER_TYPE.BOX:
                    newEnemyStateController = Create(_boxColliderEnemyStateControllerPrefab, worldPosition, parent);
                    break;
                case COLLIDER_TYPE.CIRCLE:
                    newEnemyStateController = Create(worldPosition, parent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enemyProfile.colliderType), enemyProfile.colliderType, null);
            }

            newEnemyStateController.name = $"{name}_Instance";
            newEnemyStateController.SetupEnemy(_playerTransform, enemyProfile, difficultyMultiplier);
        }
    }
}