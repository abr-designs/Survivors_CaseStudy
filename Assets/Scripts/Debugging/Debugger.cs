using Survivors.Factories;
using Survivors.ScriptableObjets;
using Survivors.ScriptableObjets.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Debugging
{
    public class Debugger : MonoBehaviour
    {
        [Header("Spawn Debug")] 
        [SerializeField]
        private EnemyProfileScriptableObject[] enemies;
        [SerializeField]
        private ItemProfileScriptableObject[] items;

        [SerializeField] private float spawnRadius;
        [SerializeField, Min(1)] private int spawnAmount = 1;
        [SerializeField] private int spawnedCount;

        [SerializeField] private KeyCode spawnEnemyKey = KeyCode.F1;
        [SerializeField] private KeyCode spawnItemKey = KeyCode.F2;

        //============================================================================================================//
        // Update is called once per frame
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(spawnEnemyKey))
                SpawnEnemies();
            if (UnityEngine.Input.GetKeyDown(spawnItemKey))
                SpawnItems();
        }
        //============================================================================================================//

        private void SpawnEnemies()
        {
            for (var i = 0; i < spawnAmount; i++)
            {
                var position = Random.insideUnitCircle * spawnRadius;
                var enemyProfile = enemies[Random.Range(0, enemies.Length)];
                
                FactoryManager
                    .GetFactory<EnemyFactory>()
                    .CreateEnemy(enemyProfile.name, position);
            }

            spawnedCount += spawnAmount;
        }

        //============================================================================================================//
        private void SpawnItems()
        {
            for (var i = 0; i < spawnAmount; i++)
            {
                var position = Random.insideUnitCircle * spawnRadius;
                var itemProfile = items[Random.Range(0, items.Length)];
                
                FactoryManager
                    .GetFactory<ItemFactory>()
                    .CreateItem(itemProfile.name, position);
            }

            spawnedCount += spawnAmount;
        }
        
        //============================================================================================================//
    }
}
