using System;
using System.Collections;
using Survivors.Factories;
using Survivors.ScriptableObjets.Enemies;
using Survivors.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Managers
{
    //FIXME Make this no longer a Monobehaviour
    public class WaveSpawnManager : MonoBehaviour
    {
        [Serializable]
        private struct SpawnData
        {
            [Min(0)]
            public int seconds;
            public EnemyProfileScriptableObject[] enemyProfiles;
            [Min(1)]
            public int minSpawn;
            [Min(0f)]
            public float spawnInterval;

        }

        [SerializeField]
        private Camera camera;
        
        [SerializeField]
        [NonReorderable]
        //FIXME Move this into a Stage Profile Scriptable Object
        private SpawnData[] spawnDatas;

        private Rect _cameraRect;
        private int _seconds;
        private float _secondsTimer;

        private int _currentIndex;

        private void Start()
        {
            StartNewWave(0);
        }

        private void Update()
        {
            if (_secondsTimer < 1f)
            {
                _secondsTimer += Time.deltaTime;
            }
            else
            {
                _secondsTimer = 0f;
                _seconds++;

                if (_seconds >= spawnDatas[_currentIndex + 1].seconds)
                {
                    StartNewWave(_currentIndex + 1);
                }
            }
            
            _cameraRect.min = camera.ViewportToWorldPoint(Vector2.zero);
            _cameraRect.max = camera.ViewportToWorldPoint(Vector2.one);
        }

        private Coroutine _spawnCoroutine;
        private void StartNewWave(in int newIndex)
        {
            _currentIndex = newIndex;
            var spawnData = spawnDatas[_currentIndex];

            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }

            _spawnCoroutine = StartCoroutine(SpawnEnemiesCoroutine(spawnData));
        }

        private IEnumerator SpawnEnemiesCoroutine(SpawnData spawnData)
        {
            const float MIN_DIST = 0.2f;
            const float MAX_DIST = 1f;
            
            var delay = spawnData.spawnInterval;
            var waitForSeconds = new WaitForSeconds(delay);
            
            for (int i = 0; i < spawnData.minSpawn; i++)
            {
                var horizontal = Random.value >= 0.5f;
                var vertical = Random.value >= 0.5f;

                var position = new Vector2(
                    horizontal ? _cameraRect.xMin - Random.Range(1, 3f) : _cameraRect.xMax + Random.Range(1, 3f),
                    vertical ? _cameraRect.yMin - Random.Range(1, 3f) : _cameraRect.yMax + Random.Range(1, 3f));

                var enemy = spawnData.enemyProfiles.GetRandomElement();

                FactoryManager
                    .GetFactory<EnemyFactory>()
                    .CreateEnemy(enemy.name, position);

                yield return waitForSeconds;
            }

            while (true)
            {
                var horizontal = Random.value >= 0.5f;
                var vertical = Random.value >= 0.5f;

                var position = new Vector2(
                    horizontal ? _cameraRect.xMin - Random.Range(MIN_DIST, MAX_DIST) : _cameraRect.xMax + Random.Range(MIN_DIST, MAX_DIST),
                    vertical ? _cameraRect.yMin - Random.Range(  MIN_DIST, MAX_DIST) : _cameraRect.yMax + Random.Range(MIN_DIST, MAX_DIST));

                var enemy = spawnData.enemyProfiles.GetRandomElement();

                FactoryManager
                    .GetFactory<EnemyFactory>()
                    .CreateEnemy(enemy.name, position);

                yield return waitForSeconds;
            }
        }
    }
}
