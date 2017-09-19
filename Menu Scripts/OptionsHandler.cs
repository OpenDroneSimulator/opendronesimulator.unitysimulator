using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;

/// <summary>
/// Handels the Optionsmenu in the main and ingame menu
/// </summary>
public class OptionsHandler : MonoBehaviour
{

    private GameObject _optionsMenu;

    private GameObject _gameMenu;

    private GameObject _pidInputPitchP;
    private GameObject _pidInputPitchI;
    private GameObject _pidInputPitchD;
    private GameObject _pidInputRollP;
    private GameObject _pidInputRollI;
    private GameObject _pidInputRollD;
    private GameObject _pidInputYawP;
    private GameObject _pidInputYawI;
    private GameObject _pidInputYawD;

	// Use this for initialization
	void Awake () {
	    _gameMenu = GameObject.Find("GameMenu"); 
        _optionsMenu = GameObject.Find("OptionsMenu");

	    _pidInputPitchP = GameObject.Find("Input_Pitch_P");
        _pidInputPitchI = GameObject.Find("Input_Pitch_I");
        _pidInputPitchD = GameObject.Find("Input_Pitch_D");
        _pidInputRollP = GameObject.Find("Input_Roll_P");
        _pidInputRollI = GameObject.Find("Input_Roll_I");
        _pidInputRollD = GameObject.Find("Input_Roll_D");
        _pidInputYawP = GameObject.Find("Input_Yaw_P");
        _pidInputYawI = GameObject.Find("Input_Yaw_I");
        _pidInputYawD = GameObject.Find("Input_Yaw_D");
    }
	

    public void Apply()
    {
        // Saves the input values to the options manager
        SavePitchValues();
        SaveRollValues();
        SaveYawValues();

        PlayerPrefs.Save();

        // Applys the new values to the Drone
        ApplyPitch();
        ApplyRoll();
        ApplyYaw();

        
    }

    private void SavePitchValues()
    {
        float pValue = float.Parse(_pidInputPitchP.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDPitchP(pValue);

        float iValue = float.Parse(_pidInputPitchI.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDPitchI(iValue);

        float dValue = float.Parse(_pidInputPitchD.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDPitchD(dValue);
    }

    private void SaveYawValues()
    {
        float pValue = float.Parse(_pidInputYawP.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDYawP(pValue);

        float iValue = float.Parse(_pidInputYawI.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDYawI(iValue);

        float dValue = float.Parse(_pidInputYawD.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDYawD(dValue);
    }

    private void SaveRollValues()
    {
        float pValue = float.Parse(_pidInputRollP.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDYawP(pValue);

        float iValue = float.Parse(_pidInputRollI.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDYawI(iValue);

        float dValue = float.Parse(_pidInputRollD.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDYawD(dValue);
    }



    public void Reset()
    {
        Vector3 pitchStandardGains = GetPitchStandardVector3();
        GameObject.Find("MainDrone").GetComponent<DroneController>().SetPitchGains(pitchStandardGains);

        Vector3 rollStandardGains = GetRollStandardVector3();
        GameObject.Find("MainDrone").GetComponent<DroneController>().SetPitchGains(rollStandardGains);

        Vector3 yawStandardGains = GetYawStandardVector3();
        GameObject.Find("MainDrone").GetComponent<DroneController>().SetPitchGains(yawStandardGains);

        ResetInputFieldValues();
    }

    private void ResetInputFieldValues()
    {
        GameObject.Find("Input_Pitch_P").GetComponent<InputField>().text = OptionsManager.GetStandardPidPitchP().ToString();
        GameObject.Find("Input_Pitch_I").GetComponent<InputField>().text = OptionsManager.GetStandardPidPitchI().ToString();
        GameObject.Find("Input_Pitch_D").GetComponent<InputField>().text = OptionsManager.GetStandardPidPitchD().ToString();
        GameObject.Find("Input_Roll_P").GetComponent<InputField>().text = OptionsManager.GetStandardPidRollP().ToString();
        GameObject.Find("Input_Roll_I").GetComponent<InputField>().text = OptionsManager.GetStandardPidRollI().ToString();
        GameObject.Find("Input_Roll_D").GetComponent<InputField>().text = OptionsManager.GetStandardPidRollD().ToString();
        GameObject.Find("Input_Yaw_P").GetComponent<InputField>().text = OptionsManager.GetStandardPidYawP().ToString();
        GameObject.Find("Input_Yaw_I").GetComponent<InputField>().text = OptionsManager.GetStandardPidYawI().ToString();
        GameObject.Find("Input_Yaw_D").GetComponent<InputField>().text = OptionsManager.GetStandardPidYawD().ToString();
    }

    public void BackToGameMenu()
    {
        _gameMenu.SetActive(true);
        
        _optionsMenu.SetActive(false);
    }

    private void ApplyYaw()
    {
        Vector3 yawGains = new Vector3(OptionsManager.GetPIDYawP(), OptionsManager.GetPIDYawI(), OptionsManager.GetPIDYawD());
        GameObject.Find("MainDrone").GetComponent<DroneController>().SetYawGains(yawGains);
    }

    private void ApplyRoll()
    {
        Vector3 rollGains = new Vector3(OptionsManager.GetPIDRollP(), OptionsManager.GetPIDRollI(), OptionsManager.GetPIDRollD());
        GameObject.Find("MainDrone").GetComponent<DroneController>().SetRollGains(rollGains);
    }

    private void ApplyPitch()
    {
        Vector3 pitchGains = new Vector3(OptionsManager.GetPIDPitchP(), OptionsManager.GetPIDPitchI(), OptionsManager.GetPIDPitchD());
        GameObject.Find("MainDrone").GetComponent<DroneController>().SetPitchGains(pitchGains);
    }

    private Vector3 GetPitchStandardVector3()
    {
        Vector3 pitchStandardGains = new Vector3(OptionsManager.GetStandardPidPitchP(), OptionsManager.GetStandardPidPitchI(), OptionsManager.GetStandardPidPitchD());
        return pitchStandardGains;
    }

    private Vector3 GetRollStandardVector3()
    {
        Vector3 rollStandardGains = new Vector3(OptionsManager.GetStandardPidRollP(), OptionsManager.GetStandardPidRollI(), OptionsManager.GetStandardPidRollD());
        return rollStandardGains;
    }

    private Vector3 GetYawStandardVector3()
    {
        Vector3 yawStandardGains = new Vector3(OptionsManager.GetStandardPidYawP(), OptionsManager.GetStandardPidYawI(), OptionsManager.GetStandardPidYawD());
        return yawStandardGains;
    }
}
