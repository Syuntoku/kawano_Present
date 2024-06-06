using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is very similar to DoorSwitch except it pauses the door instead of toggling it.
 */

public class PauseSwitch : MonoBehaviour {

	//public bool enablePausedState;
	public GameObject targetDoor;
	public GameObject targetSwitch;

	IrisEventScript targetScript;
	Animator targetAnimator;
	Animator switchAnimator;

	enum SwitchState : int
	{
		open = 0,
		paused = 1,
		closed = 2
	};
    
	void Start () {
		targetScript = targetDoor.GetComponent<IrisEventScript>();
		targetAnimator = targetDoor.GetComponent<Animator>();
		switchAnimator = targetSwitch.GetComponent<Animator>();

		targetScript.onDoorPaused += DoorPaused;
		targetScript.onDoorUnpaused += DoorUnpaused;
        
		switchAnimator.SetInteger("State", targetScript.paused ? (int)SwitchState.closed : (int)SwitchState.open);
	}

	public void ToggleDoor()
	{
		targetScript.SignalTogglePause();
	}

	void DoorPaused(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
		switchAnimator.SetInteger("State", (int)SwitchState.closed); 
	}

	void DoorUnpaused(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
		switchAnimator.SetInteger("State", (int)SwitchState.open);
	}
}
