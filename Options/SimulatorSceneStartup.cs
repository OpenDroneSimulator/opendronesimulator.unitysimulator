using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used for setting up the simulator scene
/// </summary>
public class SimulatorSceneStartup : MonoBehaviour
{
    public GameObject CityTerrain;
    public GameObject IslandTerrain;

    public GameObject Drone;

    private HashSet<GameObject> Terrains;

    public Vector3 startingPoint;

    void Start()
    {
        InitTerrainList();

        LoadTerrain(SimulatorSceneManager.GetCurrentTerrainPrefabName());     
    }

    private void InitTerrainList()
    {
        Terrains = new HashSet<GameObject>();
        Terrains.Add(CityTerrain);
        Terrains.Add(IslandTerrain);
    }

    private void LoadTerrain(string terrainPrefabName)
    {
        foreach(GameObject go in Terrains)
        {
            if (go.name.Equals(terrainPrefabName))
            {

                Vector3 spawnPoint = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
                Debug.Log("hier müsste was initialisiert werden mit dem Namen: " + go.name);
                Instantiate(go, spawnPoint, Quaternion.Euler(0, 0, 0));

                startingPoint = go.GetComponent<TerrainStatus>().GetStartingPointPosition();

                ResetDronePosition();

                return;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetDronePosition();
        }
    }


    private void ResetDronePosition()
    {
        Drone.GetComponent<Rigidbody>().Sleep();
        Drone.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Drone.transform.position = startingPoint;
        Drone.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }
}
