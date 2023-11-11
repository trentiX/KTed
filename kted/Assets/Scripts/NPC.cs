using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Animator animator;
    const string NPC_IDLE = "NPC_Idle";

    private void Start()
    {
        animator.Play(NPC_IDLE);
    }
}
