using Assets.Scripts.GameCore.Resources;

namespace Assets.Scripts.GameCore.Interfaces
{
    public interface IResourceController
    {
        void AddResource(ResourceTypes type, int amount);
        void RemoveResource(ResourceTypes type, int amount);
        int GetResourceAmount(ResourceTypes type);
        bool HasEnoughResource(ResourceTypes type, int amount);
    }
}
