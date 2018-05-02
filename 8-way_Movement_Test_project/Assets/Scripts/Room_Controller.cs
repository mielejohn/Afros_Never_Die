using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Room_State{Undiscovered, Discovered, Alerted};

public class Room_Controller : MonoBehaviour {

	public List<GameObject> Enemies = new List<GameObject> ();
	public GameObject[] Cameras;

	public GameObject Player;
	public PlayerController PM;

	[Header("Room Discovery")]
	public GameObject[] roomCovers;
	public Room_State RS = Room_State.Undiscovered;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Enemies.Count == 0 && PM != null) {
			PM.stealth_State = Stealth_States.Hidden;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		//Debug.Log ("Something just Collided");
		if (other.tag == "Player") {
			Player = other.gameObject;
			PM = Player.GetComponent<PlayerController> ();
			PM.getEnemies (Enemies);
			PM.RC = this;
		}

		if (RS == Room_State.Undiscovered && other.tag == "Player") {
			//Debug.Log ("room Discovered");
			RS = Room_State.Discovered;
			for (int i = 0; i < roomCovers.Length; i++) {
				roomCovers [i].SetActive (false);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			PM.RemoveEnemies ();
			PM.stealth_State = Stealth_States.Hidden;
			StartCoroutine(ResetRoom ());
		}
	}

	public void RoomAlert ()
	{
		RS = Room_State.Alerted;
		for (int i = 0; i < Enemies.Count; i++) {
			//Enemies [i].GetComponent<Basic_Enemy> ().PlayerofInterest = Player;
			if (Enemies [i] != null) {
				Enemies [i].GetComponent<Basic_Enemy> ().DetectedPlayer ();
			}
		}
	}

	public IEnumerator ResetRoom(){
		//Debug.Log ("Called reset room");
		//PM.RemoveEnemies ();
		RS = Room_State.Discovered;
		if (Enemies.Count == 1) {
			if (Enemies [0] != null) {
				StartCoroutine (Enemies [0].GetComponent<Basic_Enemy> ().ResetGaurd (1.0f));
			} else {
				Enemies = new List<GameObject> ();
			}
		} else {
			for (int i = 0; i < Enemies.Count; i++) {
				//Debug.Log ("Calling enemy " + Enemies[i].gameObject.name);
				Debug.Log ("Called reset room enemy [" + i + "]");
				//Enemies [i].GetComponent<Basic_Enemy> ().PlayerofInterest = Player;
				StartCoroutine (Enemies [i].GetComponent<Basic_Enemy> ().ResetGaurd (1.0f));
			}
		}
		//Debug.Log ("Reser done");
		yield return new WaitForSeconds (0.5f);
	}

	public void RemoveEnemy(GameObject DeadEnemy){
		Debug.Log("Removing enemy: " + DeadEnemy);
		Enemies.Remove (DeadEnemy);
	}

}
