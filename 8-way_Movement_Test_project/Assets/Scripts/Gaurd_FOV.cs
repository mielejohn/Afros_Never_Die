using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaurd_FOV : MonoBehaviour {

	public Basic_Enemy BE;
	void Start () {
		
	}

	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" && BE.Player.player_state == Player_States.Walking) {
			BE.PlayerofInterest = other.gameObject;
			BE.RC.RoomAlert ();
			BE.DetectedPlayer ();
		} if (other.tag == "Player" && BE.Player.player_state == Player_States.Crouching) {
			BE.detectingPlayer = true;
			//BE.ES = Enemy_States.Investigating;
			StartCoroutine (Detecting ());
		}
	}

	private IEnumerator Detecting(){
		while (BE.detectingPlayer == true) {
			if (BE.playerDetectedPercent < 99) {
				BE.playerDetectedPercent += 2;
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
			if (BE.playerDetectedPercent > 50) {
				BE.detectingPlayer = false;
				BE.ES = Enemy_States.Investigating;
				BE.SearchForPlayer (other.transform.position); 
			} else if(BE.ES == Enemy_States.Attacking) {
				BE.detectingPlayer = false;
				StartCoroutine(BE.ResetGaurd (4.0f));
			}
			if (BE.ES == Enemy_States.Attacking) {
				BE.SearchForPlayer (other.transform.position);
			}
		}
	}
}
