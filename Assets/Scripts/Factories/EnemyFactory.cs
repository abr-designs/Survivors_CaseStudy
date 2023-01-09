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
        
        private readonly Dictionary<string, EnemyProfileScriptableObject> _enemyProfiles;

        //Constructor
        //============================================================================================================//
        public EnemyFactory(
            EnemyStateController enemyStateControllerPrefab,
            IEnumerable<EnemyProfileScriptableObject> enemyProfiles) : base(enemyStateControllerPrefab)
        {
            _enemyProfiles = new Dictionary<string, EnemyProfileScriptableObject>();
            foreach (var enemyProfile in enemyProfiles)
            {
                _enemyProfiles.Add(enemyProfile.name, enemyProfile);
            }

            _playerTransform = Object.FindObjectOfType<PlayerHealth>().transform;
        }

        //============================================================================================================//
        //FIXME I should be using a GUID or something
        public void CreateEnemy(in string name, in Vector2 worldPosition, in float difficultyMultiplier = 1f, in Transform parent = null)
        {
            if (_enemyProfiles.TryGetValue(name, out var enemyProfile) == false)
                throw new KeyNotFoundException($"No enemy with name {name}");

            EnemyStateController newEnemyStateController = Create(worldPosition, parent);

            newEnemyStateController.name = $"{name}_Instance";
            newEnemyStateController.SetupEnemy(_playerTransform, enemyProfile, difficultyMultiplier);
        }
    }
}