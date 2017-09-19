using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Fetches and holds sensorinformation and offers a function for processing sensordatarequests by the networkmanager 
/// </summary>
public class SensorManager : MonoBehaviour
{
    private int testCounter = 0;
    public bool _ledEnabled;
    // Use this for initialization
    void Start ()
    {
        _ledEnabled = false;

    }
	
	// Update is called once per frame
	void Update ()
	{
	    CheckForSensorModes();
	}

    private void CheckForSensorModes()
    {
        // Check the LEDs
        if (Input.GetKeyDown(KeyCode.L) && !_ledEnabled)
        {
            _ledEnabled = true;
            Debug.Log("LEDs enabled!");
        }
        else if (Input.GetKeyDown(KeyCode.L) && _ledEnabled)
        {
            _ledEnabled = false;
            Debug.Log("LEDs disabled!");
        }
    }

    public bool GetLEDsEnabled()
    {
        return _ledEnabled;
    }

    public String GetSensorData(String code)
    {
        Debug.Log(++testCounter);
        String sensorData = "Error";

        switch (code)
        {
            case "GETLEDSENABLED":

                sensorData = GetLEDsEnabled().ToString();
                if ((testCounter%2) == 0)
                {
                    SwitchLED();
                }
                return sensorData;
            default:
                return sensorData;
        }

    }

    /// <summary>
    /// If the LEDs are off, they get turned on and the other way around
    /// </summary>
    private void SwitchLED()
    {
        if (_ledEnabled)
        {
            Debug.Log("LEDs ausgemacht jetzt eigentlich");
            _ledEnabled = false;
        }
        else
        {
            Debug.Log("LEDs angemacht jetzt eigentlich");
            _ledEnabled = true;
        }
    }
}
