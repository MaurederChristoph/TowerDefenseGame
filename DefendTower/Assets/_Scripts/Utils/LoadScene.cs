using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public void StartGame() {
		SceneManager.LoadScene("Game");
	}

	public void MainMenu() {
		SceneManager.LoadScene("Main Menu");
	}
}
