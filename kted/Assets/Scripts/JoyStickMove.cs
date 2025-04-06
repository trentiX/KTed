using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    [SerializeField] private PictureBoxUI pictureBoxUI;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private MusicUI musicUI;
    
    public Joystick movementJoystick;
    public float playerSpeed;
    private Rigidbody2D rb;
    public DialogueUI DialogueUI => dialogueUI;
    public MusicUI MusicUI => musicUI;
    public PictureBoxUI PictureBoxUI => pictureBoxUI;
    private CameraController _cameraController;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _cameraController = FindObjectOfType<CameraController>(); // Find and assign the CameraController
    }

    private void FixedUpdate()
    {
        if (Pause.isOpen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (dialogueUI.DialogueOpen || musicUI.MusicOpen || pictureBoxUI.PictureOpen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (_cameraController.transition)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if(movementJoystick.Direction.y != 0)
        {
            rb.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, movementJoystick.Direction.y * playerSpeed);
        } else
        {
            rb.velocity = Vector2.zero;
        }
    }

}