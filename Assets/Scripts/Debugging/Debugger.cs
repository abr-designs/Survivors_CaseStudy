using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Debugging
{
    public class Debugger : MonoBehaviour
    {
        [Header("Spawn Debug")] [SerializeField]
        private GameObject[] spawnPrefabs;

        [SerializeField] private float spawnRadius;
        [SerializeField, Min(1)] private int spawnAmount = 1;
        [SerializeField] private int spawnedCount;

        [SerializeField] private KeyCode spawnKey = KeyCode.F1;

        //============================================================================================================//
        // Update is called once per frame
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(spawnKey))
                SpawnPrefabs();
        }
        //============================================================================================================//

        private void SpawnPrefabs()
        {
            for (var i = 0; i < spawnAmount; i++)
            {
                var position = Random.insideUnitCircle * spawnRadius;
                var prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
                Instantiate(prefab, position, Quaternion.identity);
            }

            spawnedCount += spawnAmount;
        }
    }
}
