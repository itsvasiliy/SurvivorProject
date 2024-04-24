using UnityEngine;
using Zenject;

public class BuildingViewerInstaller : MonoInstaller
{
    [SerializeField] StructurePlacement structurePlacement;

    public override void InstallBindings()
    {
        Container.Bind<StructurePlacement>().FromInstance(structurePlacement).AsSingle();
    }
}