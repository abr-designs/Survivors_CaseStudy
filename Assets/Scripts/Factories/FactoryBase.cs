using UnityEngine;

namespace Survivors.Factories
{
    public interface IFactory
    {
 
    }
    
    public abstract class FactoryBase<T> : IFactory where T: Component
    {
        protected readonly T Prefab;
        
        public FactoryBase(T prefab)
        {
            Prefab = prefab;
        }

        protected T Create(Vector2 worldPosition, Transform parent)
        {
            var newInstance = Object.Instantiate(Prefab, parent, false);
            newInstance.transform.position = worldPosition;

            return newInstance;
        }
        
        protected T Create(T customPrefab, Vector2 worldPosition, Transform parent)
        {
            var newInstance = Object.Instantiate(customPrefab, parent, false);
            newInstance.transform.position = worldPosition;

            return newInstance;
        }
    }
}