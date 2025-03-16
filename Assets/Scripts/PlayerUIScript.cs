using UnityEngine;
using TMPro;
using Mirror;


public class PlayerUIScript : NetworkBehaviour //IDK ClientUICallbacks
{
    public Transform chatContentBox;
    public GameObject messagePrefab;

    ////Command send message to server
    //[Command]
    //public void CmdSendGlobalMessage(string text)
    //{
    //    if (string.IsNullOrEmpty(text))
    //        return;

    //    RpcBroadcastGlobalMessage(text);
    //}

    //Server broadcast message to all clients
    [ClientRpc]
    void RpcBroadcastGlobalMessage(string text)
    {
        var message = Instantiate(messagePrefab, chatContentBox);
        message.GetComponent<TMP_Text>().text = text;
    }

    //override onplayerconnect 
}
