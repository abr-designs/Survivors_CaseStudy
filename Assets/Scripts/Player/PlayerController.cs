namespace Survivors.Player
{
    public class PlayerController : CharacterController
    {
        private void OnEnable()
        {
            InputDelegator.OnMovementChanged += OnMovementChanged;
            InputDelegator.OnKillPressed += () =>
            {
                SetState(STATE.DEATH);
            };
        }
        
        private void OnDisable()
        {
            InputDelegator.OnMovementChanged -= OnMovementChanged;
        }
    }
}