using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	private AudioManager audioManager;

	[SerializeField] private string hoverOverSound = "ButtonHover";
	[SerializeField] private string pressButtonSound = "ButtonPress";

	void Start() {
		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.Log("Error no audio manager found.");
		}
	}

	public void StartGame() {
		audioManager.PlaySound(pressButtonSound);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame() {
		audioManager.PlaySound(pressButtonSound);
		Debug.Log("Game Quit.");
		Application.Quit();
	}

	public void onMouseOver() {
		audioManager.PlaySound(hoverOverSound);
	}

}
