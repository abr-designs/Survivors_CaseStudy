using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using Survivors.Enemies;
using Survivors.Utilities;
using UnityEngine;

namespace Survivors.Managers
{
    public class EnemyManager : ManagerBase, IEnable, IUpdate
    {
        private const float ENEMY_RADIUS = 0.1f;

        private static EnemyManager _instance;
        
        private List<EnemyHealth> _enemyHealths;
        private List<Transform> _enemyTransforms;

        //============================================================================================================//
        
        public EnemyManager()
        {
            
        }

        //IEnable Implementation
        //============================================================================================================//
        
        public void OnEnable()
        {
            _instance = this;
            
            EnemyHealth.OnNewEnemy += OnNewEnemy;
            EnemyHealth.OnEnemyRemoved += OnEnemyRemoved;
        }

        public void OnDisable()
        {
            EnemyHealth.OnNewEnemy -= OnNewEnemy;
            EnemyHealth.OnEnemyRemoved -= OnEnemyRemoved;
        }

        //IUpdate Implementation
        //============================================================================================================//

        public void Update()
        {
            if (_enemyTransforms == null || _enemyTransforms.Count == 0)
                return;
            
            CollisionSolver.SolveCollisions(ENEMY_RADIUS, _enemyTransforms);
        }

        //============================================================================================================//
        private void OnNewEnemy(EnemyHealth enemyHealth)
        {
            if (_enemyHealths == null)
            {
                _enemyHealths = new List<EnemyHealth>();
                _enemyTransforms = new List<Transform>();
            }
            
            _enemyHealths.Add(enemyHealth);
            _enemyTransforms.Add(enemyHealth.transform);
        }
        private void OnEnemyRemoved(EnemyHealth enemyHealth)
        {
            if (_enemyHealths == null)
                return;

            var index = _enemyHealths.FindIndex(x => x == enemyHealth);

            if (index < 0)
                throw new Exception();
            
            _enemyHealths.RemoveAt(index);
            _enemyTransforms.RemoveAt(index);
        }
        
        //============================================================================================================//
        private List<EnemyHealth> _outList;

        #region Enemies In Range

        public static IReadOnlyList<EnemyHealth> GetEnemiesInRange(in Vector2 position, in float radius, in HashSet<EnemyHealth> toIgnore = null)
        {
            return _instance.TryGetEnemiesInRange(position, radius, toIgnore);
        }
        
        private IReadOnlyList<EnemyHealth> TryGetEnemiesInRange(in Vector2 position, in float radius, in HashSet<EnemyHealth> toIgnore)
        {
            if (_enemyTransforms == null || _enemyTransforms.Count == 0)
                return default;
            
            if (_outList == null)
                _outList = new List<EnemyHealth>();
            else _outList.Clear();

            for (var i = 0; i < _enemyHealths.Count; i++)
            {
                var enemyHealth = _enemyHealths[i];
                if (toIgnore != null && toIgnore.Contains(enemyHealth))
                    continue;
                if (SMath.CirclesIntersect(radius, ENEMY_RADIUS, position, _enemyTransforms[i].position) == false)
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
            if (_enemyHealths == null || _enemyHealths.Count == 0)
                return default;
            
            if (_outList == null)
                _outList = new List<EnemyHealth>();
            else _outList.Clear();

            for (var i = 0; i < _enemyHealths.Count; i++)
            {
                var enemyHealth = _enemyHealths[i];
                if (SMath.CircleOverlapsRect(_enemyTransforms[i].position, ENEMY_RADIUS, bounds) == false)
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
            if (_enemyTransforms == null || _enemyTransforms.Count < 0)
                return default;
            
            var closestIndex = -1;
            var shortestDistance = 9999f;
            for (int i = 0; i < _enemyTransforms.Count; i++)
            {
                var position = (Vector2)_enemyTransforms[i].position;

                var mag = (worldPosition - position).sqrMagnitude;
                
                if(mag >= shortestDistance)
                    continue;

                shortestDistance = mag;
                closestIndex = i;
            }

            if (closestIndex < 0)
                return default;

            return _enemyHealths[closestIndex];
        }

        #endregion //Closest Enemy

        //Unity Editor Functions
        //============================================================================================================//
        

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false)
                return;

            if (_enemyTransforms == null)
                return;
            
            Gizmos.color = Color.red;
            for (int i = 0; i < _enemyTransforms.Count; i++)
            {
                Gizmos.DrawWireSphere(_enemyTransforms[i].position, 0.1f);

            }
        }
#endif

    }
}