using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    // Components
    Rigidbody2D rb;

    // Player
    float walkSpeed = 4f;
    float speedLimiter = 0.7f;
    float inputHorizontal;
    float inputVertical;

    // Animations and states
    Animator animator;
    string currentState;
    const string PLAYER_LEFT = "Player_Left";
    const string PLAYER_RIGHT = "Player_Right";
    const string PLAYER_UP = "Player_Up";
    const string PLAYER_DOWN = "Player_Down";
    const string PLAYER_IDLE = "Player_Idle";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (dialogueUI.IsOpen)
        {
            rb.velocity = Vector2.zero;
            ChangeAnimationState(PLAYER_IDLE);
            return;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
        }

        if (inputHorizontal != 0 || inputVertical != 0)
        {
            if (inputHorizontal != 0 && inputVertical != 0)
            {
                inputHorizontal *= speedLimiter;
                inputVertical *= speedLimiter;

            }
            rb.velocity = new Vector2(inputHorizontal * walkSpeed, inputVertical * walkSpeed);

            if (inputHorizontal > 0)
            {
                ChangeAnimationState(PLAYER_RIGHT);
            }
            else if (inputHorizontal < 0)
            {
                ChangeAnimationState(PLAYER_LEFT);
            }
            else if (inputVertical > 0)
            {
                ChangeAnimationState(PLAYER_UP);
            }
            else if (inputVertical < 0)
            {
                ChangeAnimationState(PLAYER_DOWN);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
