using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public partial class ControllablePlayerObject : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text _playerNameText;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string characterName;
    [SyncVar(hook = nameof(OnColorChanged))]
    private Color characterColor;

    Material playerMatCache;
    
    [Command]
    public void CmdMove(float moveX, float moveZ)
    {
        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);
    }
    
    [Server]
    public void SetPlayerScript(PlayerScript ps) 
    {
        characterName = ps.playerName;
        characterColor = ps.playerColor;
        //will call syncvarhooks
    }


    
}

public partial class ControllablePlayerObject : NetworkBehaviour
{
    void OnNameChanged(string _Old, string _New)
    {
        name = _New;
        characterName = _New;
        UpdateNameText();
    }
    void OnColorChanged(Color _Old, Color _New)
    {
        characterColor = _New;
        playerMatCache = GetComponent<Renderer>().material;
        playerMatCache.color = _New;
        GetComponent<Renderer>().material = playerMatCache;
        UpdateNameText();
    }

    [TargetRpc]
    public void TargetSetCamera(NetworkConnectionToClient conn)
    {
        if (!isOwned) return;
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
    }


    [Client]
    public void UpdateNameText()
    {
        _playerNameText.text = characterName;
        _playerNameText.color = characterColor;
    }

    [Client]
    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        CmdMove(moveX, moveZ);
    }
    void Update()
    {
        if (!isOwned) return; //checks for authority

        HandleMovement();
    }
}