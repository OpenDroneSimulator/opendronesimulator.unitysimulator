using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Is used to startup a new scenario. Should be allocated to an image which represents a scenario. 
/// The Scenario index represents the respective scenario and will be instantiated in the simulator scene.
/// </summary>
public class MapSelection : MonoBehaviour, IPointerClickHandler
{

    private int simulatorSceneMapIndex;
    public int ScenarioIndex;
    public GameObject LoadingScreenManager;

	// Use this for initialization
	void Start ()
	{
	    simulatorSceneMapIndex = 1;
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        SimulatorSceneManager.setScene(ScenarioIndex);
        LoadingScreenManager.GetComponent<LoadingScreenManager>().StartLoading(simulatorSceneMapIndex);

        GameObject.Find("MapSelectionCanvas").SetActive(false);
    }

}
