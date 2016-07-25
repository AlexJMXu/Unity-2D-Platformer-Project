using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;
	public static Camera2DFollow maincam;

	[SerializeField] private int maxLives = 3;
	public static int _remainingLives;
	public static int RemainingLives {
		get { return _remainingLives; }
		set { _remainingLives = value; }
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 3.5f;
	public Transform spawnPrefab;
	public string respawnCountdownSound = "Respawn";
	public string spawnSound = "Spawn";
	public string gameOverSound = "GameOver";

	public CameraShake cameraShake;

	[SerializeField] private GameObject gameOverUI;
	[SerializeField] private GameObject upgradeMenu;

	[SerializeField] private WaveSpawner waveSpawner;

	public delegate void UpgradeMenuCallback(bool active);
	public UpgradeMenuCallback onToggleUpgradeMenu;

	private AudioManager audioManager;

	[SerializeField] private int startMoney;
	public static int Money;

	void Awake() {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}

		maincam = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject.GetComponent<Camera2DFollow>();
	}

	void Start() {
		if (cameraShake == null) {
			Debug.LogError("No camera shake referenced in game master");
		}
	
		RemainingLives = maxLives;

		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.LogError ("No audio manager found.");
		}

		Money = startMoney;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.U)) {
			ToggleUpgradeMenu();
		}
	}

	private void ToggleUpgradeMenu() {
		upgradeMenu.SetActive(!upgradeMenu.activeSelf);
		waveSpawner.enabled = !upgradeMenu.activeSelf;
		onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
	}

	public void EndGame() {
		audioManager.PlaySound(gameOverSound);
		Debug.Log("Game Over!");
		gameOverUI.SetActive(true);
	}

	public IEnumerator _RespawnPlayer() {
		audioManager.PlaySound(respawnCountdownSound);
		yield return new WaitForSeconds (spawnDelay);

		Transform newPlayer = (Transform) Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		maincam.target = newPlayer;

		Transform tempParticles = (Transform) Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy(tempParticles.gameObject, 3f);
		audioManager.PlaySound(spawnSound);
	} 

	public static void KillPlayer(Player player) {
		Destroy(player.gameObject);
		_remainingLives -= 1;
		if (_remainingLives <= 0) {
			gm.EndGame();
		} else {
			gm.StartCoroutine(gm._RespawnPlayer());
		}
	}

	public static void KillEnemy(Enemy enemy) {
		gm._KillEnemy(enemy);
	}

	public void _KillEnemy(Enemy _enemy) {
		Transform _clone = (Transform) Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
		Destroy(_clone.gameObject, 5);
		cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
		Destroy(_enemy.gameObject);
		audioManager.PlaySound(_enemy.deathSound);
		Money += _enemy.moneyDrop;
		audioManager.PlaySound("Money");
	}

}
