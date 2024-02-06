using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string facingTo;
    
    Animator animator;
    const string NPC_IDLE = "NPC_Idle"; 
    const string NPC_IDLE_FRONT = "NPC_Idle_front";

    private void Start()
    {
        switch (facingTo)
        {
            case "front": animator.Play(NPC_IDLE);
                break;
            case "toPlayer": animator.Play(NPC_IDLE_FRONT);
                break;
        }
    }
}
