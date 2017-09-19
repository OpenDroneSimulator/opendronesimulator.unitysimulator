using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Used for setting up the optionsmenu in runtime
/// </summary>
public class MenuHandler : MonoBehaviour
{
    private GameObject gameMenu;
    private GameObject optionsMenu;

    
	// Use this for initialization
	void Start () {
        gameMenu = GameObject.Find("GameMenu");
        optionsMenu = GameObject.Find("OptionsMenu");
        SetupOptionsMenuValues();
        gameMenu.SetActive(false);
        optionsMenu.SetActive(false);	    
	}

    private void SetupOptionsMenuValues()
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

    // Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameMenu.activeSelf || optionsMenu.activeSelf)
            {
                gameMenu.SetActive(false);
                optionsMenu.SetActive(false);

            }
            else
            {
                gameMenu.SetActive(true);
            }
        }
    }
}
