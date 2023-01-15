using System;
using System.Collections.Generic;
using Survivors.Base;
using Survivors.Base.Interfaces;
using Survivors.Base.Managers;
using Survivors.Base.Managers.Interfaces;
using Survivors.Enemies;
using Survivors.Factories;
using Survivors.Managers.MonoBehaviours;
using Survivors.Player;
using Survivors.ScriptableObjets.Enemies;
using Survivors.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Survivors.Managers
{
    public class EnemyManager : ManagerBase, IEnable, IUpdate
    {
        private const float ENEMY_RADIUS = 0.1f;

        private static EnemyManager _instance;

        private Transform _playerTransform;
        
        //private List<(EnemyHealth health, Transform transform)> _enemies;

        private int _enemyCount;
        private List<Transform> _enemyTransforms;
        private List<EnemyStateController> _enemyStateControllers;
        private List<EnemyHealth> _enemyHealths;
        private List<EnemyMovementController> _enemyMovementControllers;
        private List<IAnimationController> _enemyAnimationControllers;

        //============================================================================================================//
        
        public EnemyManager()
        {
            _enemyTransforms = new List<Transform>();
            _enemyStateControllers = new List<EnemyStateController>();
            _enemyHealths = new List<EnemyHealth>();
            _enemyMovementControllers = new List<EnemyMovementController>();
            _enemyAnimationControllers = new List<IAnimationController>();
            
            PlayerManager.OnPlayerCreated += GameManagerOnOnPlayerCreated;
        }

        //IUpdate Implementation
        //============================================================================================================//
        
        public void Update()
        {
            var count = _enemyCount - 1;
            for (int i = count; i >= 0; i--)
            {
                if (_enemyHealths[i].CurrentHealth <= 0)
                {
                    DestroyEnemyAt(i);
                    continue;
                }
                
                _enemyStateControllers[i].Update();
                _enemyMovementControllers[i].Update();
                _enemyAnimationControllers[i].Update();
            }
        }
        
        //IEnable Implementation
        //============================================================================================================//

        public void OnEnable() => _instance = this;

        public void OnDisable() { }

        //============================================================================================================//

        public static void RequestNewEnemy(in EnemyProfileScriptableObject profile, in Vector3 position)
        {
            _instance.CreateNewEnemy(profile, position);
        }
        private void CreateNewEnemy(in EnemyProfileScriptableObject profile, in Vector3 position)
        {
            var (spriteRenderer, collider2D) = FactoryManager
                .GetFactory<EnemyFactory>()
                .CreateEnemy(profile.name, position);
            
            var index = _enemyCount++;
            var transform = spriteRenderer.transform;
            var health = new EnemyHealth(spriteRenderer);
            var movement = new EnemyMovementController(transform);
            var animation = new AnimationControllerBase(spriteRenderer);
            
            _enemyTransforms.Insert(index, transform);
            _enemyHealths.Insert(index, health);
            _enemyMovementControllers.Insert(index, movement);
            _enemyAnimationControllers.Insert(index, animation);
            _enemyStateControllers.Insert(index, new EnemyStateController(
                _playerTransform, 
                profile,
                health,
                collider2D,
                movement,
                animation, 
                spriteRenderer));
        }

        private void DestroyEnemyAt(in int index)
        {
            var gameObject = _enemyTransforms[index].gameObject;
            
            _enemyTransforms.RemoveAt(index);
            _enemyHealths.RemoveAt(index);
            _enemyMovementControllers.RemoveAt(index);
            _enemyAnimationControllers.RemoveAt(index);
            _enemyStateControllers.RemoveAt(index);

            _enemyCount--;
            
            Object.Destroy(gameObject);
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
            if (_enemyHealths == null || _enemyHealths.Count == 0)
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
            if (_enemyHealths == null || _enemyHealths.Count < 0)
                return default;
            
            var closestIndex = -1;
            var shortestDistance = 9999f;
            for (int i = 0; i < _enemyHealths.Count; i++)
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

        //Callbacks
        //============================================================================================================//
        private void GameManagerOnOnPlayerCreated(Transform playerTransform, PlayerHealth _1, SpriteRenderer _)
        {
            _playerTransform = playerTransform;
        }

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