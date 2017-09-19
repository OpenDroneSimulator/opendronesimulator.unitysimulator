using UnityEngine;
using System.Collections;

/// <summary>
/// Used to setup the main menu. Disables and enables particular canvas for the start.
/// </summary>
public class MainMenueStart : MonoBehaviour
{
    private GameObject _mapSelectionCanvas;
    private GameObject _loadingScreenCanvas;
    private GameObject _optionsMenu;

    // Use this for initialization
    void Start ()
    {
        _mapSelectionCanvas = GameObject.Find("MapSelectionCanvas");
        _loadingScreenCanvas = GameObject.Find("LoadingScreenCanvas");
        _optionsMenu = GameObject.Find("OptionsMenu");

        _mapSelectionCanvas.SetActive(false);
        _loadingScreenCanvas.SetActive(false);
        _optionsMenu.SetActive(false);
    }
	

}
