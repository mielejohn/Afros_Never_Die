using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Script : MonoBehaviour {

	public GameObject cameraBody;
	public GameObject cameraTopObject;
	private PlayerMovement Player;
	public SpriteRenderer cameraFOV;
	public float playerDetectedPercent = 0;
	public bool detectingPlayer;
	public bool detectedplayer;
	public Image questionMark;
	public GameObject exclamationPoint;


	[Header("Rotation")]
	public bool ReverseRotation = false;
	//private float rotatespeed = 0.5f;
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
		//gameObject.transform.forward = new Vector3 (0, 0, 1);
		//cameraBody.transform.forward = new Vector3 (0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		QuestionMark ();
		/*Vector3 targetDir = Player.gameObject.transform.position - cameraBody.transform.position;
		float step = Time.deltaTime * rotatespeed;
		Vector3 newDir = Vector3.RotateTowards (cameraBody.transform.forward, targetDir, step, 0.0f);
		if (detectedplayer == true) {
			Debug.Log ("Found player...ROTATING");

			cameraBody.transform.rotation = Quaternion.LookRotation (new Vector3(newDir.x, 0.0f, newDir.z));
			//cameraTopObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
		}*/

		if (detectingPlayer == false && detectedplayer == false) {
		if (ReverseRotation == false && cameraBody.transform.eulerAngles.z <= 63.0f) {
				//Debug.Log ("Less than 63");
				cameraBody.transform.localRotation *= Quaternion.Euler (0, 0, 0.4f);
			} else {
				ReverseRotation = true;
				//Debug.Log ("Else statement, greater than 63");

				cameraBody.transform.localRotation *= Quaternion.Euler (0, 0, -0.4f);
			if (cameraBody.transform.eulerAngles.z <= 0.5f) {
					//Debug.Log ("Less than 0.5");
					ReverseRotation = false;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" && Player.player_state == Player_States.Walking) {
			//Debug.Log ("Colliding with a walking player");
			DetectedPlayer ();
		} if (other.tag == "Player" && Player.player_state == Player_States.Crouching) {
			//Debug.Log ("Colliding with a crouching player");
			detectingPlayer = true;
			StartCoroutine (Detecting ());
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			detectingPlayer = false;
			ResetCamera ();
		}
	}

	private IEnumerator Detecting(){
		while (detectingPlayer == true) {
			if (playerDetectedPercent < 99) {
				playerDetectedPercent += 2;
				yield return new WaitForSeconds (0.1f);
				//StartCoroutine (Detecting ());
			} else {
				DetectedPlayer();
				playerDetectedPercent = 0;
				detectingPlayer = false;
			}

			if (detectingPlayer == true && Player.player_state == Player_States.Walking) {
				DetectedPlayer();
				playerDetectedPercent = 0;
				detectingPlayer = false;
			}
		}
	}

	private void ResetCamera(){
		exclamationPoint.SetActive (false);
		detectedplayer = false;
		playerDetectedPercent = 0;
		//Camera_FOV.color = Color.white;
		cameraFOV.color = new Color (1, 1, 1, 0.35f);
	}

	private void DetectedPlayer(){
		detectedplayer = true;
		exclamationPoint.SetActive (true);
		Player.stealth_State = Stealth_States.Detected;
		//Camera_FOV.color = Color.red;
		cameraFOV.color = new Color (1, 0, 0, 0.35f);

	}

	public void QuestionMark(){
		questionMark.fillAmount = QuestionMarkMap (playerDetectedPercent, 0, 100, 0, 1);
	}

	private float QuestionMarkMap(float value, float inMin, float inMax, float outMin, float outMax){
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
