using UnityEngine;
using System.Collections;

public class CameraTargetChanger : MonoBehaviour {

	CameraFollow cFollow;
	PlayerMove pMove;
	
	public Transform Target1;
	
	public Vector3 TargetOffset;
	
	public float TargetFollowSpeed;
	
	public bool OnlyTriggerOnce=true;

	public bool TargetIsPlayer=false;


	//We should trigger if the player enters our trigger
	void OnTriggerEnter(Collider other) {
		//If what entered our trigger has the TAG Player...
		if(other.gameObject.tag=="Player") {
			//Let's ask the Camera to give us their Camera Follow component to mess with!
			cFollow = Camera.main.GetComponent<CameraFollow>();
			cFollow.followSpeed=TargetFollowSpeed;
			if(TargetIsPlayer) {
				cFollow.target=other.gameObject.transform;
			} else {
	    		cFollow.target=Target1;
	    	}
	    	cFollow.targetOffset=TargetOffset;
	    	
			//Now, if we want to trigger this again, and not only once...
			if(OnlyTriggerOnce){
				gameObject.GetComponent<Collider>().enabled=false;
			}
		}
	}
}