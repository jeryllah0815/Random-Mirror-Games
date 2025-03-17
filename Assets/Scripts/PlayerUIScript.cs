using UnityEngine;
using TMPro;
using Mirror;


public class PlayerUIScript : NetworkBehaviour
{
    public Transform chatContentBox;
    public GameObject messagePrefab;

    //Server broadcast message to all clients

    void Start()
    {
        NetworkEventManager.Instance.OnPlayerJoined.AddListener((player) => RpcBroadcastServerMessage($"{player.playerName} joined"));
    }

    [ClientRpc]
    void RpcBroadcastServerMessage(string text)
    {
        var message = Instantiate(messagePrefab, chatContentBox);
        message.GetComponent<TMP_Text>().text = $"Server: {text}";
    }
}
