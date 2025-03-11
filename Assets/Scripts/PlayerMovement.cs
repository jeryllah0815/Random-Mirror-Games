using UnityEngine;
using Mirror;
using TMPro;
using System;

public class PlayerMovement : NetworkBehaviour
{
    public TextMeshPro textPlayerName;
    public GameObject floatingInfo;

    private Material playerMaterialCache;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    void OnColorChanged(Color _Old, Color _New)
    {
        if (!playerMaterialCache)
        {
            playerMaterialCache = new Material(GetComponent<Renderer>().material);
            GetComponent<Renderer>().material = playerMaterialCache;
        }

        textPlayerName.color = _New;
        playerMaterialCache.color = _New;
    }

    void OnNameChanged(string _Old, string _New) => textPlayerName.name = playerName;

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        //floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        //floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = "Player" + UnityEngine.Random.Range(100, 999);
        float r = UnityEngine.Random.Range(0f, 1f);
        float g = UnityEngine.Random.Range(0f, 1f);
        float b = UnityEngine.Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        CmdSetupPlayer(name, color);
    }

    [Command]
    private void CmdSetupPlayer(string name, Color color)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerName = name;
        playerColor = color;
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            // make non-local players run this
            floatingInfo.transform.LookAt(Camera.main.transform);
            return;
        }

        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);
    }
}
