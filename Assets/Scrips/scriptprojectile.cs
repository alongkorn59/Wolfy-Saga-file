using UnityEngine;
using System.Collections;

public class ScriptProjectile : MonoBehaviour {

	//Information about our projectile... how much damage? How much should it push
	//the victim on height? how much force should add on the push? how fast should it go?
	public int ProjDamage = 1;
	public float ProjHeight = 2f;
	public float ProjForce =10f;
	public float ProjSpeed=15f;
	
	//This time is to make sure it won't be running forever if it doesn't hit anything.
	//If you want to limit the range of the projectile set this to a lower value.
	public float selfDestructTime=3f;
	
	//It needs a DealDamage script, so we'll store one here.
	private DealDamage DoDamage;
	//We'll use this bool to check if we have been initiated by the player.
	private bool Initiated=false;

    public GameObject Owner { get; internal set; }

    void Start() {
		//Create a Deal Damage Component for us.
		DoDamage = gameObject.AddComponent<DealDamage>();
	}

	//This function will be called from the player to start our projectile.
	public void Initialize() {
		Initiated=true;
		//This will start our countdown to self destruction
		StartCoroutine(selfDestruct(selfDestructTime));
	}
	
	void FixedUpdate () {
		//If it has been initiated by the player
		if(Initiated) {
			//Tell the Rigid Body to move forward at the desired speed
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().transform.position+(transform.forward*ProjSpeed*Time.deltaTime));
		}
	}
	
	//This function runs for each collider on our trigger zone, on each frame they are on our trigger zone.
	void OnTriggerEnter(Collider other) {
		//If it hasn't been initiated by the player, just stop here.
		if(!Initiated) {
			return;
		}
		//If the object colliding doesn't have the tag player and is not a trigger...
		if(other.gameObject.tag!="Player" && !other.isTrigger) {
			//If it's a rigid body, tell our DealDamage to attack it!
	        if (other.attachedRigidbody) {
	           	DoDamage.Attack(other.gameObject,ProjDamage,ProjHeight,ProjForce);
	        }
	        //If it isn't we still probably want to destroy our projectile since it has a collider, so destroy it wether it is a rigid body or not.
	        Destroy(this.gameObject);
        }
    }
    
    //Coroutine to wait for the set amount of seconds and destroy itself.
    IEnumerator selfDestruct(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Destroy(this.gameObject);
	}
}