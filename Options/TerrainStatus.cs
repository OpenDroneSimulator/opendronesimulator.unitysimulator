using UnityEngine;
using System.Collections;

/// <summary>
/// Used for terrain prefabs to hold a startingpoint
/// </summary>
public class TerrainStatus : MonoBehaviour
{

    public GameObject StartingPoint;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 GetStartingPointPosition()
    {
        Transform startingTransform = StartingPoint.transform;

        return new Vector3(startingTransform.position.x, startingTransform.transform.position.y,
       startingTransform.transform.position.z);
    }
}
