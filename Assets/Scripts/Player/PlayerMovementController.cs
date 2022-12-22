using Survivors.Base;

namespace Survivors.Player
{
    public class PlayerMovementController : MovementControllerBase
    {
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