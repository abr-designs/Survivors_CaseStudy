using Survivors.Base;
using Survivors.Managers;
using UnityEngine;

namespace Survivors.Player
{
    public class PlayerMovementController : MovementControllerBase
    {
        public override float Speed => speed * PassiveManager.MoveSpeed;

        public PlayerMovementController(in Transform transform) : base(in transform)
        {
            InputDelegator.OnMovementChanged += OnMovementChanged;
        }

        ~PlayerMovementController()
        {
            InputDelegator.OnMovementChanged -= OnMovementChanged;
        }
    }
}