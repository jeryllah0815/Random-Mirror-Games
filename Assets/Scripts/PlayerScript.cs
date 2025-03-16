using UnityEngine;
using Mirror;
using System;

public class PlayerScript : NetworkBehaviour
{
    //Player Info
    [SyncVar(hook = nameof(OnNameChanged))] private string _playerName;
    public string playerName => _playerName;
    [SyncVar(hook = nameof(OnNameColorChanged))] private Color _playerColor;
    public Color playerColor => _playerColor;
    [SyncVar] public int connectionID;

    public ControllablePlayerObject controlledCharacter;

    //Object References
    private CustomNetworkManager networkManager;
    //private PlayerUIScript playerUIScript;
    void OnNameChanged(string _Old, string _New)
    {
        _playerName = _New;
    }
    void OnNameColorChanged(Color _Old, Color _New)
    {
        _playerColor = _New;
    }
    public void PossessCharacter(ControllablePlayerObject character)
    {
        controlledCharacter = character;
        character.SetPlayerScript(this);
    }

    [Server]
    public void SetPlayerData(string playerName, Color color)
    {
        _playerName = playerName;
        _playerColor = color;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        name = "Player id:" + connectionID;
    }

    void Awake()
    {
        networkManager = FindObjectOfType<CustomNetworkManager>();
        transform.SetParent(networkManager.clientScriptContainer); //Just to make things a bit neater

        //playerUIScript = FindObjectOfType<PlayerUIScript>();
    }
   
}
