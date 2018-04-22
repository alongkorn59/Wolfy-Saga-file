using UnityEngine;
using System.Collections;

public class HazardHelper : MonoBehaviour {

	//This is our baby! Our little hazard!
	public GameObject myHazard;
	
	//We'll use those vectors to store our old and new positions
	private Vector3 HazardOriginalPosition;
	private Vector3 HazardNewPosition;
	
	//This will control the actual movement on the update function
	private bool Appearing;
	private bool Disappearing;
	//We need this bool to make sure we don't trigger twice before resetting
	private bool Busy = false;
	
	//How high should I go?
	public float NewHazardY=1f;

	//How long after a player has passed by should we trigger to go up?
	public float TimeToTrigger=1f;
	//How fast should we go up?
	public float RaiseSpeed=0.2f;
	//How much time until we go down?
	public float TimeToGoDown=6f;
	//How fast should we go down?
	public float GoDownSpeed=0.2f;
	
	//How much time until we come back?
	public float TimeToTriggerIndependentOfPlayer=3f;
	//If we want this to be appearing and disappearing independent of player contact.
	public bool PlayerIndependent = false;
	
	//It's the start of the game!
	void Start() {
		//If you don't have a hazard for this, it's not going to work
		if (!myHazard) {
			Debug.Log("No, you need a hazard, seriously, this won't work without one!");
			return;
		}
		//We need to store our hazard positions when it's down and when its up
		HazardOriginalPosition=myHazard.transform.position;
		HazardNewPosition=HazardOriginalPosition;
		HazardNewPosition.y=NewHazardY;
		//If we're player independent...
        if(PlayerIndependent) {
        	//We should start counting to trigger
        	StartCoroutine(PlayerIndependentTriggering());
        }
	}
	
	//This is called every frame
	void Update() {
		//If we are not appearing nor disappearing, stop here.
		if(!Appearing&&!Disappearing) {
			return;
		}
		//If I do have a Hazard...
		if(myHazard) {
			//If I'm appearing
			if(Appearing) {
				//We'll do a Lerp to find out the next Y position for us to move smoothly
				float currentY = Mathf.Lerp(myHazard.transform.position.y,NewHazardY,RaiseSpeed);
				//We'll create a new vector3 for our hazard based on the current Y we found.
				Vector3 NewPosition = new Vector3 (myHazard.transform.position.x,currentY,myHazard.transform.position.z);
				//We'll set the hazard's transform to that new position we found
				myHazard.transform.position=NewPosition;
			}
			//If I'm disappearing
			if(Disappearing) {
				//We'll do a Lerp to find out the next Y position for us to move smoothly
				float currentY = Mathf.Lerp(myHazard.transform.position.y,HazardOriginalPosition.y,GoDownSpeed);
				//We'll create a new vector3 for our hazard based on the current Y we found.
				Vector3 NewPosition = new Vector3 (myHazard.transform.position.x,currentY,myHazard.transform.position.z);
				//We'll set the hazard's transform to that new position we found
				myHazard.transform.position=NewPosition;
			}
		}
	}
	
	//This coroutine will wait to trigger then bring the hazard up
	IEnumerator DoTheHazardBringing()
    {
    	yield return StartCoroutine(Wait(TimeToTrigger));
    	Appearing=true;
        //If the developer wants it to come back (TimeToComeBack bigger than 0...)
        if(TimeToGoDown>0) {
        	//Let's start our ComeBack Routine
        	StartCoroutine(ComeBack());
        }
        yield return StartCoroutine(Wait(RaiseSpeed*2f));
        Appearing=false;
    }
    
    //This one is in case you want this to go on and off constantly without player interaction
    IEnumerator PlayerIndependentTriggering()
    {
    	//It'll wait for the Time To Disappear time, than call the Hazard bringing coroutine.
    	yield return StartCoroutine(Wait(TimeToTriggerIndependentOfPlayer));
    	yield return StartCoroutine(DoTheHazardBringing());
    }
    
    //Come back routine! Like the appearing one but opposite!
    IEnumerator ComeBack()
    {
    	//It'll wait for the Time To ComeBack time, then start to disappear.
    	yield return StartCoroutine(Wait(TimeToGoDown));
    	Disappearing=true;
        //If we're player independent...
        if(PlayerIndependent) {
        	//We should start counting to appear again.
        	StartCoroutine(PlayerIndependentTriggering());
        }
        yield return StartCoroutine(Wait(GoDownSpeed*2f));
        Disappearing=false;
        //We should know we're not busy anymore, we can trigger again :D
        Busy=false;
    }

	//This is a fancy wait coroutine for the others to use.
	IEnumerator Wait(float duration)
    {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }

	//We should trigger if the player enters our trigger
	void OnTriggerEnter(Collider other) {
		//If we're player independent, just stop this D:
		if(PlayerIndependent||Busy) {
        	return;
        }
        if(other.gameObject.tag=="Player") {
        	Busy=true;
        	StartCoroutine(DoTheHazardBringing());
        }
    }
}