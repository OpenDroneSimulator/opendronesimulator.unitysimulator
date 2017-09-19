using UnityEngine;
using System.Collections;

/// <summary>
/// Can be bind to the camera so the user can manually setup the camera distance in the unity editor
/// </summary>
public class CustomCameraFollow : MonoBehaviour
{

    public Transform target;
    public float yDistance, zDistance, rotationSpeed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(target.position.x, target.position.y + yDistance, target.position.z - zDistance);

	    Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, target.rotation.eulerAngles.y, transform.eulerAngles.z);

        transform.LookAt(target.position);
	}
}
