using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagers : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject player; // <- Tambahkan referensi ke player

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);

        if (player) player.SetActive(false); // Nonaktifkan player
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        mainMenuUI.SetActive(false);

        if (player) player.SetActive(true); // Aktifkan kembali player
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenSettings()
    {
        Debug.Log("Settings opened (belum diimplementasi)");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
