using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class ArmRotation : MonoBehaviour {

	public int rotationOffset = 0;
	private GameObject player;
	private PlatformerCharacter2D script;
	private bool playerFacingRight;
	
	// Update is called once per frame
	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		Debug.Log(player);
		script = (PlatformerCharacter2D) player.GetComponent<PlatformerCharacter2D>();
		playerFacingRight = player.GetComponent<PlatformerCharacter2D>().facingRight;
	}

	void Update () {
		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();

		float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + rotationOffset);

		if ((rotZ > 90 || rotZ < -90) && playerFacingRight) {
			script.Flip();
			playerFacingRight = false;
		} else if (rotZ < 90 && rotZ > -90 && !playerFacingRight) {
			script.Flip();
			playerFacingRight = true;
		}

	}
}
