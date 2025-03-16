using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CustomNetworkManager : NetworkManager
{
    public Dictionary<NetworkConnectionToClient, GameObject> playerCharacters = new Dictionary<NetworkConnectionToClient, GameObject>();

    [Header("Custom Variables")]
    public Transform clientScriptContainer;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        NetworkClient.AddPlayer();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("Client Connected & Joined - " + conn.connectionId);
        GameObject go = Instantiate(playerPrefab);
        PlayerScript player = go.GetComponent<PlayerScript>();
        player.connectionID = conn.connectionId;

        #region Rand Gen Name
        //Generate Random Name - probably getting removed later
        string randName = "Player" + Random.Range(100, 999);
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color randColor = new Color(r, g, b);
        #endregion

        player.SetPlayerData(randName, randColor);
        // call this to use this gameobject as the primary controller
        playerCharacters[conn] = player.gameObject;
        NetworkServer.AddPlayerForConnection(conn, go);

        var character = CreateCharacter(conn);
        character.netIdentity.AssignClientAuthority(conn);

        player.PossessCharacter(character);
    }

    [Server]
    ControllablePlayerObject CreateCharacter(NetworkConnectionToClient conn)
    {
        Debug.Log($"Client {conn.connectionId} is creating a character.");

        Transform startPos = GetStartPosition();
        ControllablePlayerObject character = SpawnCharacterPrefab("ControllablePlayer", startPos.position, startPos.rotation);

        return character;
    }

    [Server]
    public ControllablePlayerObject SpawnCharacterPrefab(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = spawnPrefabs.Find(p => p.name == prefabName);
        if (prefab == null)
        {
            Debug.LogError($"Prefab with name {prefabName} not found in spawnable prefabs list.");
            return null;
        }

        // Instantiate and spawn the prefab
        GameObject newObject = Instantiate(prefab, position, rotation);
        NetworkServer.Spawn(newObject);

        return newObject.GetComponent<ControllablePlayerObject>();
    }
}

