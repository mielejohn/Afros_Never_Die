using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Controller : MonoBehaviour {

	public GameObject[] Enemies;
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
		Debug.Log ("Something just Collided");
		if (other.tag == "Player") {
			Player = other.gameObject;
			PM = Player.GetComponent<PlayerMovement> ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			ResetRoom ();
		}
	}

	public void RoomAlert(){
		for (int i = 0; i < Enemies.Length; i++) {
			//Enemies [i].GetComponent<Basic_Enemy> ().PlayerofInterest = Player;
			Enemies [i].GetComponent<Basic_Enemy> ().DetectedPlayer ();
		}
	}

	public void ResetRoom(){
		Debug.Log ("Called reset room");
		for (int i = 0; i < Enemies.Length; i++) {
			Debug.Log ("Calling enemy " + Enemies[i].gameObject.name);
			Debug.Log ("Called reset room enemy [" + i + "]");
			//Enemies [i].GetComponent<Basic_Enemy> ().PlayerofInterest = Player;
			StartCoroutine(Enemies [i].GetComponent<Basic_Enemy> ().ResetGaurd(1.0f));
		}
	}
}
