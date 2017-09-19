using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Offers functions for the options menu button 
/// </summary>
public class OptionsButton : MonoBehaviour
{
    private GameObject _optionsMenu; 
    private GameObject _gameMenu;
	
    // Use this for initialization
	void Awake () {
        _gameMenu = GameObject.Find("GameMenu");
	    if (_gameMenu == null)
	    {
	        _gameMenu = GameObject.Find("MainMenuCanvas");
	    }
        _optionsMenu = GameObject.Find("OptionsMenu");


    }

    public void OpenOptionsMenu()
    {
        _optionsMenu.SetActive(true);
        SetupInputFieldValues();
        _gameMenu.SetActive(false);
    }

    private void SetupInputFieldValues()
    {
        GameObject.Find("Input_Pitch_P").GetComponent<InputField>().text = OptionsManager.GetPIDPitchP().ToString();
        GameObject.Find("Input_Pitch_I").GetComponent<InputField>().text = OptionsManager.GetPIDPitchI().ToString();
        GameObject.Find("Input_Pitch_D").GetComponent<InputField>().text = OptionsManager.GetPIDPitchD().ToString();
        GameObject.Find("Input_Roll_P").GetComponent<InputField>().text = OptionsManager.GetPIDRollP().ToString();
        GameObject.Find("Input_Roll_I").GetComponent<InputField>().text = OptionsManager.GetPIDRollI().ToString();
        GameObject.Find("Input_Roll_D").GetComponent<InputField>().text = OptionsManager.GetPIDRollD().ToString();
        GameObject.Find("Input_Yaw_P").GetComponent<InputField>().text = OptionsManager.GetPIDYawP().ToString();
        GameObject.Find("Input_Yaw_I").GetComponent<InputField>().text = OptionsManager.GetPIDYawI().ToString();
        GameObject.Find("Input_Yaw_D").GetComponent<InputField>().text = OptionsManager.GetPIDYawD().ToString();

        
    }
}
