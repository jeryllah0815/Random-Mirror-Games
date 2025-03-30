using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public partial class PlayerScript : NetworkBehaviour
{
    //Player Info
    [SyncVar(hook = nameof(OnNameChanged))] private string _playerName;
    public string playerName => _playerName;
    [SyncVar(hook = nameof(OnNameColorChanged))] private Color _playerColor;
    public Color playerColor => _playerColor;
    [SyncVar] public int connectionID;

    public PuppetScript controlledCharacter;
    public InputActionAsset inputActionAsset;

    [Server]
    public void SetPlayerData(string playerName, Color color)
    {
        _playerName = playerName;
        _playerColor = color;
    }

    [Command]
    void CmdNotifyConnection()
    {
        Debug.Log($"PlayerScript{name}: RpcNotifyConnection called");
        NetworkEventManager.Instance.OnPlayerJoined.Invoke(this);
    }

    [Command]
    void CmdSendMessage(string text)
    {
        FindObjectOfType<PlayerUIScript>().RpcBroadcastClientMessage(netIdentity, text);
    }
}

public partial class PlayerScript : NetworkBehaviour
{
    [Client]
    void OnNameChanged(string _Old, string _New)
    {
        _playerName = _New;
    }
    [Client]
    void OnNameColorChanged(Color _Old, Color _New)
    {
        _playerColor = _New;
    }
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        name = "PlayerID:" + connectionID;


        if (isLocalPlayer)
        {
            CmdNotifyConnection();
        }
    }

    void Awake()
    {
        transform.SetParent(NetworkManager.singleton.GetComponent<CustomNetworkManager>().clientScriptContainer); //Just to make things a bit neater

        //playerUIScript = FindObjectOfType<PlayerUIScript>();
    }

    void Update()
    {

        if (isOwned)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                CmdSendMessage("Hello");
            }
        }
    }
}