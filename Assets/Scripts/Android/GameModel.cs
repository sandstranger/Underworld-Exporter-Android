﻿using System;
using System.IO;
using UnderworldExporter.Game;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public sealed class GameModel
{
    public const int MinimalSavesCount = 4;
    
    private static readonly string _pathToModelOnDisk = Path.Combine(Application.persistentDataPath, "gamemodel.json");

    static GameModel()
    {
        CurrentModel = LoadGameModel();
    }

    public static GameModel CurrentModel { get; }

    public int MaxFps = 60;

    public string BasePath = string.Empty;

    public float MouseXSensitivity = 0.3f;

    public float MouseYSensitivity = 0.3f;

    public float TouchYSensitivity = 15f;

    public float TouchXSensitivity = 15;

    public float GyroXSensitivity = 2;

    public float GyroYSensitivity = 1;

    public float GamepadXSensitivity = 5;

    public float GamepadYSensitivity = 5;

    public float GamepadMouseEmulationSensitivity = 1000;

    public float DefaultLightLevel = 8;

    public float FOV = 75;

    public bool InfiniteMana;

    public bool GodMode;

    public bool ContextUIEnabled = true;

    public string UW1SoundBank = string.Empty;

    public bool SpeakableNpc;

    public bool AutoEat = true;

    public bool AutoKeyUse = true;

    public bool HideScreenControls;

    public bool PreferOriginalHud;

    public bool InvertGyroXAxis = false;

    public bool InvertGyroYAxis = false;

    public bool InvertYAxis = false;

    public bool InvertXAxis = false;

    public bool EnableGyroscope;

    public bool PreferFullScreenTouchCameraInMouseMode = true;

    public bool ShowFps;

    public float PlayerSpeed = 1.0f;

    public float PlayerSwimSpeed = 1.0f;

    public bool EnableHaptickFeedback = true;

    public LogLevel LogLevel = LogLevel.None;
    
    public bool NoClipEnable = false;

    public int MaxSavesCount = MinimalSavesCount;
    
    [FormerlySerializedAs("InfiniteFlyModeEnable")] public bool InfiniteFlyMode = false;
    
    public void Save() => File.WriteAllText(_pathToModelOnDisk, JsonUtility.ToJson(this, prettyPrint: true));

    private static GameModel LoadGameModel() => File.Exists(_pathToModelOnDisk) ? JsonUtility.FromJson<GameModel>(File.ReadAllText(_pathToModelOnDisk)) : new GameModel();
}