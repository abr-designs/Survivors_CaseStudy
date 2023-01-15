using Survivors.Base;
using UnityEngine;

namespace Survivors.Enemies
{
    public class EnemyMovementController : MovementControllerBase
    {
        public EnemyMovementController(in Transform transform) : base(in transform)
        {
        }
        public void SetMoveDirection(in Vector2 moveDirection)
        {
            var dir = moveDirection.normalized;

            OnMovementChanged(dir.x, dir.y);
        }
    }
}