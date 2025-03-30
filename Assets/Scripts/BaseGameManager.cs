using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BaseGameManager : NetworkBehaviour
{
    [Header("Base Settings")]
    private LobbyConfig _lobbyConfig;

    //float timerCountdown = 5f;
    float currentCountdown = 5f;
    public override void OnStartServer()
    {
        base.OnStartServer();
        _lobbyConfig = NetworkManager.singleton.GetComponent<CustomNetworkManager>().lobbyConfig;
        Debug.Log("Server: Game Manager initialized on the server");
        Debug.Log($"Server: {_lobbyConfig.name} loaded");
    }

    void Update()
    {
        UpdateCountDown();
    }


    [Server]
    void UpdateCountDown()
    {
        currentCountdown -= Time.deltaTime;

        // Update the text display
        //countdownText.text = Mathf.Ceil(currentTime).ToString();

        if (currentCountdown <= 0)
        {
            currentCountdown = 0;
            //countdownText.text = "GO!";
            // You can add any action you want to happen when timer reaches 0 here
            //Debug.Log("Server: Countdown Done");
        }
    }
}
