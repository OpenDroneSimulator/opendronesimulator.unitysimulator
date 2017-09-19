using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;

/// <summary>
/// Used to setup the options menu in the mainmenu
/// </summary>
public class MainMenuOptionsHandler : MonoBehaviour {

    private GameObject _optionsMenu;

    public GameObject GameMenu;

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
    void Start()
    {
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

        SetupStartingValues();
    }

    private void SetupStartingValues()
    {
        Debug.Log("Wert der vom Optionsmanager zurück kommt: " + OptionsManager.GetPIDPitchP().ToString());
        _pidInputPitchP.GetComponent<InputField>().text = OptionsManager.GetPIDPitchP().ToString();
        _pidInputPitchI.GetComponent<InputField>().text = OptionsManager.GetPIDPitchI().ToString();
        _pidInputPitchD.GetComponent<InputField>().text = OptionsManager.GetPIDPitchD().ToString();

        _pidInputRollP.GetComponent<InputField>().text = OptionsManager.GetPIDRollP().ToString();
        _pidInputRollI.GetComponent<InputField>().text = OptionsManager.GetPIDRollI().ToString();
        _pidInputRollD.GetComponent<InputField>().text = OptionsManager.GetPIDRollD().ToString();

        _pidInputYawP.GetComponent<InputField>().text = OptionsManager.GetPIDYawP().ToString();
        _pidInputYawI.GetComponent<InputField>().text = OptionsManager.GetPIDYawI().ToString();
        _pidInputYawD.GetComponent<InputField>().text = OptionsManager.GetPIDYawD().ToString();
    }


    public void Apply()
    {
        // Saves the input values to the options manager
        SavePitchValues();
        SaveRollValues();
        SaveYawValues();
        PlayerPrefs.Save();
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
        OptionsManager.SetPIDRollP(pValue);

        float iValue = float.Parse(_pidInputRollI.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDRollI(iValue);

        float dValue = float.Parse(_pidInputRollD.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat);
        OptionsManager.SetPIDRollD(dValue);
    }



    public void Reset()
    {
        OptionsManager.SetPIDPitchP(OptionsManager.GetStandardPidPitchP());
        OptionsManager.SetPIDPitchI(OptionsManager.GetStandardPidPitchI());
        OptionsManager.SetPIDPitchD(OptionsManager.GetStandardPidPitchD());
        OptionsManager.SetPIDRollP(OptionsManager.GetStandardPidRollP());
        OptionsManager.SetPIDRollI(OptionsManager.GetStandardPidRollI());
        OptionsManager.SetPIDRollD(OptionsManager.GetStandardPidRollD());
        OptionsManager.SetPIDYawP(OptionsManager.GetStandardPidYawP());
        OptionsManager.SetPIDYawI(OptionsManager.GetStandardPidYawI());
        OptionsManager.SetPIDYawD(OptionsManager.GetStandardPidYawD());

        ResetInputFieldValues();

        PlayerPrefs.Save();
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

        GameMenu.SetActive(true);

        _optionsMenu.SetActive(false);
    }

}
