namespace Survivors.Player
{
    public class PlayerStateControllerBase : StateControllerBase
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