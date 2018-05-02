using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Enemy_States{ Patrolling, Investigating, Attacking};
public enum Enemy_Type{ Basic, Commander};
public enum Patrol_Type{ Stationary, Moveable};


public class Basic_Enemy : MonoBehaviour {

	[Header("GameManager")]
	public GameManager gameManager;

	[Header("GameManager")]
	public GameObject TopObject;

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

	[Header("States")]
	public Enemy_States CurrentState;
	public Enemy_States EnemyState;
	public Enemy_Type EnemyType;
	public Patrol_Type PatrolType;

	[Header("Rotation Numbers")]
	public string Operator1;
	public float rotation1;
	public string Operator2;
	public float rotation2;

	[Header("Rotation")]
	public bool ReverseRotation = false;

	[Header ("Patrolling")]
	private float WalkingSpeed= 0.03f;

	[Header ("Investigating")]
	private float SearchingSpeed = 0.02f;

	[Header ("Attacking")]
	public GameObject PlayerofInterest;
	private float AttackingSpeed = 0.04f;

	[Header("Detection")]
	public bool detectingPlayer;
	public bool detectedplayer;
	public float playerDetectedPercent = 0;
	public Image questionMark;
	public GameObject exclamationPoint;
	public PlayerController Player;

	[Header("Room Controller")]
	public Room_Controller RC;

	[Header("UI")]
	public GameObject Keyboard_Key;
	public GameObject Controller_Key;

	[Header("Gun")]
	public GameObject GunTop_Object;
	public bool canShoot;

	[Header("Flipped")]
	public bool flip;

	void Awake(){
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		PlayerofInterest = GameObject.FindGameObjectWithTag ("Player");
		selectedPatrolPointGO = patrolPoint1;
		selectedPatrolPoint = patrolPoint1.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		QuestionMark ();
		//if (detectingPlayer == false && detectedplayer == false && ES == Enemy_States.Patrolling) {

		//}
		switch (CurrentState) {
		case Enemy_States.Patrolling:
			if (PatrolType == Patrol_Type.Moveable) {
				Patrolling ();
			} else if (PatrolType == Patrol_Type.Stationary && CurrentState != Enemy_States.Investigating || CurrentState != Enemy_States.Attacking) {
				StationaryLook ();
			}
			break;
		case Enemy_States.Investigating:
			Investigating ();
			break;
		case Enemy_States.Attacking:
			Attacking ();
			break;
		}

		if (FOV.transform.rotation.z < -0.1f) {
			flip = true;
		} else {
			flip = false;
		}

		if (flip == true) {
			this.transform.localScale = new Vector3 (-1f, 1f, 1f);
			GunTop_Object.transform.localScale = new Vector3 (1f, 1f, 1f);
		} else {
			this.transform.localScale = new Vector3 (1f, 1f, 1f);
			GunTop_Object.transform.localScale = new Vector3 (-1f, -1f, 1f);
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
		canShoot = false;
		playerDetectedPercent = 0;
		detectingPlayer = false;
		FOV_Sprite.color = new Color (1, 1, 1, 0.35f);
		if (PatrolType == Patrol_Type.Moveable) {
			CurrentState = Enemy_States.Patrolling;
		} else if (PatrolType == Patrol_Type.Stationary) {
			ReturnToPosition ();
		}
		//Debug.Log ("Just reset Gaurd");
	}

	public void DetectedPlayer(){
		detectedplayer = true;
		exclamationPoint.SetActive (true);
		Player.stealth_State = Stealth_States.Detected;
		CurrentState = Enemy_States.Attacking;
		//Camera_FOV.color = Color.red;
		FOV_Sprite.color = new Color (1, 0, 0, 0.35f);

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

	public void DisplayKey(){
		Keyboard_Key.SetActive (true);
	}

	public void RemoveKey(){
		Keyboard_Key.SetActive (false);
	}

	protected virtual void Patrolling(){
		if (Time.timeScale != 0.0f) {
			TopObject.transform.position = Vector2.MoveTowards (TopObject.transform.position, selectedPatrolPoint, WalkingSpeed);
			float distance = Vector2.Distance (TopObject.transform.position, selectedPatrolPoint);
			if (distance < 0.2) {
				StartCoroutine (SelectNewPatrolPoint ());
			}

			Vector3 vectorToTarget = selectedPatrolPointGO.transform.position - transform.position;
			float angle = Mathf.Atan2 (-vectorToTarget.x, -vectorToTarget.y) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis (angle, -Vector3.forward);
			FOV.transform.rotation = Quaternion.Slerp (FOV.transform.rotation, q, Time.deltaTime * 20);
		}
	}


	private IEnumerator SelectNewPatrolPoint(){
		if(selectedPatrolPointGO == patrolPoint1){
			yield return new WaitForSeconds (0.7f);
			selectedPatrolPoint = patrolPoint2.transform.position;
			selectedPatrolPointGO = patrolPoint2;
		} else if(selectedPatrolPointGO == patrolPoint2){
			yield return new WaitForSeconds (0.7f);
			selectedPatrolPoint = patrolPoint1.transform.position;
			selectedPatrolPointGO = patrolPoint1;
		}
	}

	private void Attacking ()
	{

		if (Time.timeScale != 0.0f) {
			float distance = Vector3.Distance (TopObject.transform.position, PlayerofInterest.transform.position);
			if (distance > 5.0f) {
				TopObject.transform.position = Vector2.MoveTowards (TopObject.transform.position, PlayerofInterest.transform.position, AttackingSpeed);
			}

			Vector3 vectorToTarget = PlayerofInterest.transform.position - transform.position;
			float angle = Mathf.Atan2 (-vectorToTarget.x, -vectorToTarget.y) * Mathf.Rad2Deg;
			Vector3 q = Quaternion.AngleAxis (angle, -Vector3.forward).eulerAngles;
			FOV.transform.rotation = Quaternion.Slerp (FOV.transform.rotation, Quaternion.Euler(q), Time.deltaTime * 2);

			Vector3 vectorToTarget_2 = PlayerofInterest.transform.position - GunTop_Object.transform.position;
			float angle_2 = Mathf.Atan2 (-vectorToTarget_2.x, -vectorToTarget_2.y) * Mathf.Rad2Deg;
			Vector3 q_2 = Quaternion.AngleAxis (angle_2, -Vector3.forward).eulerAngles;
			q_2.z += 90;
			GunTop_Object.transform.rotation = Quaternion.Slerp (GunTop_Object.transform.rotation, Quaternion.Euler(q_2), Time.deltaTime * 2);

			/*var Player = PlayerofInterest.transform.position;
			var screentoPoint = transform.position;
			var offset = new Vector2 (Player.x - screentoPoint.x, Player.y - screentoPoint.y);
			var angle_2 = Mathf.Atan2 (offset.y, offset.x) * Mathf.Rad2Deg;
			GunTop_Object.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle_2));*/

			if (distance < 5.0f && PlayerofInterest.GetComponent<PlayerController>().health_state != HealthyNess.Dead) {
				canShoot = true;
				Debug.Log("Can Shoot");
			} else if(PlayerofInterest.GetComponent<PlayerController>().health_state == HealthyNess.Dead){
				canShoot = false;
			}
		}
	}

	private void Investigating(){
		if (Time.timeScale != 0.0f) {
			float distance = Vector3.Distance (TopObject.transform.position, investigationPoint);
			TopObject.transform.position = Vector2.MoveTowards (TopObject.transform.position, investigationPoint, SearchingSpeed);

			Vector3 vectorToTarget = PlayerofInterest.transform.position - transform.position;
			float angle = Mathf.Atan2 (-vectorToTarget.x, -vectorToTarget.y) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis (angle, -Vector3.forward);
			FOV.transform.rotation = Quaternion.Slerp (FOV.transform.rotation, q, Time.deltaTime * 2);
		}
	}

	private void StationaryLook(){
		Quaternion Rotation1_Q = Quaternion.Euler (new Vector3 (0f, 0f, rotation1));
		Quaternion Rotation2_Q = Quaternion.Euler (new Vector3 (0f, 0f, rotation2));
		Quaternion rotation = transform.rotation;
		if (detectingPlayer == false && detectedplayer == false && Time.timeScale != 0.0f) {

			switch (Operator1) {
			case ">":
				if (ReverseRotation == false && FOV.transform.rotation.z >= Rotation1_Q.z) {
					FOV.transform.localRotation *= Quaternion.Euler (0, 0, 0.2f);
				} else if(ReverseRotation != true) {
					ReverseRotation = true;
				}

				if(ReverseRotation == true) {
					FOV.transform.localRotation *= Quaternion.Euler (0, 0, -0.2f);
				}
				break;

			case "<":
				if (ReverseRotation == false && FOV.transform.rotation.z <= Rotation1_Q.z) {
					FOV.transform.localRotation *= Quaternion.Euler (0, 0, 0.2f);
				} else  if(ReverseRotation != true) {
					ReverseRotation = true;
				}

				if(ReverseRotation == true) {
					FOV.transform.localRotation *= Quaternion.Euler (0, 0, -0.2f);
				}
				break;
			}

			switch (Operator2) {
			case ">":
				if (FOV.transform.rotation.z >=  Rotation2_Q.z && ReverseRotation == true) {
					ReverseRotation = false;
				} 
				break;

			case "<":				
				if (FOV.transform.rotation.z <=  Rotation2_Q.z && ReverseRotation == true) {
					ReverseRotation = false;
				}
				break;
			}

		}
	}

	public void Dead(){
		Debug.Log (this.gameObject + " is DEAD");
		PlayerofInterest.GetComponent<PlayerController> ().ClosestEnemy = null;
		RC.RemoveEnemy (this.gameObject);
		Destroy (TopObject);
		//TopObject.SetActive (false);
	}

	private void ReturnToPosition(){
		float distance = Vector2.Distance (TopObject.transform.position, selectedPatrolPoint);
		while (distance > 1) {
			TopObject.transform.position = Vector2.MoveTowards (TopObject.transform.position, selectedPatrolPoint, WalkingSpeed);
			distance = Vector2.Distance (TopObject.transform.position, selectedPatrolPoint);
		}
		CurrentState = Enemy_States.Patrolling;
	}

}
