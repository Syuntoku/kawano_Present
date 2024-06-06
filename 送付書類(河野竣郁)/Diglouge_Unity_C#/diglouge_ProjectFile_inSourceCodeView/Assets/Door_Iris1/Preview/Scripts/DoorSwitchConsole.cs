using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is a slightly fancier-looking console.
 */

public class DoorSwitchConsole : MonoBehaviour {

	public GameObject targetDoor;

	IrisEventScript targetScript;
	Animator targetAnimator;

	enum ConsoleText : int
	{
		opened = 0,
        opening = 1,
		closed = 2,
        closing = 3,
        stopped = 4,
	};

    ConsoleText state = 0;
    bool stopped = false;
    Vector2 uvOpened;
    Vector2 uvOpening;
    Vector2 uvClosed;
    Vector2 uvClosing;
    Vector2 uvStopped;
    Vector2 uvBlank;

	void Start () {
		targetScript = targetDoor.GetComponent<IrisEventScript>();
		targetAnimator = targetDoor.GetComponent<Animator>();

		targetScript.onDoorClosingStarted += DoorClosing;
		targetScript.onDoorOpeningStarted += DoorOpening;
		targetScript.onDoorClosed += DoorClosed;
		targetScript.onDoorOpened += DoorOpened;
        targetScript.onDoorPaused += DoorStopped;
        targetScript.onDoorUnpaused += DoorStarted;

        /* Texture has 6 images in two rows of 3:
         * | CLOSING | STOPPED | BLANK  |
         * | OPEN    | OPENING | CLOSED |
         * It starts at blank */
        uvOpened    = new Vector2(-0.666f, -0.5f);
        uvOpening   = new Vector2(-0.333f, -0.5f);
        uvClosed    = new Vector2(0.0f, -0.5f);
        uvClosing   = new Vector2(-0.666f, 0.0f);
        uvStopped   = new Vector2(-0.333f, 0.0f);
        uvBlank     = new Vector2(0.0f, 0.0f);
    }

	public void ToggleDoor() // Activated by a message from the Activator script
	{
		targetScript.SignalToggle();
	}

    void SetConsoleUV(Vector2 uv)
    {
        if (!stopped)
        {
            GetComponent<Renderer>().materials[0].SetTextureOffset("_MainTex", uv);
        }
    }

    void SetConsoleText(ConsoleText text)
    {
        switch (text)
        {
            case ConsoleText.opened:
                SetConsoleUV(uvOpened);
                state = ConsoleText.opened;
                break;
            case ConsoleText.opening:
                SetConsoleUV(uvOpening);
                state = ConsoleText.opening;
                break;
            case ConsoleText.closed:
                SetConsoleUV(uvClosed);
                state = ConsoleText.closed;
                break;
            case ConsoleText.closing:
                SetConsoleUV(uvClosing);
                state = ConsoleText.closing;
                break;
            case ConsoleText.stopped:
                SetConsoleUV(uvStopped);
                break;
        }
    }

    // Event handlers that respond to different events from the door
    void DoorClosed(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
        SetConsoleText(ConsoleText.closed);
	}

    void DoorClosing(object sender, IrisEventScript.AnimationEventArgs eventArgs)
    {
        SetConsoleText(ConsoleText.closing);
    }

    void DoorOpened(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
        SetConsoleText(ConsoleText.opened);
    }

    void DoorOpening(object sender, IrisEventScript.AnimationEventArgs eventArgs)
    {
        SetConsoleText(ConsoleText.opening);
    }

    void DoorStopped(object sender, IrisEventScript.AnimationEventArgs eventArgs)
    {
        SetConsoleText(ConsoleText.stopped);
        stopped = true;
    }

    void DoorStarted(object sender, IrisEventScript.AnimationEventArgs eventArgs)
    {
        stopped = false;
        SetConsoleText(state);
    }
}