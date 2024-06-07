using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private GameObject connElements;

    private int maxPlayers = 1;

    private async void Start()
    {
        if (RelayServerDataManagerSingleton.relayCode != null)
        {
            JoinRelayAutomatic();
            return;
        }


        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }



    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            NetworkManager.Singleton.StartHost();

        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public async void JoinRelay()
    {
        try
        {
            Debug.Log("Joining relay with " + inputField.text);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(inputField.text);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async void JoinRelayAutomatic()
    {
        connElements.SetActive(false);
        try
        {
            var asHost = RelayServerDataManagerSingleton.isHost ? " as host" : " as client";
            Debug.Log("Conneting to " + RelayServerDataManagerSingleton.relayCode + asHost);


            if (RelayServerDataManagerSingleton.isHost)
            {
                RelayServerData relayServerData = RelayServerDataManagerSingleton.relayServerData;
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartHost();

            }
            else
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(RelayServerDataManagerSingleton.relayCode);

                RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
            }


        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

}