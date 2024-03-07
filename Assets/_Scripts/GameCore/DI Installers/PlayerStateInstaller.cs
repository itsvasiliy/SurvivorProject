using UnityEngine;
using Zenject;

public class PlayerStateInstaller : MonoInstaller
{
    [SerializeField] private GameObject playerStateRealization;

    public override void InstallBindings()
    {
        Container.Bind<IPlayerStateController>().FromInstance(playerStateRealization.GetComponent<PlayerStateController>()).AsSingle();
    }
}