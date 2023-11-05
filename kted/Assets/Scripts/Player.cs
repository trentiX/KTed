using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private AudioManager audioManager;

    public DialogueUI DialogueUI => dialogueUI;
    public AudioManager AudioManager => audioManager;

    public IInteractable Interactable { get; set; }

    // Components
    Rigidbody2D rb;

    // Player
    float walkSpeed = 3f;
    float speedLimiter = 0.7f;
    float inputHorizontal;
    float inputVertical;

    // Animations and states
    Animator animator;
    string currentState;
    string lastDirection;
    const string PLAYER_SIDE_WALK = "Player_Side_Walk";
    const string PLAYER_UP_WALK = "Player_Up_Walk";
    const string PLAYER_DOWN_WALK = "Player_Down_Walk";
    const string PLAYER_SIDE_IDLE = "Player_Side_Idle";
    const string PLAYER_UP_IDLE = "Player_Up_Idle";
    const string PLAYER_DOWN_IDLE = "Player_Down_Idle";

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
            ChangeAnimationState(IdleAnimation());
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
                lastDirection = "side";
                GetComponent<SpriteRenderer>().flipX = false;
                ChangeAnimationState(PLAYER_SIDE_WALK);
            }
            else if (inputHorizontal < 0)
            {
                lastDirection = "side";
                GetComponent<SpriteRenderer>().flipX = true; 
                ChangeAnimationState(PLAYER_SIDE_WALK);
            }
            else if (inputVertical < 0)
            {
                lastDirection = "down";
                ChangeAnimationState(PLAYER_DOWN_WALK);
            }
            else if (inputVertical > 0)
            {
                lastDirection = "up";
                ChangeAnimationState(PLAYER_UP_WALK);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
            ChangeAnimationState(IdleAnimation());
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    private string IdleAnimation()
    {
        switch(lastDirection)
        {
            case "side":
                return PLAYER_SIDE_IDLE;
            case "up":
                return PLAYER_UP_IDLE;
            case "down":
                return PLAYER_DOWN_IDLE;
            default:
                return null;
        }
    }
}
