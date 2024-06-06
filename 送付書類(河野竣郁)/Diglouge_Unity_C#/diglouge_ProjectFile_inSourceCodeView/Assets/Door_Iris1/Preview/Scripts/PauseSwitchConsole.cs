using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSwitchConsole : MonoBehaviour {

	//public bool enablePausedState;
	public GameObject targetDoor;

	IrisEventScript targetScript;
	Animator targetAnimator;
    SkinnedMeshRenderer switchRenderer;
    
	// Use this for initialization
	void Start () {
		targetScript = targetDoor.GetComponent<IrisEventScript>();
		targetAnimator = targetDoor.GetComponent<Animator>();
        switchRenderer = GetComponent<SkinnedMeshRenderer>();

		targetScript.onDoorPaused += DoorPaused;
		targetScript.onDoorUnpaused += DoorUnpaused;

        switchRenderer.SetBlendShapeWeight(0, targetScript.paused ? 100f : 0f);
	}

	// Update is called once per frame
	void Update () {

	}

	public void ToggleDoor()
	{
		targetScript.SignalTogglePause();
	}

	void DoorPaused(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
        switchRenderer.SetBlendShapeWeight(0, 100f);
    }

	void DoorUnpaused(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
        switchRenderer.SetBlendShapeWeight(0, 0f);
	}
}
