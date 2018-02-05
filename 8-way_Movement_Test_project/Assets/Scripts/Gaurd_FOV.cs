using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaurd_FOV : MonoBehaviour {

	public Basic_Enemy BE;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("OntriggerEnter just went off");
		if (other.tag == "Player" && BE.Player.player_state == Player_States.Walking) {
			Debug.Log ("Colliding with a walking player");
			BE.DetectedPlayer ();
		} if (other.tag == "Player" && BE.Player.player_state == Player_States.Crouching) {
			Debug.Log ("Colliding with a crouching player");
			BE.detectingPlayer = true;
			StartCoroutine (Detecting ());
		}
	}

	private IEnumerator Detecting(){
		while (BE.detectingPlayer == true) {
			if (BE.playerDetectedPercent < 99) {
				BE.playerDetectedPercent += 5;
				yield return new WaitForSeconds (0.1f);
				//StartCoroutine (Detecting ());
			} else {
				BE.DetectedPlayer();
				BE.playerDetectedPercent = 0;
				BE.detectingPlayer = false;
			}

			if (BE.detectingPlayer == true && BE.Player.player_state == Player_States.Walking) {
				BE.DetectedPlayer();
				BE.playerDetectedPercent = 0;
				BE.detectingPlayer = false;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			BE.detectingPlayer = false;
			BE.ResetGaurd ();
		}
	}
}
