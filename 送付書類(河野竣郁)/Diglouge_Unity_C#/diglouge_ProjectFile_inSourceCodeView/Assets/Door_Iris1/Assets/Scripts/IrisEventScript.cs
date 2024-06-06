using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Syuntoku.DigMode.Settings;

/*
 *  IrisEventScript
 * 
 *  This script is designed as the single point for other scripts to interact with the door. The general use is:
 * 
 *  1. Scripts can call SignalOpen, SignalClose or SignalToggle to ask the door to open/close/toggle
 *  2. Scripts can call SignalPause, SignalUnpause or SignalTogglePause to ask the door to pause/unpause/toggle
 *  3. Scripts can register handlers for the onDoorOpened, onDoorClosed, onDoorOpeningStarted,
 * onDoorClosingStarted, onDoorPaused or onDoorUnpaused events.
 * 
 *  The script also uses its own event system to run its sounds to help demonstrate them. There are also a
 * few scripts in the test scene that demonstrate its use: DoorSwitch, PauseSwitch, TestLiftScript and
 * DoorSwitchConsole.
 * 
 *  If you don't want to use this script, you should also remove or repurpose the scripts on the states in the
 * animator.
 */

public class IrisEventScript : MonoBehaviour {

	public bool paused { get; private set;} = false; // tracks if the door's animation is paused
	Animator animator; // the animator associated with this script's door

    // State hashes for detecting what state the animator is on
	static readonly int closingStateHash = Animator.StringToHash("Base Layer.Closing");
	static readonly int openingStateHash = Animator.StringToHash("Base Layer.Opening");
	static readonly int openingParameterHash = Animator.StringToHash("isOpening");

    public bool startsOpen; // TRUE if the door is open initially
	public AudioClip openSound;
	public AudioClip closeSound;
	public AudioClip movingSound;
	public AudioClip stopSound;
	[SerializeField] GameObject _player;
	[SerializeField] float _openingLength;
	[SerializeField] GameSetting _gameSetting;

    public float doorSpeed; // the door's animation speed

    // Two separate AudioSources are used because AudioSource.isPlaying() seems to count one-shots now.
	AudioSource doorLoopAudioSource, doorOneShotAudioSource;

	public class AnimationEventArgs : EventArgs // Event argument object, containing a ref to the script
	{
		public IrisEventScript doorScript; 
		public AnimationEventArgs(IrisEventScript script)
		{
			doorScript = script;
		}
	}

	// Events you can register to find out when the door enters various states
	public event EventHandler<AnimationEventArgs> onDoorOpened = delegate {};
	public event EventHandler<AnimationEventArgs> onDoorClosed = delegate {};
	public event EventHandler<AnimationEventArgs> onDoorOpeningStarted = delegate {};
	public event EventHandler<AnimationEventArgs> onDoorClosingStarted = delegate {};
	public event EventHandler<AnimationEventArgs> onDoorPaused = delegate {};
	public event EventHandler<AnimationEventArgs> onDoorUnpaused = delegate {};

	void Start () {
		animator = GetComponent<Animator>();
        animator.SetBool(openingParameterHash, startsOpen);
        animator.speed = doorSpeed;

        // Set references to the audio sources
        AudioSource[] doorAudioSources = GetComponents<AudioSource>();
        Assert.AreEqual(doorAudioSources.Length, 2);
		doorLoopAudioSource = doorAudioSources[0];
        doorOneShotAudioSource = doorAudioSources[1];

        Invoke("RegisterForSoundEvents", 0.05f);
	}

    private void Update()
    {
		if (_gameSetting.bStopGameAction) return;
		float length = (_player.transform.position - transform.position).sqrMagnitude;

		if (length > _openingLength * _openingLength + _openingLength) return;

		if (length <= _openingLength * _openingLength)
		{
			SignalOpen();
		}
		else
		{
			SignalClose();
		}
	}

    void RegisterForSoundEvents()
	{     
		if (closeSound)
			onDoorClosed += PlayDoorClosedSound;
		if (openSound)
			onDoorOpeningStarted += PlayDoorOpenedSound;
        if (stopSound)
        {
            onDoorClosingStarted += PlayDoorStopSound;
            onDoorOpened += PlayDoorStopSound;
        }
        if (movingSound)
		{
			onDoorOpeningStarted += StartDoorMovingSound;
			onDoorClosingStarted += StartDoorMovingSound;
			onDoorClosed += StopDoorMovingSound;
			onDoorOpened += StopDoorMovingSound;
			doorLoopAudioSource.clip = movingSound;
		}
	}

    /*
     * SignalOpen: a public method that asks the door to open.   
     */   
	public void SignalOpen()
	{
		bool opening = animator.GetBool(openingParameterHash);
		if (!opening) // only open if we aren't already opening
		{
			// If door state is Closing, play Opening at the appropriate point to allow changing state midway
			if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == closingStateHash)
			{
				animator.Play("Opening", 0, 1.0f - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			}
			animator.SetBool(openingParameterHash, true); // update the animator's state
		}
	}

    /*
     * SignalClose: a public method that asks the door to close.
     */
	public void SignalClose()
	{
		bool opening = animator.GetBool(openingParameterHash);
		if (opening) // only close if we aren't already closing
		{
			// If door state is Closing, play Opening at the appropriate point
			if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == openingStateHash)
			{
				animator.Play("Closing", 0, 1.0f - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			}
			animator.SetBool(openingParameterHash, false); // update the animator's state

		}
	}

    /*
     * SignalToggle: toggle the door to whatever state it's not in already
     */
	public void SignalToggle()
	{
		bool opening = animator.GetBool(openingParameterHash);
		if (opening)
			SignalClose();
		else
			SignalOpen();
	}

    /*
     * SignalPause: if the door is moving, halt its movement until unpaused.
     */
	public void SignalPause()
	{
        // The pause signal will pause the door only if it's in a moving state. If it's not, the signal
        // will just be discarded.
        if (!paused
         && (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == openingStateHash
         || animator.GetCurrentAnimatorStateInfo(0).fullPathHash == closingStateHash))
        {
            animator.speed = 0.0f;
            StopDoorMovingSound(this, null);
            paused = true;
            FireDoorPausedEvent();
        }
	}

    /*
     * SignalUnpause: if the door's movement is paused, unpause it
     */
	public void SignalUnpause()
	{
        if (paused)
        {
            animator.speed = doorSpeed;
            StartDoorMovingSound(this, null);
            paused = false;
            FireDoorUnpausedEvent();
        }
	}

    /*
     * SignalTogglePause: toggle the pause state
     */
	public void SignalTogglePause()
	{
		if (paused)
			SignalUnpause();
		else
			SignalPause();
	}

    /*
     *  These events handle all the sounds of the door using the door's event system.
     */
	void StartDoorMovingSound(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
		doorLoopAudioSource.loop = true;
        if (!(doorLoopAudioSource.isPlaying))
    		doorLoopAudioSource.Play();
	}

	void StopDoorMovingSound(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
		doorLoopAudioSource.loop = false;
	}

	void PlayDoorClosedSound(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
		doorOneShotAudioSource.PlayOneShot(closeSound);
	}

	void PlayDoorOpenedSound(object sender, IrisEventScript.AnimationEventArgs eventArgs)
	{
        if (openSound && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.0f)
        {
            doorOneShotAudioSource.PlayOneShot(openSound);
        }
        else if (stopSound)
            PlayDoorStopSound(sender, eventArgs);
	}

    void PlayDoorStopSound(object sender, IrisEventScript.AnimationEventArgs eventArgs)
    {
        doorOneShotAudioSource.PlayOneShot(stopSound);
    }

    /*
     *  These functions are for use by the event system. They are fired by the scripts on the animator states.
     */

	public void FireDoorOpenedEvent() { onDoorOpened.Invoke(this, new AnimationEventArgs(this)); }
	public void FireDoorClosedEvent() { onDoorClosed.Invoke(this, new AnimationEventArgs(this)); }
	public void FireDoorClosingStartedEvent() { onDoorClosingStarted.Invoke(this, new AnimationEventArgs(this)); }
	public void FireDoorOpeningStartedEvent() { onDoorOpeningStarted.Invoke(this, new AnimationEventArgs(this)); }
	void FireDoorPausedEvent() { onDoorPaused.Invoke(this, new AnimationEventArgs(this)); }
	void FireDoorUnpausedEvent() { onDoorUnpaused.Invoke(this, new AnimationEventArgs(this)); }

}
