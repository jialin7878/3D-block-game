﻿using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorInit
{
    static EditorInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
        Debug.Log(pathOfFirstScene + " was set as default play mode scene");
    }
}
