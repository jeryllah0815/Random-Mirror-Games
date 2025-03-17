using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
public class NetworkEventManager : MonoSingleton<NetworkEventManager>
{
    public UnityEvent<PlayerScript> OnPlayerJoined = new UnityEvent<PlayerScript>();
    public UnityEvent<PlayerScript> OnPlayerLeft = new UnityEvent<PlayerScript>();
    public UnityEvent<int> OnScoreUpdated = new UnityEvent<int>();

    void Start()
    {
        OnPlayerJoined.AddListener((x) => Debug.Log("NetworkEvent: OnPlayerJoined."));
        OnPlayerLeft.AddListener((x) => Debug.Log("NetworkEvent: OnPlayerLeft."));
    }
}
