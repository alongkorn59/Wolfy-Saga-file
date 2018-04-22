using UnityEngine;
using System.Collections;

public class PlayerRanged : MonoBehaviour {

	//We'll use those 2 to communicate with the rest of the kit.
	private PlayerMove playerMove;
	private CharacterMotor characterMotor;

	//This one will be our projectile object, it should have a ScriptProjectile in it.
	//If you don't set one, I'll just create a sphere for you and use it, it'll be ugly! D:
	public GameObject ProjectileObject;
	//You can set a Transform here for the object to spawn from, if none is set I'll pick a
	//Position in front of the player so no worries.
	public Transform SpawnPosition;
	//How much time should we wait before firing? We should wait for the animation probably.
	public float WaitBeforeFiring =0.2f;
	//How much cooldown should we have before firing again?
	public float CooldownTime =1f;
	
	//I'll keep my last fired projectile on this variable for setting up purposes...
	private GameObject LastFiredProjectile;
	
	private bool CanShoot;
	// Use this for initialization
	void Start () {
		//We're supposed to be on the same gameobject as the PlayerMove...
		//I'll need to talk with the animator so I'll just grab a reference from him.
		playerMove = GetComponent<PlayerMove>();
		//We're not on cooldown!
		CanShoot=true;
	}
	
	//Runs every frame
	void Update () {
		//If the player pressed the X key and he is not on cooldown...
		if (Input.GetKeyDown(KeyCode.X)&&CanShoot) {
			//First off, let's check if we're not already playing our ArmsThrow animation.
			if(!CheckIfPlaying("ArmsThrow",1)) {
					//We're not playing it! So let's play it :D
					playerMove.animator.Play("ArmsThrow",1);
					//Now start a coroutine that will wait before firing so the animation plays a bit before spawning projectiles.
					StartCoroutine(WaitAndFire(WaitBeforeFiring));
					//Let's start our cooldown!
					CanShoot=false;
					StartCoroutine(CoolDown(CooldownTime));
			}
		}
	}

	//This is an enumerator, it can stop and wait before resuming it's function.	
	IEnumerator WaitAndFire(float waitTime)
    {
    	//It'll wait for as much time as the float waitTime inputted.
        yield return new WaitForSeconds(waitTime);
        //First lets create a new vector 3 and zero it out.
        Vector3 spawnPos = Vector3.zero;
        
        //If the player hasn't set a spawn position, let's make one for him in front of our character and kind of half his height
        if(!SpawnPosition) {
        	spawnPos = gameObject.transform.position + gameObject.transform.forward + Vector3.up*0.45f;
        } else {
        	//If the player did set up a spawn position, just use it :D
        	spawnPos = SpawnPosition.position;
        }
        
        //If the plaer was too lazy to create a Projectile Object let's make one for him and position it.
        if(!ProjectileObject) {
			LastFiredProjectile = CreateProjectile();
			LastFiredProjectile.transform.rotation=gameObject.transform.rotation;
			LastFiredProjectile.transform.position=spawnPos;
		} else {
			//If the player did create a Projectile Object lets instantiate it on our spawn position and rotate it propertly
			LastFiredProjectile = GameObject.Instantiate(ProjectileObject,spawnPos,gameObject.transform.rotation) as GameObject;
		}
		//Lets initialize our newly spawned Projectile!
		LastFiredProjectile.GetComponent<ScriptProjectile>().Initialize();
    }
    
    //We'll use this as a cooldown so we can't spam the projectiles D:
	IEnumerator CoolDown(float waitTime)
    {
    	//It'll wait for as much time as the float waitTime inputted.
        yield return new WaitForSeconds(waitTime);
        //We can shoot now! The cooldown is over!
        CanShoot=true;
    }
    
	//This function will create a projectile for the lazy developer that didn't add one
	//and add all the necessary stuff to it, then it'll return it to the funciton that called it.
	GameObject CreateProjectile() {
		//Creates a Sphere
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//Adds our Projectile script! Wouldn't be a projectile without it xD!
		go.AddComponent<ScriptProjectile>();
		//And a collider! It's important!
		go.AddComponent<SphereCollider>();
		//Also it needs a rigid body to move D: so lets add one
		Rigidbody gobody = go.AddComponent<Rigidbody>();
		//Now we'll make the rigid body into a Kinematic one
		gobody.isKinematic=true;
		//And let's deactivate gravity on it.
		gobody.useGravity=false;
		//Also, the collider we added... it should be a trigger :O
		go.GetComponent<Collider>().isTrigger = true;
				
		//Now return the created game object
		return go;
	}
	
	bool CheckIfPlaying(string Anim,int Layer) {
		//Grabs the AnimatorStateInfo out of our PlayerMove animator for the desired Layer.
		AnimatorStateInfo AnimInfo = playerMove.animator.GetCurrentAnimatorStateInfo(Layer);
		//Returns the bool we want, by checking if the string ANIM given is playing.
		return AnimInfo.IsName(Anim);
	}
}