using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	private AudioManager audioManager;

	private string mouseHoverSound = "ButtonHover";
	private string buttonPressSound = "ButtonPress";

	void Start() {
		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.Log("No audio manager found.");
		}
	}

	public void Quit() {
		audioManager.PlaySound(buttonPressSound);
		Debug.Log("APPLICATION QUIT!");
		Application.Quit();
	}

	public void Retry() {
		audioManager.PlaySound(buttonPressSound);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnMouseOver() {
		audioManager.PlaySound(mouseHoverSound);
	}

}
