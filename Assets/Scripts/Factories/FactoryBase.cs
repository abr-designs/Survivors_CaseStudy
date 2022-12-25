using UnityEngine;

namespace Survivors.Factories
{
    public interface IFactory
    {
 
    }
    
    public abstract class FactoryBase<T> : IFactory where T: Component
    {
        protected readonly T StateControllerPrefab;
        
        public FactoryBase(T stateControllerPrefab)
        {
            StateControllerPrefab = stateControllerPrefab;
        }

        protected T Create(Vector2 worldPosition, Transform parent)
        {
            var newInstance = Object.Instantiate(StateControllerPrefab, parent, false);
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