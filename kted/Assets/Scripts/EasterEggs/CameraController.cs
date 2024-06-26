using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;
    public bool transition;

    private float sizeOfCam;

    private void Start()
    {
        transition = false;
        cam = GetComponent<Camera>(); // Get the Camera component attached to this GameObject
        sizeOfCam = cam.orthographicSize;
    }

    public void TransitionCamera(float posX, float posY)
    {
        if (!transition) // Ensure transition is not already in progress
        {
            StartCoroutine(TransitionCoroutine(posX, posY));
        }
    }
    
    IEnumerator TransitionCoroutine(float posX, float posY)
    {
        transition = true;
        
        yield return new WaitForSeconds(0.1f);

        // Transition camera to fixed position
        gameObject.transform.position = new Vector3(posX, posY, -10);
        cam.orthographicSize = 4.5f;

        yield return new WaitForSeconds(2f);

        // Transition camera back to player's position
        cam.orthographicSize = sizeOfCam;
        gameObject.transform.position = new Vector3(player.position.x, player.position.y, -10);

        transition = false; // Reset transition flag after transition is complete
    }
}