using Survivors.Base;
using Survivors.Managers;

namespace Survivors.Player
{
    public class PlayerMovementController : MovementControllerBase
    {
        public override float Speed => speed * PassiveManager.MoveSpeed;

        private void OnEnable()
        {
            InputDelegator.OnMovementChanged += OnMovementChanged;
        }
        
        private void OnDisable()
        {
            InputDelegator.OnMovementChanged -= OnMovementChanged;
        }
    }
}