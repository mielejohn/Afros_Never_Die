using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[Space]
	[SerializeField]
	public Player_States player_state;
	[SerializeField]
	public Stealth_States stealth_State;

	void Start () {
		RB = this.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis ("Vertical");

		Vector2 movement = new Vector2 (moveX, moveY);
		RB.velocity = movement * speed;

		if (Input.GetButton ("L_Control")) {
			player_state = Player_States.Crouching;
			transform.localScale = new Vector3 (5.836665f,6,1);
			speed = 4;
		} else if (Input.GetButtonUp ("L_Control")) {
			player_state = Player_States.Walking;
			transform.localScale = new Vector3 (5.836665f,8.338114f,1);
			speed = 7;
		}
	}



	void Update () {
		/*if (Input.GetButton ("Horizontal")) {
			player_sprite.flipX = true;
			moveX = Input.GetAxis ("Horizontal") * speed;
			RB.AddForce (transform.right * moveX);
		} else {
			player_sprite.flipX = false;
			moveX = Mathf.Lerp (moveX, 0, LerpSpeed);
			RB.velocity = new Vector2 (moveX,moveY);
		}
		//RB.AddForce (transform.right * moveX);

		if (Input.GetButton ("Vertical")) {
			moveY = Input.GetAxis ("Vertical") * speed;
			RB.AddForce (transform.up * moveY);
		} else {
			moveY = Mathf.Lerp (moveY, 0, LerpSpeed);
			RB.velocity = new Vector2 (moveX,moveY);
		}
		//RB.AddForce (transform.up * moveY);

		//Translate_Movement (moveX, moveY);

		if (Input.GetButton ("L_Control")) {
			state = Player_States.Crouching;
			transform.localScale = new Vector3 (5.836665f,6,1);
			speed = 5;
		} else if (Input.GetButtonUp ("L_Control")) {
			state = Player_States.Walking;
			transform.localScale = new Vector3 (5.836665f,8.338114f,1);
			speed = 10;
		}
*/
	}
}
