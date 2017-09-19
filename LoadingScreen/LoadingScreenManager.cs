using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Used for loading the simulatorscene
/// </summary>
public class LoadingScreenManager : MonoBehaviour
{

    public Slider _slider;

    private AsyncOperation _ansAsyncOperation;

    private GameObject LoadingScreenCanvas;

    void Awake()
    {
        LoadingScreenCanvas = GameObject.Find("LoadingScreenCanvas");
    }

    public void StartLoading(int sceneIndex)
    {
        StartCoroutine(LoadingScreen(sceneIndex));
    }


    IEnumerator LoadingScreen(int sceneIndex)
    {
        LoadingScreenCanvas.SetActive(true);

        _ansAsyncOperation = SceneManager.LoadSceneAsync(sceneIndex); // TODO: hier den übergebenen index nehmen statt 0
        _ansAsyncOperation.allowSceneActivation = false;

        while (_ansAsyncOperation.isDone == false)
        {
            _slider.value = _ansAsyncOperation.progress;

            if (_ansAsyncOperation.progress == 0.9f)
            {
                _slider.value = 1f;
                _ansAsyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
