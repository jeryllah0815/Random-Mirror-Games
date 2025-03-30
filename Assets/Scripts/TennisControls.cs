using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
public class TennisControls : NetworkBehaviour
{
    PlayerInputHandler _InputHandler;

    [Command]
    public void CmdShoot()
    {
        Debug.Log("SHOOT");
    }

    void Awake()
    {
        _InputHandler = PlayerInputHandler.Instance;
    }
    void Update()
    {
        if (!isOwned) return;

        if(_InputHandler.FireInput)
        {
            CmdShoot();
        }
    }
}
    