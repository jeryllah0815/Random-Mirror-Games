using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;

public partial class ControllablePlayerObject : NetworkBehaviour
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
    CharacterController _characterController;
    Material _playerMatCache;
    PlayerInput _playerInput;

    [Command]
    public void CmdMove(float horizontal, float vertical)
    {
        transform.Rotate(0, horizontal * _rotateSpeed, 0);
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
        Vector3 moveDirection = transform.forward * vertical;// + transform.right * horizontalInput;
        Vector3 horizontalMovement = moveDirection * _moveSpeed * Time.fixedDeltaTime;

        // Combine horizontal movement + gravity
        Vector3 totalMovement = horizontalMovement + Vector3.up * _verticalVelocity;

        // Apply movement
        _characterController.Move(totalMovement);
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
        _playerMatCache = GetComponent<Renderer>().material;
        _playerMatCache.color = _New;
        GetComponent<Renderer>().material = _playerMatCache;
        UpdateNameText();
    }

    [Client]
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputBuffer  = context.ReadValue<Vector2>();
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
    void UpdateInputBuffer()
    {
        _inputBuffer.x = Input.GetAxisRaw("Horizontal");
        _inputBuffer.y = Input.GetAxisRaw("Vertical");
    }

    [Client]
    void HandleMovement()
    {
        CmdMove(_inputBuffer.x, _inputBuffer.y);
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.enabled = isOwned;
    }

    private void Update()
    {
        if (!isOwned) return; //checks for authority

        UpdateInputBuffer();
    }

    void FixedUpdate()
    {
        if (!isOwned) return; //checks for authority
        
        HandleMovement();
    }
}