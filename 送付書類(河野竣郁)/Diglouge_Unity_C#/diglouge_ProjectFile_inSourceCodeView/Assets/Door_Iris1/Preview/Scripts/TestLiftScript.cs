using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/*
 * This is a little lift that integrates with the door as a demonstration for a possible way to use its
 * scripting system.
 * 
 * The lift starts at the top level and when Used by the player with the E button will start descending.
 * When it is low enough, the lift will trigger the door to close. The lift descends to the bottom of the
 * shaft, stops briefly and then starts rising again. When it reaches the close trigger point it will stop
 * and open the door, and when the door opens will return to its starting position.
 * 
 * Unfortunately, this scene has nothing that actually holds the player object to the lift, so the
 * experience of riding the lift is extremely jittery and can involve falling through the lift. I hope
 * it makes sense as an example of the event system, though.
 */

public class TestLiftScript : MonoBehaviour
{
    const float clearance = 2.0f; // the distance to leave between the elevator and the closing/opening door
    public float dropDistance; // how far the elevator should descend in total
    public GameObject associatedDoor; // the door this lift interacts with
    IrisEventScript associatedScript;
    Vector3 restPosition; // the point where the door starts moving from
    Vector3 startPosition; // the starting point of the current movement
    Vector3 endPosition; // the endpoint of the current movement
    delegate void MovementHandler(); // the type of... v
    MovementHandler movementHandler; // the function currently handling the door's movement
    float startTime; // the time the current movement state started
    readonly float descentLength = 6.0f; // the length of time a descent segment takes, in seconds

    // Start is called before the first frame update
    void Start()
    {
        restPosition = transform.position;
        endPosition = startPosition - new Vector3(0.0f, -dropDistance, 0.0f);
        movementHandler = null;
        associatedScript = associatedDoor.GetComponent<IrisEventScript>();
        Assert.IsNotNull(associatedScript);
    }

    // Update is called once per frame
    void Update()
    {
        movementHandler?.Invoke();
    }

    void FixedUpdate()
    {
    }

    void ToggleDoor()
    {
        if (movementHandler == null) // only activate if we're in the rest position
        {
            movementHandler = InitialDescent;
            startTime = Time.time;
            startPosition = restPosition;
            endPosition = startPosition; endPosition.y -= clearance;
        }
    }

    void InitialDescent() // move to the first stop point, just under the clearance
    {
        if (!LerpStartToEnd())
        {
            movementHandler = DoNothing; // switch movement handler
            associatedScript.onDoorClosed += PauseWhileClosingEvent; // register an event to respond
            associatedScript.SignalClose(); // ask the door to close
        }
    }

    void DoNothing() // empty handler while closing
    {

    }

    void PauseWhileClosingEvent(object sender, IrisEventScript.AnimationEventArgs eventArgs)
    {
        // This event fires when the door finishes closing while the lift has descended.
        movementHandler = SecondDescent;
        associatedScript.onDoorClosed -= PauseWhileClosingEvent;
        startPosition = transform.position;
        endPosition = new Vector3(restPosition.x, restPosition.y - dropDistance, restPosition.z);
        startTime = Time.time;
    }

    void SecondDescent() // movement handler for descending to the lowest level
    {
        if (!LerpStartToEnd())
        {
            movementHandler = InitialAscent;
            startPosition = endPosition;
            endPosition = new Vector3(restPosition.x, restPosition.y - clearance, restPosition.z);
            startTime = Time.time;
        }
    }

    void InitialAscent() // movement handler for returning to the door opening point
    {
        if (!LerpStartToEnd())
        {
            movementHandler = DoNothing;
            associatedScript.onDoorOpened += PauseWhileOpeningEvent;
            associatedScript.SignalOpen();
        }
    }

    // Event for pausing while the door opens before the final ascent
    void PauseWhileOpeningEvent(object sender, IrisEventScript.AnimationEventArgs eventArgs)
    {
        movementHandler = SecondAscent;
        startPosition = transform.position;
        endPosition = restPosition;
        startTime = Time.time;
        associatedScript.onDoorOpened -= PauseWhileOpeningEvent;
    }

    void SecondAscent() // movement handler for rising back to the starting point
    {
        if (!LerpStartToEnd())
        {
            movementHandler = null;
        }
    }

    bool LerpStartToEnd() // Lerp between start and end points. If t > 1.0 then returns false
    {
        float timeFraction = (Time.time - startTime) / descentLength;

        if (timeFraction > 1.0f)
        {
            transform.position = endPosition;
            return false;
        }
        else
        {
            transform.position = new Vector3(startPosition.x,
                  Mathf.Lerp(startPosition.y, endPosition.y, timeFraction),
                  startPosition.z);
            return true;
        }
    }
}
