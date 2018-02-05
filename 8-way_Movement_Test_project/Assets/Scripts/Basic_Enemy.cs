using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Enemy_States{ Patrolling, Investigating, Attacking};


public class Basic_Enemy : MonoBehaviour {

	[Header("FOV")]
	public GameObject FOV;
	public SpriteRenderer FOV_Sprite;
	[Space]
	[Header("Patrol points")]
	public GameObject patrolPoint1;
	public GameObject patrolPoint2;
	public GameObject selectedPatrolPoint;
	[Space]
	[Header ("Attacking")]
	public GameObject PlayerofInterest;
	[Space]
	[Header("Detection")]
	public bool detectingPlayer;
	public bool detectedplayer;
	public float playerDetectedPercent = 0;
	public Image questionMark;
	public GameObject exclamationPoint;
	public PlayerMovement Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
		selectedPatrolPoint = patrolPoint1;
	}
	
	// Update is called once per frame
	void Update () {
		QuestionMark ();
		if (detectingPlayer == false && detectedplayer == false) {
			transform.position = Vector2.MoveTowards (transform.position, selectedPatrolPoint.transform.position, 0.05f);
			float distance = Vector2.Distance (transform.position, selectedPatrolPoint.transform.position);
			if (distance < 0.2) {
				SelectNewPatrolPoint ();
			}
		}
	}

	/*void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" && Player.player_state == Player_States.Walking) {
			Debug.Log ("Colliding with a walking player");
			DetectedPlayer ();
		} if (other.tag == "Player" && Player.player_state == Player_States.Crouching) {
			Debug.Log ("Colliding with a crouching player");
			detectingPlayer = true;
			StartCoroutine (Detecting ());
		}
	}*/

	public IEnumerator Detecting(){
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

	public void ResetGaurd(){
		exclamationPoint.SetActive (false);
		detectedplayer = false;
		playerDetectedPercent = 0;
		//Camera_FOV.color = Color.white;
		FOV_Sprite.color = new Color (1, 1, 1, 0.35f);
	}

	public void DetectedPlayer(){
		detectedplayer = true;
		exclamationPoint.SetActive (true);
		Player.stealth_State = Stealth_States.Detected;
		//Camera_FOV.color = Color.red;
		FOV_Sprite.color = new Color (1, 0, 0, 0.35f);

	}

	private void SelectNewPatrolPoint(){
		if(selectedPatrolPoint == patrolPoint1){
			selectedPatrolPoint = patrolPoint2;
		} else if(selectedPatrolPoint == patrolPoint2){
			selectedPatrolPoint = patrolPoint1;
		}
	}

	public void QuestionMark(){
		questionMark.fillAmount = QuestionMarkMap (playerDetectedPercent, 0, 100, 0, 1);
	}

	private float QuestionMarkMap(float value, float inMin, float inMax, float outMin, float outMax){
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
