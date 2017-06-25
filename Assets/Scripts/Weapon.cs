using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 5f;
	public int damage = 10;
	public LayerMask whatToHit;

	public Transform bulletTrailPrefab;
	public Transform hitPrefab;
	public Transform muzzleFlashPrefab;

	private float timeToSpawnEffect = 0f;
	private float effectSpawnRate = 10f;
	private float timeToFire = 0f;
	private Transform firePoint;

	public float camShakeAmount = 0.03f;
	public float camShakeLength = 0.1f;
	CameraShake camShake;

	public string weaponShootSound = "DefaultShot";

	//caching
	AudioManager audioManager;


	void Awake () {
		firePoint = transform.Find("FirePoint");
		if (firePoint == null) {
			Debug.LogError("Yo yo can't find firepoint");
		}
	}

	void Start() {
		camShake = GameMaster.gm.GetComponent<CameraShake>();
		if (camShake == null) {
			Debug.LogError("No CameraShake script found on GM object.");
		}

		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.LogError ("No audio manager found.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fireRate == 0f) {
			if (Input.GetButtonDown("Fire1")) {
				Shoot();
			}
		} else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
				timeToFire = Time.time + 1/fireRate;
				Shoot();
			}
		}
	}

	void Shoot() {
		Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
			Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, whatToHit);

		//Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100, Color.cyan);

		if (hit.collider != null) {
			//Debug.DrawLine(firePointPosition, hit.point, Color.red);

			Enemy enemy = hit.collider.GetComponent<Enemy>();
			if (enemy != null) {
				enemy.DamageEnemy(damage);
				// Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage.");
			}
		}

		if (Time.time >= timeToSpawnEffect) {
			Vector3 hitPos;
			Vector3 hitNormal;

			if (hit.collider == null) {
				hitPos = (mousePosition-firePointPosition) * 30;
				hitNormal = new Vector3 (9999,9999,9999);
			} else {
				hitPos = hit.point;
				hitNormal = hit.normal;
			}

			Effect(hitPos, hitNormal);
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}

	}

	void Effect (Vector3 hitPos, Vector3 hitNormal) {
		Transform trail =  (Transform) Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
		LineRenderer lr = trail.GetComponent<LineRenderer>();

		if (lr != null) {
			lr.SetPosition(0, firePoint.position);
			lr.SetPosition(1, hitPos);
		}

		Destroy(trail.gameObject, 0.04f);

		if (hitNormal != new Vector3 (9999,9999,9999)) {
			Transform impactParticles = (Transform) Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
			Destroy(impactParticles.gameObject, 1f);
		}

		Transform muzzleFlash = (Transform) Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
		muzzleFlash.parent = firePoint;
		float size = Random.Range(0.6f, 0.9f);
		muzzleFlash.localScale = new Vector3(size, size, size);
		Destroy (muzzleFlash.gameObject, 0.02f);

		// Shake the camera
		camShake.Shake(camShakeAmount, camShakeLength);

		// Play shoot sound
		audioManager.PlaySound(weaponShootSound);
	}
}
