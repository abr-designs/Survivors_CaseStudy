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
        
        private List<(EnemyHealth health, Transform transform)> _enemies;
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
        private const float ENEMY_RADIUS = 0.1f;
        private List<EnemyHealth> _outList;

        #region Enemies In Range

        public static IReadOnlyList<EnemyHealth> GetEnemiesInRange(in Vector2 position, in float radius, in HashSet<EnemyHealth> toIgnore = null)
        {
            return _instance.TryGetEnemiesInRange(position, radius, toIgnore);
        }
        
        private IReadOnlyList<EnemyHealth> TryGetEnemiesInRange(in Vector2 position, in float radius, in HashSet<EnemyHealth> toIgnore)
        {
            if (_enemies == null || _enemies.Count == 0)
                return default;
            
            if (_outList == null)
                _outList = new List<EnemyHealth>();
            else _outList.Clear();

            foreach (var (enemyHealth, enemyTransform) in _enemies)
            {
                if(toIgnore != null && toIgnore.Contains(enemyHealth))
                    continue;
                if(SMath.CirclesIntersect(radius, ENEMY_RADIUS, position, enemyTransform.position) == false)
                    continue;
                
                _outList.Add(enemyHealth);
            }

            return _outList;
        }

        #endregion //Enemies In Range

        //============================================================================================================//
        
        #region Enemies In Bounds

        public static IReadOnlyList<EnemyHealth> GetEnemiesInBounds(in Bounds bounds)
        {
            return _instance.TryGetEnemiesInBounds(bounds);
        }
        
        private IReadOnlyList<EnemyHealth> TryGetEnemiesInBounds(in Bounds bounds)
        {
            if (_enemies == null || _enemies.Count == 0)
                return default;
            
            if (_outList == null)
                _outList = new List<EnemyHealth>();
            else _outList.Clear();

            foreach (var (enemyHealth, enemyTransform) in _enemies)
            {
                if(SMath.CircleOverlapsRect(enemyTransform.position, ENEMY_RADIUS, bounds) == false)
                    continue;
                
                _outList.Add(enemyHealth);
            }

            return _outList;
        }

        #endregion //Enemies In Bounds

        //============================================================================================================//

        #region Closest Enemy

        public static EnemyHealth GetClosestEnemy(in Vector2 worldPosition)
        {
            return _instance.TryGetClosestEnemy(worldPosition);
        }
        private EnemyHealth TryGetClosestEnemy(in Vector2 worldPosition)
        {
            if (_enemies == null || _enemies.Count < 0)
                return default;
            
            var closestIndex = -1;
            var shortestDistance = 9999f;
            for (int i = 0; i < _enemies.Count; i++)
            {
                var position = (Vector2)_enemies[i].transform.position;

                var mag = (worldPosition - position).sqrMagnitude;
                
                if(mag >= shortestDistance)
                    continue;

                shortestDistance = mag;
                closestIndex = i;
            }

            if (closestIndex < 0)
                return default;

            return _enemies[closestIndex].health;
        }

        #endregion //Closest Enemy

        //Unity Editor Functions
        //============================================================================================================//
        

#if UNITY_EDITOR
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
#endif
    }
}