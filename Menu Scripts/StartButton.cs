using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Offers functions for a button to startup a new scenario 
/// </summary>
public class StartButton : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject MapSelectionCanvas;

   

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }


    public void ShowMapSelection()
    {
        MapSelectionCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false); 
    }
}
