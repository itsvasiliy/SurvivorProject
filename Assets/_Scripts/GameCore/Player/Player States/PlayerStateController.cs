using UnityEngine;

public class PlayerStateController : MonoBehaviour, IPlayerStateController
{
    [SerializeField] PlayerStates currentState;

    public PlayerStates GetState() => currentState;
    public void SetState(PlayerStates state) => currentState = state;

}
