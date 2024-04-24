using Assets.Scripts.GameCore.Interfaces;
using Zenject;

public class ResourceControlInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IResourceController>().To<ResourceController>().AsSingle();
    }

}