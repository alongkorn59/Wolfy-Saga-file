using UnityEngine;
using System.Collections;

public class PlayerLadderClimb: MonoBehaviour {

	private PlayerMove playerMove;
	private CharacterMotor characterMotor;
	private Rigidbody playerRB;
	public float ClimbingSpeed=10;
	
	private bool ClimbingLadder;
	private bool CanClimbLadder;
	
	void Start () {
		//Let's initialize our stuff, and store some info.
		playerMove = GetComponent<PlayerMove>();
		characterMotor = GetComponent<CharacterMotor>();
		playerRB = GetComponent<Rigidbody>();
		ClimbingLadder=false;
		CanClimbLadder=true;
	}
		
	// Update is called once per frame
	void Update () {
		//If we're not currently climbing, we won't do anything, but if we are...
		if(ClimbingLadder) {
			//I'll get the vertical axis (Front and back
			float v = Input.GetAxisRaw ("Vertical");
			//I'll create a new vector 3 with the Y being the current vertical axis * climbing speed.
			//So if I'm not inputing anything, the Y will be 0 (Climbing speed times 0 is 0).
			//If i'm inputing positive, it'll be a positive Y, if negative, a negative Y.
			Vector3 speed = new Vector3(0,v*ClimbingSpeed,0);
			//With this new vector3, we'll do a rigid body move position,  times deltatime (current time)
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + speed * Time.deltaTime);
			
			//If we press the jump button we probably don't want to be in the ladder anymore so let's call our stop climbing function
			if (Input.GetButtonDown ("Jump")) {
				StopClimbingLadder();
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		//If I can't climb (just stopped climbing) let's not do anything!
		if(!CanClimbLadder){
			return;
		}
		//If the object has the tag Ladder and I can Climb...
		if(other.gameObject.tag=="Ladder"&&CanClimbLadder) {
			//Set my Climbing Ladder to true
			ClimbingLadder=true;
			//Disable my Player Move, let me handle the input.
			playerMove.enabled=false;
			//Set the RigidBody to Kinematic, I GOT THIS BRO!
			playerRB.isKinematic=true;
			//Set the Bool Climbing on the animnator to true
			//So Set up your climbing animation to use that bool.
			playerMove.animator.SetBool ("Climbing", true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		//If a object with the gametag ladder exited my Trigger...
		if(other.gameObject.tag=="Ladder") {
			//Stop climbing! let's get control back!
			StopClimbingLadder();
		}
	}
	
	void StopClimbingLadder() {
		//Let's set the can climb, climbing to false So we're not climbing and we can't climb
		CanClimbLadder=false;
		ClimbingLadder=false;
		//Let's turn on our player movement and uncheck rigidbody kinematic
		//So that gravity will work again on us and we'll be able to control ourselves.
		playerMove.enabled=true;
		playerRB.isKinematic=false;
		//Let's turn off our climbing animation
		playerMove.animator.SetBool ("Climbing", false);
		//And in a couple of F time we'll want to climb agian so we'll start this coroutine
		StartCoroutine(CanClimbAgain());
	}
	
	IEnumerator CanClimbAgain()
    {
    	//Let's wait a delay amount of time to start.
    	yield return StartCoroutine(Wait(0.5f));
    	//Now we can climb again! :D
    	CanClimbLadder=true;
    }
    
    	
	//This is a fancy wait coroutine for the others to use.
	IEnumerator Wait(float duration)
    {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }
}