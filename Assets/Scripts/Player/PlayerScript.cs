using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private float speed = 8.0f;
	private float maxVelocity = 4.0f;

	private Rigidbody2D body;
	private Animator anim;

	[SerializeField]
	private GameObject[] arrows;

	private float height;
	private bool shootOnce, shootTwice;
	private bool canWalk;

	[SerializeField]
	private AudioClip shootClip;

	void Awake(){
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		height = -Camera.main.orthographicSize - 0.5f;
		canWalk = true;
		shootOnce = true;
		shootTwice = true;
	}

	void Start () {
	
	}

	void FixedUpdate () {
		MovementKeyboard ();
	}

	void Update(){
		ShootArrow ();
	}

	public void SetShootOnce(){
		shootOnce = true;
	}

	public void SetShootTwice(){
		shootTwice = true;
	}

	public void ShootArrow(){
		if(Input.GetMouseButtonDown(0)){
			if (shootOnce) {
				shootOnce = false;
				StartCoroutine (PlayShootAnimation ());
				Instantiate (arrows [2], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
			} else if (shootTwice) {
				shootTwice = false;
				StartCoroutine (PlayShootAnimation ());
				Instantiate (arrows [1], new Vector3 (transform.position.x, height, 0), Quaternion.identity);
			}

		}
	}

	IEnumerator PlayShootAnimation(){
		canWalk = false;
		anim.SetBool ("Shoot", true);
		string clipName = name + " Shoot";
		anim.Play (clipName);
		AudioSource.PlayClipAtPoint (shootClip, transform.position);
		yield return new WaitForSeconds (0.15f);
		anim.SetBool ("Shoot", false);
		canWalk = true;
	}

	void MovementKeyboard(){
		if(canWalk){
			float force = 0.0f;
			float velocity = Mathf.Abs (body.velocity.x);

			float h = Input.GetAxis ("Horizontal");

			if(h > 0){
				if(velocity < maxVelocity){
					force = speed;
				}
				Vector3 scale = transform.localScale;
				scale.x = 1.0f;
				transform.localScale = scale;
			} else if (h < 0){
				if(velocity < maxVelocity){
					force = -speed;
				}
				Vector3 scale = transform.localScale;
				scale.x = -1.0f;
				transform.localScale = scale;
			}

			body.AddForce (new Vector2 (force, 0));

			anim.SetBool ("Walk", velocity > 0);
		}
	}
}
