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
              currentState != PlayerStates.Mining)
            currentState = PlayerStates.Idle;
    }

}
