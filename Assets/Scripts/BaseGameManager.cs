using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BaseGameManager : NetworkBehaviour
{
    [Header("Base Settings")]
    private LobbyConfig _lobbyConfig;

    public override void OnStartServer()
    {
        base.OnStartServer();
        _lobbyConfig = NetworkManager.singleton.GetComponent<CustomNetworkManager>().lobbyConfig;
        Debug.Log("Server: Game Manager initialized on the server");
        Debug.Log($"Server: {_lobbyConfig.name} loaded");
    }
}
