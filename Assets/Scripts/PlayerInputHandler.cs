using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Action Name References")]
    [SerializeField] InputActionAsset _InputActionAsset;

    //InputAction set by InputActionAsset's ActionMap
    //Where all the callbacks and adjustments should be at
    public InputAction MoveAction { get; private set; }
    public InputAction LookAction { get; private set; }
    public InputAction FireAction { get; private set; }
    //Values modified by inputactions
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool FireInput { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    void Awake()
    {
        #region Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        var actionMap = _InputActionAsset.FindActionMap("Player");
        MoveAction = actionMap.FindAction("Move");
        LookAction = actionMap.FindAction("Look");
        FireAction = actionMap.FindAction("Fire");

        RegisterInputAction();
    }

    void RegisterInputAction()
    {
        MoveAction.performed    += ctx => MoveInput = ctx.ReadValue<Vector2>();
        MoveAction.canceled     += ctx => MoveInput = Vector2.zero;

        LookAction.performed    += ctx => LookInput = ctx.ReadValue<Vector2>();
        LookAction.canceled     += ctx => LookInput = Vector2.zero;

        FireAction.performed += ctx => FireInput = true;
        FireAction.canceled += ctx => FireInput = false;

        Debug.Log("Input handler registered");
    }
    void OnEnable()
    {
        MoveAction.Enable();
        LookAction.Enable();
        FireAction.Enable();
    }

    void OnDisable()
    {
        MoveAction.Disable();
        LookAction.Disable();
        FireAction.Disable();
    }
}
