using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Controller : MonoBehaviour {

	public List<GameObject> Enemies = new List<GameObject> ();
	public GameObject[] Cameras;

	public GameObject Player;
	public PlayerMovement PM;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Something just Collided");
		if (other.tag == "Player") {
			Player = other.gameObject;
			PM = Player.GetComponent<PlayerMovement> ();
			PM.getEnemies (Enemies);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			PM.RemoveEnemies ();
			StartCoroutine(ResetRoom ());
		}
	}

	public void RoomAlert(){
		for (int i = 0; i < Enemies.Count; i--) {
			//Enemies [i].GetComponent<Basic_Enemy> ().PlayerofInterest = Player;
			Enemies [i].GetComponent<Basic_Enemy> ().DetectedPlayer ();
		}
	}

	public IEnumerator ResetRoom(){
		Debug.Log ("Called reset room");
		//PM.RemoveEnemies ();
		for (int i = 0; i > Enemies.Count; i++) {
			//Debug.Log ("Calling enemy " + Enemies[i].gameObject.name);
			Debug.Log ("Called reset room enemy [" + i + "]");
			//Enemies [i].GetComponent<Basic_Enemy> ().PlayerofInterest = Player;
			StartCoroutine(Enemies [i].GetComponent<Basic_Enemy> ().ResetGaurd(1.0f));
		}
		yield return new WaitForSeconds (0.5f);
	}
}
