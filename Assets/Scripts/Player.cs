using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class Player : MonoBehaviour {
	private PlayerStats stats;

	public float fallBoundary = -10f;

	public string deathSound = "DeathVoice";
	public string[] damageSound = new string[] {"Grunt1", "Grunt2"};

	[SerializeField] private StatusIndicator statusIndicator;

	private AudioManager audioManager;

	void Start() {
		stats = PlayerStats.instance;

		stats.curHealth = stats.maxHealth;

		if (statusIndicator == null) {
			Debug.LogError ("No status indicator referenced on player");
		} else {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}

		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.Log("Error no audio manager found.");
		}

		InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
	}

	private void RegenHealth() {
		stats.curHealth += 1;
		statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
	}

	void Update() {
		if (transform.position.y <= fallBoundary) {
			DamagePlayer(999999);
		}
	}

	void OnUpgradeMenuToggle(bool active) { 
		GetComponent<Platformer2DUserControl>().enabled = !active;
		Weapon _weapon = GetComponentInChildren<Weapon>();
		if (_weapon != null) {
			_weapon.enabled = !active;
		}
	}

	public void DamagePlayer (int damage) {
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) {
			audioManager.PlaySound(deathSound);
			GameMaster.KillPlayer(this);
		} else {
			int test = Random.Range(0, 2);
			audioManager.PlaySound(damageSound[test]);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}

	void OnDestroy() {
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}

}
