using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Canvas mainCanvas; // Assign Main Canvas in the Inspector
    public Canvas loadingCanvas; // Assign Loading Canvas in the Inspector
    public Canvas dashboardCanvas; // Assign Dashboard Canvas in the Inspector

    void Start()
    {
        // Initially show Main Canvas and hide others
        mainCanvas.gameObject.SetActive(true);
        loadingCanvas.gameObject.SetActive(false);
        dashboardCanvas.gameObject.SetActive(false);

        // Find the Play Button in the Main Canvas and add listener
        Button playButton = mainCanvas.GetComponentInChildren<Button>();
        if (playButton != null)
        {
            playButton.onClick.AddListener(StartLoading);
        }
        else
        {
            Debug.LogError("Play button not found in Main Canvas!");
        }
    }

    private void StartLoading()
    {
        mainCanvas.gameObject.SetActive(false); // Hide Main Canvas
        loadingCanvas.gameObject.SetActive(true); // Show Loading Canvas
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        // Wait for 3 seconds to simulate loading time
        yield return new WaitForSeconds(4f);

        // Hide Loading Canvas and show Dashboard Canvas
        loadingCanvas.gameObject.SetActive(false);
        dashboardCanvas.gameObject.SetActive(true);
    }
}
