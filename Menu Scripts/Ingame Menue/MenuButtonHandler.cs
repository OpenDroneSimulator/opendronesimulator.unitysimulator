using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Offers functions for buttons in the game menu to close the menu, exit the game or to return to the mainmenu
/// </summary>
public class MenuButtonHandler : MonoBehaviour {

    public void CloseMenu()
    {

        GameObject gameMenu = GameObject.Find("GameMenu");
        gameMenu.SetActive(false);
    }

    public void ReturnToMainMenu(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
