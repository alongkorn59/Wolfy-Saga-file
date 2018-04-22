using UnityEngine;
using System.Collections;

public class SpeedUpOnKeyPressed : MonoBehaviour {
	
	private PlayerMove playerMove;
	
	float OriginalSpeed;
	float OriginalAccel;
	float OriginalAirAccel;
	public float newMaxSpeed =15f;
	public float newMaxAccel=95f;
	public float newAirAccel=25f;
	// Use this for initialization
	void Start () {
		playerMove = GetComponent<PlayerMove>();
		OriginalSpeed=playerMove.maxSpeed;
		OriginalAccel=playerMove.accel;
		OriginalAirAccel=playerMove.airAccel;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)) {
			if(playerMove.maxSpeed!=newMaxSpeed) {
				playerMove.maxSpeed=newMaxSpeed;
			}
			if(playerMove.accel!=newMaxAccel) {
				playerMove.accel=newMaxAccel;
			}
			if(playerMove.airAccel!=newAirAccel) {
				playerMove.airAccel=newAirAccel;
			}
			playerMove.animator.SetBool ("SpeedUp", true);
		}
		if (Input.GetKeyUp(KeyCode.B)) {
			if(playerMove.maxSpeed!=OriginalSpeed) {
				playerMove.maxSpeed=OriginalSpeed;
			}
			if(playerMove.accel!=OriginalAccel) {
				playerMove.accel=OriginalAccel;
			}
			if(playerMove.airAccel!=OriginalAirAccel) {
				playerMove.airAccel=OriginalAirAccel;
			}
			playerMove.animator.SetBool ("SpeedUp", false);
		}
	}
}