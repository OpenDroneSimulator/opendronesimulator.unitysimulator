using UnityEngine;
using System.Collections;

/// <summary>
/// Used to initialize the correct terrain and checks if the drone should be returned to its starting point
/// </summary>
public class SimulationManager : MonoBehaviour
{
    public GameObject quadcopter;
    public GameObject startingPoint;

    private Transform startTransform;
    private Vector3 startPosition;

	// Use this for initialization
	void Awake ()
	{


	    if (GameObject.Find("StartingPoint") != null)
	    {
	        startTransform = GameObject.Find("StartingPoint").transform;

            startPosition = new Vector3(startTransform.position.x, startTransform.transform.position.y,
	            startTransform.transform.position.z);
	    }
	}

    void Start()
    {
 
            if (GameObject.Find("StartingPoint") != null)
            {
                startTransform = GameObject.Find("StartingPoint").transform;

                startPosition = new Vector3(startTransform.position.x, startTransform.transform.position.y,
                    startTransform.transform.position.z);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.R))
	    {
	        quadcopter.GetComponent<Rigidbody>().Sleep();
            quadcopter.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
	        quadcopter.transform.position = startPosition;
            quadcopter.transform.rotation = new Quaternion(0f,0f,0f,0f);

	    }
	}
}
