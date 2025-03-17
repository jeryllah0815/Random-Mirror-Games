using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;

public class PlayerUIScript : NetworkBehaviour
{
    public Transform chatContentBox;
    public ScrollRect messageRect;
    public GameObject messagePrefab;
    public string playerName;
    //Server broadcast message to all clients

    void Start()
    {
        NetworkEventManager.Instance.OnPlayerJoined.AddListener((player) => RpcBroadcastServerMessage($"{player.playerName} joined"));
        
    }

    [ClientRpc]
    public void RpcBroadcastServerMessage(string text)
    {
        SendTextMessage($"Server: {text}");
    }

    [ClientRpc]
    public void RpcBroadcastClientMessage(NetworkIdentity sender, string text)
    {
        SendTextMessage($"{sender.GetComponent<PlayerScript>().playerName}: {text}");
    }

    void SendTextMessage(string text)
    {
        var message = Instantiate(messagePrefab, chatContentBox);
        message.GetComponent<TMP_Text>().text = text;

        Canvas.ForceUpdateCanvases(); //might be lazy coz its calling ALL canvases but should be fine for now
        //LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect.transform as RectTransform);
        messageRect.verticalNormalizedPosition = 0;
    }
}
