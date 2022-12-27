using System;
using System.Collections.Generic;
using Survivors.Enemies;
using Survivors.Utilities;
using UnityEngine;

namespace Survivors.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class EnemyManager : MonoBehaviour
    {
        private static EnemyManager _instance;
        
        private List<(EnemyHealth, Transform)> _enemies;
        //TODO Store active enemies
        //TODO Find all enemies in a radius

        private void Awake()
        {
            _instance = this;
        }

        private void OnEnable()
        {
            EnemyHealth.OnNewEnemy += OnNewEnemy;
            EnemyHealth.OnEnemyRemoved += OnEnemyRemoved;
        }

        private void OnDisable()
        {
            EnemyHealth.OnNewEnemy -= OnNewEnemy;
            EnemyHealth.OnEnemyRemoved -= OnEnemyRemoved;
        }

        //============================================================================================================//
        private void OnNewEnemy(EnemyHealth enemyHealth)
        {
            if (_enemies == null)
                _enemies = new List<(EnemyHealth, Transform)>();
            
            _enemies.Add((enemyHealth, enemyHealth.transform));
        }
        private void OnEnemyRemoved(EnemyHealth enemyHealth)
        {
            if (_enemies == null)
                return;

            var index = _enemies.FindIndex(x => x.Item1 == enemyHealth);

            if (index < 0)
                throw new Exception();
            
            _enemies.RemoveAt(index);
        }
        
        //============================================================================================================//
        private List<EnemyHealth> _outList;
        public static IReadOnlyList<EnemyHealth> GetEnemiesInRange(in Vector2 position, in float radius)
        {
            return _instance.TryGetEnemiesInRange(position, radius);
        }
        
        private IReadOnlyList<EnemyHealth> TryGetEnemiesInRange(in Vector2 position, in float radius)
        {
            if (_enemies == null || _enemies.Count == 0)
                return default;
            
            if (_outList == null)
                _outList = new List<EnemyHealth>();
            else _outList.Clear();

            foreach (var (enemyHealth, enemyTransform) in _enemies)
            {
                if(SMath.CirclesIntersect(radius, 0.1f, position, enemyTransform.position) == false)
                    continue;
                
                _outList.Add(enemyHealth);
            }

            return _outList;
        }
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false)
                return;

            if (_enemies == null)
                return;
            
            Gizmos.color = Color.red;
            for (int i = 0; i < _enemies.Count; i++)
            {
                Gizmos.DrawWireSphere(_enemies[i].Item2.position, 0.1f);

            }
        }
    }
}