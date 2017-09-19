using UnityEngine;
using System.Collections;

/// <summary>
/// Offers a function which can be bind to a button to act as an return to main menu button.
/// </summary>
public class BackButton : MonoBehaviour
{
    private GameObject MapSelectionCanvas;
    private GameObject MainMenuCanvas;

    void Awake()
    {
        MapSelectionCanvas = GameObject.Find("MapSelectionCanvas");
        MainMenuCanvas = GameObject.Find("MainMenuCanvas");
    }

    // Use this for initialization
    void Start () {
      
    }

    public void BackToMainMenue()
    {
        MapSelectionCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

}
