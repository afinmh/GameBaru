using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Camera mainMenuCamera;

    private void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (canvas != null) canvas.SetActive(false);

        if (player != null) player.SetActive(false);
        if (mainMenuCamera != null) mainMenuCamera.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (canvas != null) canvas.SetActive(true);

        if (mainMenuCamera != null) mainMenuCamera.gameObject.SetActive(false);
        if (player != null) player.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
