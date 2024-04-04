using Unity.Mathematics;
using UnityEngine;

public class EasterEggPickUp : MonoBehaviour
{
    [SerializeField] private GameObject pickUpFX;
    [SerializeField] private GameObject pickedPoint;
    private CameraController _cameraController;

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>(); // Find and assign the CameraController
        if (_cameraController == null)
        {
            Debug.LogError("CameraController not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EasterEggManager easterEggManager = FindObjectOfType<EasterEggManager>();
            if (easterEggManager != null)
            {
                AudioManager.Instance.EasterEggSound();
                easterEggManager.IncreaseEasterEggCount();
                EasterEggPosition();
            }
        }
    }

    private void EasterEggPosition()
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
}