using System;
using UnityEngine;
using System.Collections;

public class LEDTimer : MonoBehaviour
{

    public Light FlLight;
    public Light FrLight;
    public Light RlLight;
    public Light RrLight;

    private float timer = 500f ;
    private float lastTime;

    private bool lightsOn;

    public SensorManager _sManager;

	// Use this for initialization
	void Start ()
	{
	    TurnOffLights();
	    lightsOn = false;
	    lastTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (_sManager._ledEnabled)
	    {
	        float currentTime = Time.time;

	        if (currentTime - lastTime > 0.5)
	        {
	            lastTime = currentTime;
	            if (lightsOn)
	            {
	                TurnOffLights();
	                lightsOn = false;
	            }
	            else
	            {
	                TurnOnLights();
	                lightsOn = true;
	            }
	        }
	    }
	    else
	    {
            TurnOffLights();
	    }
	}

    private void TurnOffLights()
    {
        FlLight.intensity = 0;
        FrLight.intensity = 0;
        RlLight.intensity = 0;
        RrLight.intensity = 0;
    }

    private void TurnOnLights()
    {
        FlLight.intensity = 1;
        FrLight.intensity = 1;
        RlLight.intensity = 1;
        RrLight.intensity = 1;
    }
}
