namespace Survivors.Base.Interfaces
{
    public interface IMovementController
    {
        bool IsMoving { get; }
        int MoveDirection { get; }
        float Speed { get; }

        void SetSpeed(in float speed);
    }
}