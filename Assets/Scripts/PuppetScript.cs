using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;

public partial class PuppetScript : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text _playerNameText;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SyncVar(hook = nameof(OnNameChanged))]
    private string characterName;
    [SyncVar(hook = nameof(OnColorChanged))]
    private Color characterColor;

    float _verticalVelocity = 0f; // Tracks falling speed
    Vector2 _inputBuffer;
    Material _playerMatCache;

    public CharacterController _characterController;
    private PlayerInputHandler _inputHandler;

    [Command]
    public void CmdMove(Vector2 input)
    {
        transform.Rotate(0, input.x * _rotateSpeed, 0);
        // Check if the player is grounded
        bool isGrounded = _characterController.isGrounded;

        // Reset vertical velocity when grounded
        if (isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -0.5f; // Small downward force to stay grounded
        }

        // Apply gravity (always, even when not moving)
        _verticalVelocity += -0.98f * Time.fixedDeltaTime;

        // Calculate movement direction (relative to player's facing direction)
        Vector3 moveDirection = transform.forward * input.y;// + transform.right * horizontalInput;
        Vector3 horizontalMovement = moveDirection * _moveSpeed * Time.fixedDeltaTime;

        // Combine horizontal movement + gravity
        Vector3 totalMovement = horizontalMovement + Vector3.up * _verticalVelocity;

        // Apply movement
        _characterController.Move(totalMovement);
    }

    [Command]
    void CmdSyncPlayerData(PlayerScript ps)
    {
        //Serverside - SERVER
        characterName = ps.playerName;
        characterColor = ps.playerColor;
    }
}

public partial class PuppetScript : NetworkBehaviour
{
    void OnNameChanged(string _Old, string _New)
    {
        name = _New;
        characterName = _New;
        UpdateVisuals();
    }
    void OnColorChanged(Color _Old, Color _New)
    {
        characterColor = _New;
        _playerMatCache = GetComponent<Renderer>().material;
        _playerMatCache.color = _New;
        GetComponent<Renderer>().material = _playerMatCache;
        UpdateVisuals();
    }

    [TargetRpc]
    public void TargetConfigurePuppet(NetworkConnectionToClient conn, PlayerScript ps, LobbyConfig config)
    {
        switch (config.cameraMode)
        {
            case CameraMode.FPP:
                Camera.main.transform.SetParent(transform);
                Camera.main.transform.localPosition = new Vector3(0, 0, 0);
                break;
            case CameraMode.TPP: Debug.LogWarning("CAMERA MODE TPP NOT IMPLEMENTED");
                break;
            case CameraMode.PRESET:
                break;
        }
       
        CmdSyncPlayerData(ps);
    }

    [Client]
    public void UpdateVisuals()
    {
        _playerNameText.text = characterName;
        _playerNameText.color = characterColor;
    }

    [Client]
    void HandleMovement()
    {
        if(_characterController != null)
            CmdMove(_inputHandler.MoveInput);
    }

    void Awake()
    {
        _inputHandler = PlayerInputHandler.Instance;
    }

    void FixedUpdate()
    {
        if (!isOwned) return; //checks for authority
        
        HandleMovement();
    }
}