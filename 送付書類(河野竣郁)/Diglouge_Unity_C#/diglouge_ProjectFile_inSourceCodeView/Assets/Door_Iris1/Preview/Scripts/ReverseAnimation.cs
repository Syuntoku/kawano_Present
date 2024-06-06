using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseAnimation : StateMachineBehaviour {

	Transform openSound;
	Transform closeSound;
	Transform movingSound;
	Transform stopSound;

	float normalizedTime;
	bool closing;
	bool paused;
	bool stopOnExit;
	enum Sound {
		open,
		close,
		moving,
		stop
	}


	// GetSound returns an AudioSource of the requested sound, or null if it wasn't found
	AudioSource GetSound(Sound snd) {
		AudioSource effect = null;

		switch (snd) {
		case Sound.open:
			if (openSound != null)
				effect = openSound.GetComponent<AudioSource>();
			break;
		case Sound.close:
			if (closeSound != null)
				effect = closeSound.GetComponent<AudioSource>();
			break;
		case Sound.moving:
			if (movingSound != null)
				effect = movingSound.GetComponent<AudioSource>();
			break;
		case Sound.stop:
			if (stopSound != null)
				effect = stopSound.GetComponent<AudioSource>();
			break;
		default:
			break;
		}

		return effect;
	}

	// PlaySound checks the sound exists before playing it, using delay if positive. Can also check if its playing before trying to play. True on play
	bool PlaySound(Sound snd, float delay = 0f, bool checkPlaying = false) {
		AudioSource effect = GetSound(snd);

		if (effect != null) {
			if (!checkPlaying || !effect.isPlaying) {
				if (delay > 0f)
					effect.PlayDelayed(delay);
				else
					effect.Play();
				return true;
			}
		}
		return false;
	}

	// StopSound checks if a sound exists and is playing before stopping it, returning true if the sound was playing
	bool StopSound(Sound snd) {
		AudioSource effect = GetSound(snd);

		if (effect != null && effect.isPlaying) {
			effect.Stop();
			return true;
		}
		return false;
	}

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// Get the different sounds from the door, they should be GameObjects with the listed names containing an AudioSource each
		openSound = animator.gameObject.transform.Find("Sound_Open");
		closeSound = animator.gameObject.transform.Find("Sound_Close");
		movingSound = animator.gameObject.transform.Find("Sound_Moving");
		stopSound = animator.gameObject.transform.Find("Sound_Stop");

		paused = false;
		stopOnExit = true;

		// This script is called by either the 'Opening' or 'Closing' animation states
		if (stateInfo.IsName("Opening")) {
			// Door either just started OPENING or it switched from closing to opening part way through
			if (stateInfo.normalizedTime == 0) {
				// If the time is zero then it should have just started opening, so play the open sound and queue up the moving sound.
				PlaySound(Sound.open);
				PlaySound(Sound.moving, 0.144f, true);
			} else {
				PlaySound(Sound.stop);
			}
			closing = false;
		} else {
			// Door either just started CLOSING or switched directions part way.
			PlaySound(Sound.stop);
			if (stateInfo.normalizedTime == 0) {
				// If the time is zero then it should have just started closing, so queue up the moving sound (stop sound is always played in this case).
				PlaySound(Sound.moving, 0.144f, true);
			}
			closing = true;	
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		normalizedTime = stateInfo.normalizedTime;
		int state = animator.GetInteger("State");

		// States are from 0-2:
		// 0 = Opening state, when in state 0 the 'Opening' animation should be playing or finished.
		// 1 = Paused state, when in state 1 either animation could be playing but should be paused.
		// 2 = Closing state, when in state 2 the 'Closing' animation should be playing or finished.

		/* The switch can either be in bi-state mode, in which case the 1 state will never be set,
		 * or it can be in the tri-state mode which will enable the paused state. The switch script
		 * updates the 'State' value of the animator when it is toggled. To allow for the animations
		 * to change direction seamlessly changes to state are checked here on each update */

		if (state == 1) {
			if (!paused) {
				// State is paused
				paused = true;
				animator.speed = 0;
				StopSound(Sound.moving);
				PlaySound(Sound.stop);
			}
		} else {
			if (paused) {
				// Logic for coming out of paused state
				paused = false;
				animator.speed = 1;
				PlaySound(Sound.stop);
				PlaySound(Sound.moving, 0.144f, true);
			}
			
			if (closing && state != 2) {
				// State doesn't match the direction, swap to the opening animation at the inverted time (don't stop sounds when this animation exits)
				stopOnExit = false;
				animator.Play("Opening", layerIndex, (1 - normalizedTime));
			}
			else if (!closing && state != 0) {
				// State doesn't match the direction, swap to the closing animation at the inverted time (don't stop sounds when this animation exits)
				stopOnExit = false;
				animator.Play("Closing", layerIndex, (1 - normalizedTime));
			}
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stopOnExit) {
			// Stop the moving sound and play the close sound if we just closed
			if (StopSound(Sound.moving)) {
				PlaySound(Sound.stop);
			}
			if (closing) {
				PlaySound(Sound.close);
			}
		}
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
