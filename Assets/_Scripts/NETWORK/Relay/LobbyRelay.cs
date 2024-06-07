using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class LobbyRelay : MonoBehaviour
{
    public async Task<string> CreateRelay(int maxConnections)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            return joinCode;

        }
        catch (RelayServiceException error)
        {
            Debug.LogError(error);
            return null;
        }
    }

    public async Task<string> InitRelay(int maxConnections)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            RelayServerDataManagerSingleton.relayServerData = relayServerData;
            RelayServerDataManagerSingleton.relayCode = joinCode;
            RelayServerDataManagerSingleton.isHost = true;

            return joinCode;
        }
        catch (RelayServiceException error)
        {
            Debug.LogError(error);
        }
        return null;
    }
}
