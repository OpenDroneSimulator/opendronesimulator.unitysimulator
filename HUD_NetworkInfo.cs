using System;
using UnityEngine;
using System.Collections;
using System.Net;
using UnityEngine.UI;

/// <summary>
/// Used to show the port for the command socket and the IP of the running machine in the UI
/// </summary>
public class HUD_NetworkInfo : MonoBehaviour
{

    public GameObject _networkManager;

	// Use this for initialization
	void Start ()
	{
	    Text text = this.GetComponent<Text>();

	    string host = _networkManager.GetComponent<NetworkManager>().GetIPAddress().ToString();
	    string port = _networkManager.GetComponent<NetworkManager>().getPort().ToString();

        Debug.Log("IP: " + host + ":" + port);

        string ipText = "IP: " + host + "\n" +"Port: " + port;

	    text.text = ipText;
    }


	
	
}
