using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Holds all the necessary options and parameters for the PID controllers of the drone
/// </summary>
public static class OptionsManager
{

    private const String configFilename = "config.json";

    // Customizable Values
    private static float _pidPitch_p;
    private static float _pidPitch_i;
    private static float _pidPitch_d;
    private static float _pidYaw_p;
    private static float _pidYaw_i;
    private static float _pidYaw_d;
    private static float _pidRoll_p;
    private static float _pidRoll_i;
    private static float _pidRoll_d;

    // Standard
    private static float _standardPidPitch_p;
    private static float _standardPidPitch_i;
    private static float _standardPidPitch_d;
    private static float _standardPidRoll_p;
    private static float _standardPidRoll_i;
    private static float _standardPidRoll_d;
    private static float _standardPidYaw_p ;
    private static float _standardPidYaw_i;
    private static float _standardPidYaw_d;

    // Static Constructor
    static OptionsManager()
    {
        Initialize();
    }

    private static void Initialize()
    {
        // Builds up the filepath for the config.json in the streamingassetspath folder
        String filepath = Path.Combine(Application.streamingAssetsPath, configFilename);


        // read JSON directly from file in the streamingassetspath folder, FilePath declares the exact path
        using (StreamReader file = File.OpenText(filepath))
        using(JsonTextReader reader = new JsonTextReader(file))
        {
            JObject jsonConfig = (JObject)JToken.ReadFrom(reader);

            _standardPidPitch_p = float.Parse(jsonConfig.Property("standardPitchP").Value.ToString(), CultureInfo.InvariantCulture);
            _standardPidPitch_i = float.Parse(jsonConfig.Property("standardPitchI").Value.ToString(), CultureInfo.InvariantCulture);
            _standardPidPitch_d = float.Parse(jsonConfig.Property("standardPitchD").Value.ToString(), CultureInfo.InvariantCulture);

            _standardPidRoll_p = float.Parse(jsonConfig.Property("standardRollP").Value.ToString(), CultureInfo.InvariantCulture);
            _standardPidRoll_i = float.Parse(jsonConfig.Property("standardRollI").Value.ToString(), CultureInfo.InvariantCulture);
            _standardPidRoll_d = float.Parse(jsonConfig.Property("standardRollD").Value.ToString(), CultureInfo.InvariantCulture);

            _standardPidYaw_p = float.Parse(jsonConfig.Property("standardYawP").Value.ToString(), CultureInfo.InvariantCulture);
            _standardPidYaw_i = float.Parse(jsonConfig.Property("standardYawI").Value.ToString(), CultureInfo.InvariantCulture);
            _standardPidYaw_d = float.Parse(jsonConfig.Property("standardYawD").Value.ToString(), CultureInfo.InvariantCulture);
        }

        if (PlayerPrefs.HasKey("pitchP"))
        {
            Debug.Log("AKtueller Pitchwert: " + PlayerPrefs.GetFloat("pitchP"));
        }
               
    }


    // Pitch
    public static void SetPIDPitchP(float pValue)
    {
        Debug.Log("ES wird ganz frech ein Wert gesetzt für PitchP undzwar: " + pValue);
        PlayerPrefs.SetFloat("pitchP", pValue);
    }

    public static void SetPIDPitchI(float iValue)
    {
        PlayerPrefs.SetFloat("pitchI", iValue);
    }

    public static void SetPIDPitchD(float dValue)
    {
        PlayerPrefs.SetFloat("pitchD", dValue);
    }

    // Roll
    public static void SetPIDRollP(float pValue)
    {
        PlayerPrefs.SetFloat("rollP", pValue);
    }

    public static void SetPIDRollI(float iValue)
    {
        PlayerPrefs.SetFloat("rollI", iValue);
    }

    public static void SetPIDRollD(float dValue)
    {
        PlayerPrefs.SetFloat("rollD", dValue);
    }

    // Yaw
    public static void SetPIDYawP(float pValue)
    {
        PlayerPrefs.SetFloat("yawP", pValue);
    }

    public static void SetPIDYawI(float iValue)
    {
        PlayerPrefs.SetFloat("yawI", iValue);
    }

    public static void SetPIDYawD(float dValue)
    {
        PlayerPrefs.SetFloat("yawD", dValue);
    }


    // Getters
    public static float GetPIDPitchP()
    {
        if (PlayerPrefs.HasKey("pitchP"))
        {
            Debug.Log("Wert der von der Methode kommt: " + PlayerPrefs.GetFloat("pitchP"));
            return PlayerPrefs.GetFloat("pitchP");
        }
        
        Debug.Log("Der hurensohn kommt doch zum Standardwert der nutten arsch");
        return _standardPidPitch_p;          
    }

    public static float GetPIDPitchI()
    {
        if (PlayerPrefs.HasKey("pitchI"))
        {
            return PlayerPrefs.GetFloat("pitchI");
        }
        return _standardPidPitch_i;
    }

    public static float GetPIDPitchD()
    {
        if (PlayerPrefs.HasKey("pitchD"))
        {
            return PlayerPrefs.GetFloat("pitchD");
        }

        return _standardPidPitch_d;
    }

    public static float GetPIDRollP()
    {
        if (PlayerPrefs.HasKey("rollP"))
        {
            return PlayerPrefs.GetFloat("rollP");
        }

        return _standardPidRoll_p;
    }

    public static float GetPIDRollI()
    {
        if (PlayerPrefs.HasKey("rollI"))
        {
            return PlayerPrefs.GetFloat("rollI");
        }

        return _standardPidRoll_i;
    }

    public static float GetPIDRollD()
    {
        if (PlayerPrefs.HasKey("rollD"))
        {
            return PlayerPrefs.GetFloat("rollD");
        }
        return _standardPidRoll_d;
    }

    public static float GetPIDYawP()
    {
        if (PlayerPrefs.HasKey("yawP"))
        {
            return PlayerPrefs.GetFloat("yawP");
        }
        return _standardPidYaw_p;
    }

    public static float GetPIDYawI()
    {
        if (PlayerPrefs.HasKey("yawI"))
        {
            return PlayerPrefs.GetFloat("yawI");
        }
        return _standardPidYaw_i;
    }

    public static float GetPIDYawD()
    {
        if (PlayerPrefs.HasKey("yawD"))
        {
            return PlayerPrefs.GetFloat("yawD");
        }

        return _standardPidYaw_d;
    }

    // Standard Values
    // Pitch
    public static float GetStandardPidPitchP()
    {
        return _standardPidPitch_p;
    }

    public static float GetStandardPidPitchI()
    {
        return _standardPidPitch_i;
    }

    public static float GetStandardPidPitchD()
    {
        return _standardPidPitch_d;
    }

    // Roll
    public static float GetStandardPidRollP()
    {
        return _standardPidRoll_p;
    }

    public static float GetStandardPidRollI()
    {
        return _standardPidRoll_i;
    }

    public static float GetStandardPidRollD()
    {
        return _standardPidRoll_d;
    }

    // Yaw
    public static float GetStandardPidYawP()
    {
        return _standardPidYaw_p;
    }

    public static float GetStandardPidYawI()
    {
        return _standardPidYaw_i;
    }

    public static float GetStandardPidYawD()
    {
        return _standardPidYaw_d;
    }


}
