using System.Collections;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyHeartbeater : MonoBehaviour
{
    private float heartbeatTimer = 15f;

    private void Start() => DontDestroyOnLoad(this.gameObject);

    public void StartLobbyHeartbeat(string lobbyId) => StartCoroutine(LobbyHeartbeat(lobbyId));

    private IEnumerator LobbyHeartbeat(string lobbyId)
    {
        while (true)
        {
            yield return new WaitForSeconds(heartbeatTimer);
            Debug.Log("Sending ping to lobby");
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
        }
    }
}
