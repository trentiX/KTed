using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private MusicUI musicUI;
    [SerializeField] private PictureBoxUI pictureBoxUI;
    [SerializeField] private GameObject panel;

    public string location = "none";
    public DialogueUI DialogueUI => dialogueUI;
    public MusicUI MusicUI => musicUI;
    public PictureBoxUI PictureBoxUI => pictureBoxUI;
    public AudioManager AudioManager => audioManager;
    public KeysActivator KeysActivator => KeysActivator;

    private CameraController _cameraController;
    private Messanger _messanger;

    public IInteractable Interactable { get; set; }

    // Components
    Rigidbody2D rb;

    // Player
    float walkSpeed = 3.3f;
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
        _cameraController = FindObjectOfType<CameraController>(); // Find and assign the CameraController
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        _messanger = FindObjectOfType<Messanger>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.isOpen = true;
            Time.timeScale = 0f;
        
            if (panel.activeSelf == true)
            {
                panel.SetActive(false);
                Pause.isOpen = false;
                Time.timeScale = 1f;
            }
            else
            {
                Pause.isOpen = true;
                panel.SetActive(true);
            }
        }
        
        if (!canMove())
        {
            rb.velocity = Vector2.zero;
            ChangeAnimationState(IdleAnimation());
            return;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal") + MobileInput.HorizontalAxis;
        inputVertical = Input.GetAxisRaw("Vertical") + MobileInput.VerticalAxis;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
        }
        
        if (inputHorizontal != 0 || inputVertical != 0)
        {
#if !UNITY_ANDROID
            if (inputHorizontal != 0 && inputVertical != 0)
            {
                inputHorizontal *= speedLimiter;
                inputVertical *= speedLimiter;
            }
#endif
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

    public bool canMove()
    {
        if (dialogueUI.DialogueOpen || musicUI.MusicOpen || pictureBoxUI.PictureOpen || _cameraController.transition 
            || Pause.isOpen || _messanger.messangerIsOpen)
        {
            return false;
        }
        else
            return true;
    }
    
    // DATA
    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
