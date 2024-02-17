using UnityEngine;
using Zenject;

public class ResourceUIControlInstaller : MonoInstaller
{
    [SerializeField] ResourcesUIControl resourcesUIControl;

    public override void InstallBindings()
    {
        Container.Bind<ResourcesUIControl>().FromInstance(resourcesUIControl).AsSingle();
    }
}