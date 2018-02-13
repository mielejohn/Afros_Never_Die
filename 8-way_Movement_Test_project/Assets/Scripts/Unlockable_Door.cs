using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable_Door : MonoBehaviour {

	public GameObject Room_cover;
	public GameObject Room_lock;
	public bool Room_Discovered = false;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Colliding with something");
		if (Room_Discovered == false && other.tag == "Player") {
			//Debug.Log ("room Discovered");
			Room_Discovered = true;
			Room_lock.SetActive (false);
			Room_cover.SetActive (false);
		}
	}
}
