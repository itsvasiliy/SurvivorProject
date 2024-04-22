public interface IPlayerStateController
{
    public void SetState(PlayerStates state);
    public PlayerStates GetState();
}


public enum PlayerStates
{
    Idle,
    Running,
    Mining,
    BuildViewing,
    Shooting,
}