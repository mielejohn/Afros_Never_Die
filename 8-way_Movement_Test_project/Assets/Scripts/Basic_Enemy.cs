using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Enemy_States{ Patrolling, Investigating, Attacking};
public enum Enemy_Type{ Basic, Commander};


public class Basic_Enemy : MonoBehaviour {

	[Header("FOV")]
	public GameObject FOV;
	public SpriteRenderer FOV_Sprite;
	[Space]
	[Header("Patrol points")]
	public GameObject patrolPoint1;
	public GameObject patrolPoint2;
	public GameObject selectedPatrolPointGO;
	public Vector3 selectedPatrolPoint;
	public Vector3 investigationPoint;
	[Space]
	[Header("States")]
	public Enemy_States ES;
	[Space]
	[Header ("Patrolling")]
	private float WalkingSpeed= 0.01f;
	[Space]
	[Header ("Investigating")]
	private float SearchingSpeed = 0.02f;
	[Space]
	[Header ("Attacking")]
	public GameObject PlayerofInterest;
	private float AttackingSpeed = 0.02f;

	[Space]
	[Header("Detection")]
	public bool detectingPlayer;
	public bool detectedplayer;
	public float playerDetectedPercent = 0;
	public Image questionMark;
	public GameObject exclamationPoint;
	public PlayerMovement Player;

	[Space]
	[Header("Room Alert")]
	public Room_Controller RC;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
		PlayerofInterest = GameObject.FindGameObjectWithTag ("Player");
		selectedPatrolPointGO = patrolPoint1;
		selectedPatrolPoint = patrolPoint1.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		QuestionMark ();
		//if (detectingPlayer == false && detectedplayer == false && ES == Enemy_States.Patrolling) {

		//}

		switch (ES) {
		case Enemy_States.Patrolling:
			Patrolling ();
			break;
		case Enemy_States.Investigating:
			Investigating ();
			break;
		case Enemy_States.Attacking:
			Attacking ();
			break;
		}
	}

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

	public IEnumerator ResetGaurd(float WaitTime){
		yield return new WaitForSeconds (WaitTime);
		exclamationPoint.SetActive (false);
		detectedplayer = false;
		playerDetectedPercent = 0;
		//Camera_FOV.color = Color.white;
		ES = Enemy_States.Patrolling;
		FOV_Sprite.color = new Color (1, 1, 1, 0.35f);
		//Debug.Log ("Just reset Gaurd");
	}

	public void DetectedPlayer(){
		detectedplayer = true;
		exclamationPoint.SetActive (true);
		Player.stealth_State = Stealth_States.Detected;
		ES = Enemy_States.Attacking;
		//Camera_FOV.color = Color.red;
		FOV_Sprite.color = new Color (1, 0, 0, 0.35f);

	}

	private void SelectNewPatrolPoint(){
		if(selectedPatrolPointGO == patrolPoint1){
			selectedPatrolPoint = patrolPoint2.transform.position;
			selectedPatrolPointGO = patrolPoint2;
		} else if(selectedPatrolPointGO == patrolPoint2){
			selectedPatrolPoint = patrolPoint1.transform.position;
			selectedPatrolPointGO = patrolPoint1;
		}
	}

	public void QuestionMark(){
		questionMark.fillAmount = QuestionMarkMap (playerDetectedPercent, 0, 100, 0, 1);
	}

	private float QuestionMarkMap(float value, float inMin, float inMax, float outMin, float outMax){
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	public void SearchForPlayer(Vector3 LastKnownLocation){
		//Debug.Log("Im going to go search for Afr0");
		//Debug.Log ("Last known location was" + LastKnownLocation);
		investigationPoint = LastKnownLocation;
		Investigating ();
		//yield return new WaitForSeconds (4.0f);
		//detectingPlayer = false;
		//ResetGaurd ();
	}























	protected virtual void Patrolling(){
		transform.position = Vector2.MoveTowards (transform.position, selectedPatrolPoint, WalkingSpeed);
		float distance = Vector2.Distance (transform.position, selectedPatrolPoint);
		if (distance < 0.2) {
			SelectNewPatrolPoint ();
		}

		Vector3 vectorToTarget = selectedPatrolPointGO.transform.position - transform.position;
		float angle = Mathf.Atan2 (-vectorToTarget.x, -vectorToTarget.y) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, -Vector3.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * 20);
	}

	private void Attacking(){
		float distance = Vector3.Distance(transform.position, PlayerofInterest.transform.position);
		transform.position = Vector2.MoveTowards (transform.position, PlayerofInterest.transform.position, AttackingSpeed);

		Vector3 vectorToTarget = PlayerofInterest.transform.position - transform.position;
		float angle = Mathf.Atan2 (-vectorToTarget.x, -vectorToTarget.y) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, -Vector3.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * 2);
	}

	private void Investigating(){
		float distance = Vector3.Distance(transform.position, investigationPoint);
		transform.position = Vector2.MoveTowards (transform.position, investigationPoint, SearchingSpeed);

		Vector3 vectorToTarget = PlayerofInterest.transform.position - transform.position;
		float angle = Mathf.Atan2 (-vectorToTarget.x, -vectorToTarget.y) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, -Vector3.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * 2);
	}

	public void Dead(){
		gameObject.SetActive (false);
	}
}
