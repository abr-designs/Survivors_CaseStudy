namespace Survivors.Base.Interfaces
{
    public interface IMovementController
    {
        bool IsMoving { get; }
        int MoveDirection { get; }

        void SetActive(bool state);
    }
}