using UnityEngine;

public class PlayerStateController : MonoBehaviour, IPlayerStateController
{
    [SerializeField] PlayerStates currentState;

    public PlayerStates GetState() => currentState;
    public void SetState(PlayerStates state) => currentState = state;

    public void TryToSetIdleState()
    {
        if (currentState != PlayerStates.Idle &&
             currentState != PlayerStates.Shooting &&
             currentState != PlayerStates.BuildViewing &&
              currentState != PlayerStates.Mining)
            currentState = PlayerStates.Idle;
    }

    public void TryToSetRunningState()
    {
        if (currentState != PlayerStates.BuildViewing)
            currentState = PlayerStates.Running;
    }
}
