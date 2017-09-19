using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Allocates indexes to the particular scenarios
/// </summary>
public static class SimulatorSceneManager
{
    private static int CurrentSceneIndex;
    /**
    *   SceneIndex = 0 -> City
    *   SceneIndex = 1 -> Island 
    */
    internal static void setScene(int scenarioIndex)
    {
        CurrentSceneIndex = scenarioIndex;
    }

    public static String GetCurrentTerrainPrefabName()
    {
        switch (CurrentSceneIndex)
        {
            case 0:
                return "CityTerrain";
            case 1:
                return "IslandTerrain";
            default:
                return "Error";

        }
    }
}
