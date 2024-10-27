using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Build 1"); // Remplace "Build 1" par le nom exact de ta scène de jeu
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu"); // Remplace "Menu" par le nom exact de ta scène de menu principal
    }
}
