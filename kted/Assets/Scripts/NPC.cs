using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string facingTo;
    [SerializeField] private string NPC_type;

    
    Animator animator;
    const string NPC_IDLE = "NPC_Idle"; 
    const string NPC1_IDLE = "NPC1_Idle"; 
    const string NPC_IDLE_FRONT = "NPC_Idle_front";
    const string NPC1_IDLE_FRONT = "NPC1_Idle_front";


    private void Start()
    {
        switch (NPC_type)
        {
            case "NPC":
                switch (facingTo)
                {
                    case "front": animator.Play(NPC_IDLE);
                        break;
                    case "toPlayer": animator.Play(NPC_IDLE_FRONT);
                        break;
                }
                break;
            case "NPC1":
                switch (facingTo)
                {
                    case "front": animator.Play(NPC1_IDLE);
                        break;
                    case "toPlayer": animator.Play(NPC1_IDLE_FRONT);
                        break;
                }
                break;
        }

    }
}
