using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PetAnimations : MonoBehaviour
{
    // Serialization
    [SerializeField] private Animator petAnimator;
    
    
    // Variables
    private string currentState;
    
    // Animations
    const string Pet_IDLE = "Pet_IDLE";
    const string Pet_LookingAround = "Pet_LookingAround";
    const string Pet_Yawning = "Pet_Yawning";
    const string Pet_Sleeping = "Pet_Sleeping";

    // Code
    private void Start()
    {
        ChangeAnimationState(Pet_IDLE);    
        StartCoroutine(AnimationsCoroutine());
    }

    private IEnumerator AnimationsCoroutine()
    {
        int delayBeforeAction  = UnityEngine.Random.Range(7, 12); // RANDOM DELAY BETWEEN ANIMATIONS
        Debug.Log("Delay before action: " + delayBeforeAction);
        yield return new WaitForSeconds(delayBeforeAction);
        
        int k = UnityEngine.Random.Range(0, 2); // RANDOM ANIMATION
        switch (k)
        {
            case 0:
                ChangeAnimationState(Pet_LookingAround);
                break;
            case 1:
                ChangeAnimationState(Pet_Yawning);
                break;
            case 2:
                ChangeAnimationState(Pet_Sleeping);
                break;
        }
        
        float animationLength = GetAnimationClipLength(currentState); // GETTING LENGTH OF CURRENT ANIMATION
        yield return new WaitForSeconds(animationLength); // WAITING FOR ANIMATION TO FINISH
        ChangeAnimationState(Pet_IDLE); // RETURNING TO IDLE ANIMATION
        
        StartCoroutine(AnimationsCoroutine()); // RECURSIVE CALL TO CONTINUE THE LOOP FOR PET ANIMATION
    }
    
    private void ChangeAnimationState(string newState)
	{
		if (currentState == newState) return;

		petAnimator.Play(newState);

		currentState = newState;
	}
	
	private float GetAnimationClipLength(string animationName)
    {
        foreach (AnimationClip clip in petAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f; // если не нашли анимацию
    }
}
