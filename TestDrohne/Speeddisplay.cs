using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using Newtonsoft.Json;
using TMPro.Examples;

/// <summary>
/// Used to display the speed of the allocated GameObject to the UI
/// </summary>
public class Speeddisplay : MonoBehaviour
{

    public GameObject quadcopter;
    public Text speedGUIDisplay;
    public Text speedGUIDisplayMPS;

    private int _counter;
    private const string PATH = @"Assets\DroneSpeedtestVerticalData.txt";
    private long _lastTime;

    // Use this for initialization
    void Start ()
    {
        
        _counter = 0;
        _lastTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;  
    }
	
	// Update is called once per frame
	void Update ()
	{
        long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if ((currentTime - _lastTime) >= 500)
	    {
	        _lastTime = currentTime; 
	        ++_counter;

	        float kph = 0.0f;

	        float magnitude = quadcopter.GetComponent<Rigidbody>().velocity.magnitude;

	        if (magnitude > 1 || magnitude < -1)
	        {
	            kph = Mathf.Round(quadcopter.GetComponent<Rigidbody>().velocity.magnitude*3.6f);
	        }

	        speedGUIDisplay.text = kph + " km/h";

	        float mps = quadcopter.GetComponent<Rigidbody>().velocity.magnitude;


	        speedGUIDisplayMPS.text = mps + " m/s";

	       // WriteToFile(mps);
	    }

	}

    private void WriteToFile(float mps)
    {
        string mpsToFile = mps.ToString().Replace('.', ',');

        if (!File.Exists(PATH))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(PATH))
            {
                sw.WriteLine("MPH:");
                sw.WriteLine("----");
                sw.WriteLine("----");
                //sw.WriteLine(mps + " m/s" + " | " + "Pitch: " + Input.GetAxis("Vertical") + " | " + "Throttle: " + Input.GetAxis("VerticalWS") + " | " + _counter);
                sw.WriteLine(mpsToFile + " | " + Input.GetAxis("Vertical") + " | " + Input.GetAxis("VerticalWS") + " | " + _counter);
            }
        }
        else
        {
            // Open the file to read from.
            using (StreamWriter sw = File.AppendText(PATH))
            {
                //sw.WriteLine(mps + " m/s" + " | " + "Pitch: " + Input.GetAxis("Vertical") + " | " + "Throttle: " + Input.GetAxis("VerticalWS") + " | " + _counter);
                sw.WriteLine(mpsToFile + " | " + Input.GetAxis("Vertical") + " | " + Input.GetAxis("VerticalWS") + " | " + _counter);
            }
        }

        
    }
}
