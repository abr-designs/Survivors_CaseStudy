namespace Survivors.Managers
{
    public interface IUpdate
    {
        void Update();
    }
    
    public interface ILateUpdate
    {
        void LateUpdate();
    }
    
    public interface IEnable
    {
        void OnEnable();
        void OnDisable();
    }
    public abstract class ManagerBase
    {
    }
    
    
}