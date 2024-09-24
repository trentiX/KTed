using System;
using Unity.Mathematics;
using UnityEngine;

public class EasterEggPickUp : MonoBehaviour
{
    [Header("EasterEgg:")]
    [SerializeField] private GameObject pickUpFX;
    [SerializeField] private GameObject pickedPoint;
    [SerializeField] private bool music;
    [SerializeField] private bool smartPhone;
    
    [HideInInspector] public static event Action<DialogueObject> EasterEggPickedUp;
    private CameraController _cameraController;
    private RingManager _ringManager;

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>(); // Find and assign the CameraController
        if (_cameraController == null)
        {
            Debug.LogError("CameraController not found in the scene!");
        }

        _ringManager = FindObjectOfType<RingManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EasterEggManager easterEggManager = FindObjectOfType<EasterEggManager>();
            EasterEggPickedUp?.Invoke(_ringManager._dialogueObjectEasterEgg); // convey info to ringManager so smartphone will ring
            
            if (easterEggManager != null)
            {
                AudioManager.Instance.EasterEggSound();
                EasterEggPosition();

                if (music)
                {
                    easterEggManager.MusicPlatePickedUp(gameObject);
                }
                else if (smartPhone)
                {
                    easterEggManager.SmartPhonePickedUp();
                }
                else 
                {
                    easterEggManager.IncreaseEasterEggCount();
                }
            }
        }
    }

    private void EasterEggPosition()
    {
        if (!smartPhone)
        {
            GameObject fx = Instantiate(pickUpFX, transform.position, quaternion.identity);
            Destroy(fx, 1.9f);
        
            if (_cameraController != null)
            {
                var position = pickedPoint.transform.position;
                _cameraController.TransitionCamera(position.x, position.y); // Call TransitionCamera method of the CameraController
            }
            else
            {
                Debug.LogWarning("CameraController is not assigned.");
            }

            gameObject.transform.position = pickedPoint.transform.position;
            BoxCollider2D coll = gameObject.GetComponent<BoxCollider2D>();
            coll.size = new Vector2(0.001f, 0.001f);

            GameObject fx2 = Instantiate(pickUpFX, transform.position, quaternion.identity);
            Destroy(fx2, 1.9f);   
        }

        else if (smartPhone)
        {
            GameObject fx = Instantiate(pickUpFX, transform.position, quaternion.identity);
            Destroy(fx, 1.9f);
        }
    }
}