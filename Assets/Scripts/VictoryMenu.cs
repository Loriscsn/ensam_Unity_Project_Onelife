using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    // Fonction pour retourner au menu principal
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu"); // Remplace "MainMenu" par le nom exact de ta scène de menu principal
    }

    // Fonction pour redémarrer la partie
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recharge la scène actuelle
    }
}
