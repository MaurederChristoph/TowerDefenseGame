using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Loads a new scene
/// </summary>
public class LoadScene : MonoBehaviour {
    /// <summary>
    /// Loads the game
    /// </summary>
    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
    /// <summary>
    /// Loads the main menue
    /// </summary>
    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
}
