using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;
	private float[] parallaxScales;
	public float smoothing = 1f;

	private Transform cam;
	private Vector3 previousCamPos;

	// Awake is called before Start(). Great for references
	void Awake() {
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		previousCamPos = cam.position;

		parallaxScales = new float[backgrounds.Length];

		for (int i = 0; i < parallaxScales.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z*-1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < parallaxScales.Length; i++) {
			float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScales[i];
			float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScales[i];

			float backgroundTargetPosX = backgrounds[i].position.x + parallaxX;
			float backgroundTargetPosY = backgrounds[i].position.y + parallaxY;

			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		previousCamPos = cam.position;
	}
}
