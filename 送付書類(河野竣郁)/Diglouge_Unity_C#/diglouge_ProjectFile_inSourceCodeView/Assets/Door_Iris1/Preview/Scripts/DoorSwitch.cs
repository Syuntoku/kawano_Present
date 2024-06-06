using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * DoorSwitch
 * 
 *  This script is messaged by the Activator script on the player and is responsible for sending signals to the
 * associated iris door and updating the switch's animator based on its responses. It uses the event system of
 * the iris door to update its own state, allowing it to be in the correct position even if a switch on the other
 * side of the door has been activated.
 * 
 *  It is included as an example of how the door might be used.
 */

public class DoorSwitch : MonoBehaviour {

	public GameObject targetDoor;   // the door to operate
	public GameObject targetSwitch; // the switch associated with this script, for some reason

	IrisEventScript targetScript;   // the IrisEventScript of the target door
	Animator targetAnimator;        // the animator of the target door
	Animator switchAnimator;        // the animator of the associated switch

	enum SwitchState : int // the switch has three states but we only use "open" and "closed"
	{
		open = 0,
		paused = 1,
		closed = 2
	};
    
	// Use this for initialization
	void Start () {
        // Set up references
		targetScript = targetDoor.GetComponent<IrisEventScript>();
		targetAnimator = targetDoor.GetComponent<Animator>();
		switchAnimator = targetSwitch.GetComponent<Animator>();

        // Register for events
		targetScript.onDoorClosingStarted += DoorClosed;
		targetScript.onDoorOpeningStarted += DoorOpened;
		targetScript.onDoorClosed += DoorClosed;
		targetScript.onDoorOpened += DoorOpened;

        // Set the animator's state
		bool doorIsOpening = targetAnimator.GetBool("isOpening");
		switchAnimator.SetInteger("State", doorIsOpening ? (int)SwitchState.open : (int)SwitchState.closed);
	}

	public void ToggleDoor()
	{
		targetScript.SignalToggle(); // ask the door to toggle whether it's open or closed
	}

    // These are event handlers that are invoked by the IrisEventScript
	void DoorClosed(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
		switchAnimator.SetInteger("State", (int)SwitchState.closed); 
	}

	void DoorOpened(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
		switchAnimator.SetInteger("State", (int)SwitchState.open);
	}
}
