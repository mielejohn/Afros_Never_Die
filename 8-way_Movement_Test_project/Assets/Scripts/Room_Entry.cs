using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_Entry : MonoBehaviour {

	public GameObject Room_cover;
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
			Room_cover.SetActive (false);
		}
	}
}
