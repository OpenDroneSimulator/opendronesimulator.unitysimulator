using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

/// <summary>
/// Holds the commandvalues 
/// </summary>
public class CommandManager : MonoBehaviour
{
    public GameObject _networkManager;

    private float _pitch, _roll, _yaw, _throttle;

    void Start()
    {
        _pitch = 0f;
        _roll = 0f;
        _yaw = 0f;
        _throttle = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!_networkManager.GetComponent<NetworkManager>().GetCommandString().Equals(""))
        {
            FetchValues(_networkManager.GetComponent<NetworkManager>().GetCommandString());
        }
    }

    /// <summary>
    /// Fetches the pitch, yaw, roll and throttle value from the command queue and pass it to the class variables
    /// </summary>
    /// <param name="cmd">String which holds the values for pitch, yaw, throttle, roll in a json-format</param>
    private void FetchValues(string cmd)
    {   
        // Decodes the json object to a readable form
        JObject json = JObject.Parse(cmd);

        // Fetches the values from the JSON-Object as String values
        float pitch = float.Parse(json.Property("Pitch").Value.ToString(), CultureInfo.InvariantCulture);
        float yaw = float.Parse(json.Property("Yaw").Value.ToString(), CultureInfo.InvariantCulture);
        float roll = float.Parse(json.Property("Roll").Value.ToString(), CultureInfo.InvariantCulture);
        float throttle = float.Parse(json.Property("Throttle").Value.ToString(), CultureInfo.InvariantCulture);

        // Round the values to 4 decimal places
        _pitch = -(float)Math.Round((Decimal)pitch, 4);
        _throttle = -(float)Math.Round((Decimal)throttle, 4);
        _yaw = (float)Math.Round((Decimal)yaw, 4);
        _roll = (float)Math.Round((Decimal)roll, 4);

    }

    /// <summary>
    /// Gets the command queue from the networkmanager
    /// </summary>
    /// <returns>The commands in a Queue of strings </returns>
    private Queue<String> GetCommandQueue()
    {
        Queue<string> commands = _networkManager.GetComponent<NetworkManager>().GetCommandQueue();
        
        return commands;
    }


    /// <summary>
    /// Fetches the throttle command
    /// </summary>
    /// <returns>The value for throttle [-1;1] </returns>
    public float GetThrottle()
    {
        return _throttle;
    }

    /// <summary>
    /// Fetches the pitch command
    /// </summary>
    /// <returns>The value for pitch [-1;1] </returns>
    public float GetPitch()
    {
        return _pitch;
    }

    /// <summary>
    /// Fetches the yaw command
    /// </summary>
    /// <returns>The value for yaw [-1;1] </returns>
    public float GetYaw()
    {
        return _yaw;
    }

    /// <summary>
    /// Fetches the roll command
    /// </summary>
    /// <returns>The value for roll [-1;1] </returns>
    public float GetRoll()
    {
        return _roll;
    }
}
