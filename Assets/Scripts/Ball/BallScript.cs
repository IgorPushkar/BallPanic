using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	private float forceX, forceY;
	private bool moveLeft = true;
	private Rigidbody2D body;

	[SerializeField]
	private GameObject originalBall;

	private GameObject ball1, ball2;

	private BallScript ball1Script, ball2Script;

	[SerializeField]
	private AudioClip[] clip;

	void Awake(){
		body = GetComponent<Rigidbody2D> ();
		SetBallSpeed ();
		InstantiateBalls ();

	}

	void Start () {
	
	}

	void Update () {
		MoveBall ();
	}

	void InstantiateBalls(){
		if(this.gameObject.tag != "SmallestBall"){
			ball1 = Instantiate (originalBall);
			ball2 = Instantiate (originalBall);

			ball1Script = ball1.GetComponent<BallScript> ();
			ball2Script = ball2.GetComponent<BallScript> ();

			ball1.SetActive (false);
			ball2.SetActive (false);
		}
	}

	public void SetMoveLeft(bool flag){
		moveLeft = flag;
	}

	void MoveBall(){
		Vector3 temp = transform.position;
		temp.x += moveLeft ? -(forceX * Time.deltaTime) : (forceX * Time.deltaTime);
		transform.position = temp;
	}

	void InitNewBalls(){
		Vector3 position = transform.position;

		ball1.transform.position = position;
		ball1Script.SetMoveLeft(true);
		ball1.SetActive (true);

		ball2.transform.position = position;
		ball2Script.SetMoveLeft(false);
		ball2.SetActive (true);

		if(this.gameObject.tag != "SmallestBall"){
			if(transform.position.y > 1 && transform.position.y < 1.3f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
			} else if(transform.position.y > 1.3f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
			} else if (transform.position.y > 1f){
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
			}
		}
	}

	void SetBallSpeed(){
		forceX = 2.5f;
		switch(gameObject.tag){
		case "LargestBall":
			forceY = 11.5f;
			break;
		case "LargeBall":
			forceY = 10.5f;
			break;
		case "MediumBall":
			forceY = 9f;
			break;
		case "SmallBall":
			forceY = 8f;
			break;
		case "SmallestBall":
			forceY = 7f;
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "FirstArrow" || other.tag == "SecondArrow" || other.tag == "FirstStickyArrow" || other.tag == "SecondStickyArrow"){
			if(gameObject.tag != "SmallestBall"){
				InitNewBalls ();
			}
			AudioSource.PlayClipAtPoint (clip[Random.Range(0,clip.Length)], transform.position);
			gameObject.SetActive (false);
		}

		if(other.tag == "UnbreakableBrickTop" || other.tag == "BreakableBrickTop" || other.tag == "UnbreakableBrickTopVertical"){
			body.velocity = new Vector2 (0, 5);
		} else if(other.tag == "UnbreakableBrickBottom" || other.tag == "BreakableBrickBottom" || other.tag == "UnbreakableBrickBottomVertical"){
			body.velocity = new Vector2 (0, -2);
		} else if(other.tag == "UnbreakableBrickLeft" || other.tag == "BreakableBrickLeft" || other.tag == "UnbreakableBrickLeftVertical"){
			moveLeft = false;
		} else if(other.tag == "UnbreakableBrickRight" || other.tag == "BreakableBrickRight" || other.tag == "UnbreakableBrickRightVertical"){
			moveLeft = true;
		}

		if(other.tag == "BottomBrick"){
			body.velocity = new Vector2 (0, forceY);
		}
		if(other.tag == "LeftBrick"){
			moveLeft = false;
		}
		if(other.tag == "RightBrick"){
			moveLeft = true;
		}
	}
}
