using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

	[SerializeField] private WaveSpawner spawner;
	[SerializeField] private Animator waveAnimator;
	[SerializeField] private Text waveCountdownText;
	[SerializeField] private Text waveCountText;

	private WaveSpawner.SpawnState previousState;

	// Use this for initialization
	void Start () {
		if (spawner == null) {
			Debug.LogError("No spawner referenced!");
			this.enabled = false;
		}
		if (waveAnimator == null) {
			Debug.LogError("No waveAnimator referenced!");
			this.enabled = false;
		}
		if (waveCountdownText == null) {
			Debug.LogError("No waveCountdownText referenced!");
			this.enabled = false;
		}
		if (waveCountText == null) {
			Debug.LogError("No waveCountText referenced!");
			this.enabled = false;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (spawner.State) {
			case WaveSpawner.SpawnState.COUNTING:
				UpdateCountingUI();
				break;
			case WaveSpawner.SpawnState.SPAWNING:
				UpdateSpawningUI();
				break;
		}

		previousState = spawner.State;
	
	}

	void UpdateCountingUI () {
		if (previousState != WaveSpawner.SpawnState.COUNTING) {
			waveAnimator.SetBool("WaveIncoming", false);
			waveAnimator.SetBool("WaveCountdown", true);
			Debug.Log("COUNTING");
		}

		waveCountdownText.text = ((int) spawner.WaveCountdown).ToString();
	}

	void UpdateSpawningUI () {
		if (previousState != WaveSpawner.SpawnState.SPAWNING) {
			waveAnimator.SetBool("WaveIncoming", true);
			waveAnimator.SetBool("WaveCountdown", false);
			Debug.Log("SPAWNING");
		}

		waveCountText.text = spawner.NextWave.ToString();
	}
}
