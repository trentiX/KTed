using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{

	[SerializeField] private DialogueUI dialogueUI;
	[SerializeField] private AudioManager audioManager;
	[SerializeField] private MusicUI musicUI;
	[SerializeField] private PictureBoxUI pictureBoxUI;
	[SerializeField] private GameObject panel;
	[SerializeField] private Joystick movementJoystick; // Reference to the joystick
	[SerializeField] private GameObject skipButtonPrefab;
	[SerializeField] private GameObject interactButtonPrefab;



	private bool onButtonCliked = false;
	public string location = "none";
	public DialogueUI DialogueUI => dialogueUI;
	public MusicUI MusicUI => musicUI;
	public PictureBoxUI PictureBoxUI => pictureBoxUI;
	public AudioManager AudioManager => audioManager;
	public KeysActivator KeysActivator => KeysActivator;

	private CameraController _cameraController;
	private Browser _browser;
	private TestHandler testHandler;
	public static GameObject skipButton;
	public static GameObject interactButton;
	public static Player playerInstance { get; private set; }


	public IInteractable Interactable { get; set; }

	// Components
	Rigidbody2D rb;

	// Player
	[SerializeField] float walkSpeed = 3.3f;
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
		playerInstance = this;
	    skipButton = skipButtonPrefab;
	    interactButton = interactButtonPrefab;
		_cameraController 
		= FindObjectOfType<CameraController>(); // Find and assign the CameraController
		rb = GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
		_browser = FindObjectOfType<Browser>();
		testHandler = FindObjectOfType<TestHandler>();
	}

	public void PauseGame()
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
	
	private void Update()
	{
		if (!CaptureInput()) return;
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}
		
		if (!canMove())
		{
			rb.velocity = Vector2.zero;
			ChangeAnimationState(IdleAnimation());
			return;
		}

		if (movementJoystick != null && movementJoystick.gameObject.activeInHierarchy)
		{
			inputHorizontal = movementJoystick.Horizontal;
			inputVertical = movementJoystick.Vertical;
		}
		else
		{
			inputHorizontal = Input.GetAxisRaw("Horizontal");
			inputVertical = Input.GetAxisRaw("Vertical");
		}


		if (Input.GetKeyDown(KeyCode.E) || onButtonCliked)
        {
            Interactable?.Interact(this);
            onButtonCliked = false;
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
			|| Pause.isOpen || testHandler.TestOpen || _browser.browserOpen )
		{
			return false;
		}
		else
			return true;
	}
	
	public bool CaptureInput()
	{
		if (KTedpet.instance == null) return true;
		if (KTedpet.instance.gameMode)
		{
			return false;
		}
		else
			return true;
	}
	
	public void OnPointerDown()
    {
        onButtonCliked = true;
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
