using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Fonction pour le bouton Start
    public void StartGame()
    {
        // Charge la premi�re sc�ne du jeu
        SceneManager.LoadScene("Loris"); // Assure-toi d'avoir ajout� la sc�ne dans le build settings
    }

    // Fonction pour le bouton Quit
    public void QuitGame()
    {
        Debug.Log("Quit"); // Affiche dans la console pour les tests dans l'�diteur
        Application.Quit(); // Quitte l'application (ne fonctionne pas dans l'�diteur)
    }
}
