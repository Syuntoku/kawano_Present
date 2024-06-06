using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisDoorOpened : StateMachineBehaviour {

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		IrisEventScript irisScript = animator.gameObject.GetComponent<IrisEventScript>();

		if (irisScript)
			irisScript.FireDoorOpenedEvent();
	}
}
