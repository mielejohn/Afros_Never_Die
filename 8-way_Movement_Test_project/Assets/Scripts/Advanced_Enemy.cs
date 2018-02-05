using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyAiStates{Patrolling, Chasing, Attacking};


public class Advanced_Enemy : MonoBehaviour {

	[Space]
	[Header("AI items")]
	public EnemyAiStates state;
	static protected List<GameObject> patrolPoints = null;

	#region Enemy Options
	[Space]
	[Header("Speeds")]
	public float walkingSpeed = 30.0f;
	public float ChaseSpeed = 30.0f;
	public float AttackSpeed =30.5f;

	[Space]
	[Header("Distances")]
	public float attackingDistance = 250.0f;
	public float chasingDistance = 750.0f;
	public float ShootingDistance = 200.0f;
	#endregion

	[Space]
	[Header("Attacking")]

	[Space]
	[Header("Misc.")]
	protected GameObject patrollingInterestPoint;
	public GameObject PlayerOfInterest;

	void Awake(){
		if (patrolPoints == null) {
			patrolPoints = new List<GameObject> ();
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("NavPatrolPoints")) {
				patrolPoints.Add (go);
			}
		}
	}


	protected virtual void Start () {
		SwitchToPatrolling ();
	}

	protected void Update () {
			PlayerOfInterest = GameObject.FindGameObjectWithTag ("Player");
			switch (state) {
			case EnemyAiStates.Attacking:
				OnAttackingUpdate ();
				break;
			case EnemyAiStates.Chasing:
				OnChasingUpdate ();
				break;
			case EnemyAiStates.Patrolling:
				OnPatrollingUpdate ();
				break;

			}

			if (state == EnemyAiStates.Chasing || state == EnemyAiStates.Attacking) {
				Vector2 targetDir = PlayerOfInterest.transform.position - transform.position;
				Quaternion lookRotation = Quaternion.LookRotation (targetDir);
				Vector2 rotation = lookRotation.eulerAngles;
				transform.rotation = Quaternion.Euler (0f, rotation.y, 0f);
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			}

			if (state == EnemyAiStates.Patrolling) {
				transform.rotation = Quaternion.LookRotation (patrollingInterestPoint.transform.position - transform.position, Vector3.up);
			}
	}

	protected virtual void OnAttackingUpdate(){
		//Debug.Log ("OnAttackingStarted");
		float step = AttackSpeed * Time.deltaTime;

		float distance = Vector3.Distance (transform.position, PlayerOfInterest.transform.position);
		if (distance>attackingDistance) {
			SwitchToChasing (PlayerOfInterest);
			//Debug.Log ("SwitchingToChasing");
		}
	}

	protected virtual void OnChasingUpdate(){
		//Debug.Log ("OnChasingStarted");
		float step = ChaseSpeed * Time.deltaTime;
		transform.position = Vector2.MoveTowards (transform.position, PlayerOfInterest.transform.position, step);

		float distance = Vector2.Distance (transform.position, PlayerOfInterest.transform.position);
		//Debug.Log ("About to switch to attacking");
		if (distance <= attackingDistance) {
			SwitchToAttacking (PlayerOfInterest);
			//Debug.Log ("SwitchingToAttacking");
		}
	}

	protected virtual void OnPatrollingUpdate(){
		//Debug.Log ("OnPatrollingStarted");
		float step = walkingSpeed * Time.deltaTime;
		transform.position = Vector2.MoveTowards (transform.position, patrollingInterestPoint.transform.position, step);

		float PlayerDistance = Vector2.Distance (transform.position, PlayerOfInterest.transform.position);
		float distance = Vector2.Distance (transform.position, patrollingInterestPoint.transform.position);

		if (PlayerDistance <= chasingDistance) {
			//Debug.Log ("SwitchingToChasing");
			SwitchToChasing (PlayerOfInterest);
		}

		if (distance <= 1) {
			//Debug.Log ("SelectingRandom point");
			SelectRandomPatrolPoint();

		}

	}

	protected void OnTriggerEnter(Collider other){ 

	}

	protected void OnTriggerExit(Collider collider) { 
		SwitchToPatrolling (); 
	}

	protected void SwitchToPatrolling(){
		state = EnemyAiStates.Patrolling;
		SelectRandomPatrolPoint();
		PlayerOfInterest = null;
	}

	protected void SwitchToAttacking(GameObject target){
		state = EnemyAiStates.Attacking;
	}

	protected void SwitchToChasing(GameObject target){
		state = EnemyAiStates.Chasing;
		PlayerOfInterest = target;
	}

	protected virtual void SelectRandomPatrolPoint(){
		//Debug.Log ("Choosing a Random point");
		int choice = Random.Range (0, patrolPoints.Count);
		patrollingInterestPoint = patrolPoints [choice];
		//Debug.Log ("Patrol points are: " + patrolPoints);
	}
		
}
