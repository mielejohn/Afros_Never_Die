using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Stealth_States {Hidden, Detected};

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D RB;
	//[SerializeField]
	//private float moveX = 0;
	//[SerializeField]
	//private float moveY = 0;
	[SerializeField]
	private float speed = 7;

	public float LerpSpeed = 2.0f;
	public SpriteRenderer player_sprite;
	[Header("Player States")]
	[SerializeField]
	public Player_States player_state;
	[SerializeField]
	public Stealth_States stealth_State;
	[Header("Enemies")]
	public GameObject ClosestEnemy;
	//public GameObject[] Enemies_Player;
	public List<float> Enemy_Distances_List = new List<float> ();
	public List<GameObject> Enemies_Player_List = new List<GameObject> ();
	public List<string> Test_List = new List<string> ();
	//public float[] Enemy_Distances;

	void Awake(){
		Test_List.Add ("Awake Add");
	}

	void Start () {
		RB = this.GetComponent<Rigidbody2D> ();
		Test_List.Add ("Start Add");
	}

	void FixedUpdate(){
		float moveX = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown(KeyCode.A)) {
			player_sprite.flipX= true;
		}

		if (Input.GetKeyDown(KeyCode.D)) {
			player_sprite.flipX= false;
		}
		float moveY = Input.GetAxis ("Vertical");
		Vector2 movement = new Vector2 (moveX, moveY);
		RB.velocity = movement * speed;

		if (Input.GetButton ("L_Shift")) {
			player_state = Player_States.Crouching;
			transform.localScale = new Vector3 (0.8164563f,0.6164563f,0.8164563f);
			speed = 3;
		} else if (Input.GetButtonUp ("L_Shift")) {
			player_state = Player_States.Walking;
			transform.localScale = new Vector3 (0.8164563f,0.8164563f,0.8164563f);
			speed = 5;
		}

		if (Input.GetKeyDown (KeyCode.E) && ClosestEnemy != null) {
			ClosestEnemy.GetComponent<Basic_Enemy> ().Dead();
			ClosestEnemy = null;
		}
	}



	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other){
		/*Debug.Log ("Hitting something");
		if (other.gameObject.tag == "Enemy") {
			Enemy = other.gameObject;
		}*/
	}

	public void getEnemies(List<GameObject> Enemies){
		//Enemies_Player = new GameObject[Enemies.Length];
		//Enemy_Distances = new float[Enemies.Length];

		for (int i = 0; i < Enemies.Count; i++) {
			Enemies_Player_List.Add (Enemies [i].gameObject);
		}
		Debug.Log ("Grabbing Enemies");
		StartCoroutine (CheckEnemyDistance ());
	}

	public void RemoveEnemies(){
		for (int i = 0; i < Enemies_Player_List.Count; i++) {
			//Enemies_Player [i] = null;
			Enemies_Player_List.RemoveAt(i);
		}
		Debug.Log ("Removing Enemies");
	}

	private IEnumerator CheckEnemyDistance(){
		if (Enemies_Player_List.Count == 1) {
			ClosestEnemy = Enemies_Player_List [0];
		} else {
			for (int i = 0; i < Enemies_Player_List.Count; i++) {
				float distance = Vector2.Distance (transform.position, Enemies_Player_List [i].gameObject.transform.position);
				Enemy_Distances_List.Add (distance);
			}
			ClosestEnemy = Enemies_Player_List [SmallestDistance ()];
		}
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (CheckEnemyDistance ());
	}

	private int SmallestDistance(){
		int indexOfClosestEnemy = 0;

		for (int i = 0; i < Enemy_Distances_List.Count-1; i++) {
			if (Enemy_Distances_List [i] < Enemy_Distances_List [i + 1]) {
				indexOfClosestEnemy = i;
			}
		}
		Debug.Log ("Checked Distances");
		for (int i = 0; i < Enemy_Distances_List.Count; i++) {
			Enemy_Distances_List.RemoveAt (i);
		}
		Debug.Log ("Index of closest enemy is" + indexOfClosestEnemy);
		return indexOfClosestEnemy;
	}
}
