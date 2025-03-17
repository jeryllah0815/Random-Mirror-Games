using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LobbyConfig", menuName = "ScriptableObjects/LobbyConfig", order = 1)]
public class LobbyConfig : ScriptableObject
{
    public int maxPlayers = 2;
    public CameraMode cameraMode;
    public GameMode gameMode;
}

public enum GameMode
{
    TENNIS
}

public enum CameraMode
{
    FPP,
    TPP,
    PRESET
}