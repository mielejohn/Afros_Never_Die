using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Script : MonoBehaviour {

	[Header("Camera Objects")]
	public GameObject cameraBody;
	public GameObject cameraTopObject;
	public SpriteRenderer cameraFOV;

	[Header("Player")]
	private PlayerMovement Player;

	[Header("Detection Items")]
	public float playerDetectedPercent = 0;
	public bool detectingPlayer;
	public bool detectedplayer;
	public Image questionMark;
	public GameObject exclamationPoint;

	[Header("Rotation Numbers")]
	public string Operator1;
	public float rotation1;
	public string Operator2;
	public float rotation2;

	[Header("Rotation")]
	public bool ReverseRotation = false;

	[Header("Room Controllers")]
	public Room_Controller RC;
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();

	}

	void Update () {
		QuestionMark ();
		Quaternion Rotation1_Q = Quaternion.Euler (new Vector3 (0f, 0f, rotation1));
		Quaternion Rotation2_Q = Quaternion.Euler (new Vector3 (0f, 0f, rotation2));
		Quaternion rotation = transform.rotation;
		if (detectingPlayer == false && detectedplayer == false) {

			switch (Operator1) {
			case ">":
				if (ReverseRotation == false && rotation.z >= Rotation1_Q.z) {
					cameraBody.transform.localRotation *= Quaternion.Euler (0, 0, 0.2f);
					//Debug.Log ("Positive Rotation 1");
				} else if(ReverseRotation != true) {
					//Debug.Log ("Making reverse rotation true #1");
					ReverseRotation = true;
				}

				if(ReverseRotation == true) {
					//ReverseRotation = true;
					cameraBody.transform.localRotation *= Quaternion.Euler (0, 0, -0.2f);
					//Debug.Log ("Negative Rotation 1");
				}
				break;

			case "<":
				if (ReverseRotation == false && rotation.z <= Rotation1_Q.z) {
					cameraBody.transform.localRotation *= Quaternion.Euler (0, 0, 0.2f);
					//Debug.Log ("Positive Rotation 2");
				} else  if(ReverseRotation != true) {
					//Debug.Log ("Making reverse rotation true #2");
					ReverseRotation = true;
				}

				if(ReverseRotation == true) {
					//ReverseRotation = true;
					cameraBody.transform.localRotation *= Quaternion.Euler (0, 0, -0.2f);
					//Debug.Log ("Negative Rotation 1");
				}
				break;
			}

			switch (Operator2) {
			case ">":
				//Debug.Log ("In greater than, Operator 2 A");
				if (rotation.z >=  Rotation2_Q.z && ReverseRotation == true) {
					ReverseRotation = false;
					//Debug.Log ("Reverse Rotation = false 3");
				} 
				break;

			case "<":				
				//Debug.Log ("In less than, Operator 2 B");
				if (rotation.z <=  Rotation2_Q.z && ReverseRotation == true) {
					ReverseRotation = false;
					//Debug.Log ("Reverse Rotation = false 4");
				}
				break;
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
		cameraFOV.color = new Color (1, 1, 1, 0.35f);
	}

	private void DetectedPlayer(){
		RC.RoomAlert ();
		detectedplayer = true;
		exclamationPoint.SetActive (true);
		Player.stealth_State = Stealth_States.Detected;
		cameraFOV.color = new Color (1, 0, 0, 0.35f);

	}

	public void QuestionMark(){
		questionMark.fillAmount = QuestionMarkMap (playerDetectedPercent, 0, 100, 0, 1);
	}

	private float QuestionMarkMap(float value, float inMin, float inMax, float outMin, float outMax){
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
