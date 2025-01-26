using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelatedTransition : MonoBehaviour
{	
	// Pixelated Transition
	[SerializeField] public GameObject pixelatedPanel;
	[SerializeField] public Camera mainCamera;
	[SerializeField] public Camera transitionCamera;
	[SerializeField] public RenderTexture RT;
	
	
	// Variables
	public static PixelatedTransition instance;
	int originalWidth = 1920;
	int originalHeight = 1080;
	
	private void Awake()
	{
		instance = this;
	}
	
	public void TransitionIn(string sceneName)
	{
		Debug.Log("TransitionIN");
		StartCoroutine(PixelationInAnimation(sceneName));
	}
	public void TransitionOut()
	{
		StartCoroutine(PixelationOutAnimation());
	}
	
	private IEnumerator PixelationInAnimation(string gameScene)
	{
		Debug.Log("courutine started");
		originalWidth = RT.width;
		originalHeight = RT.height;
		
		pixelatedPanel.SetActive(true);
		mainCamera.GetComponent<Camera>().enabled = false;
		transitionCamera.GetComponent<Camera>().enabled = true;

		int steps = 100;
		float waitTime = 0.04f;
		
		for (int i = 0; i < steps; i++)
		{
			yield return new WaitForSeconds(waitTime);
			AdjustRenderTextureSize(i, steps);
		}
		
		SceneManager.LoadScene(gameScene);
	}
	
	private IEnumerator PixelationOutAnimation()
	{
		pixelatedPanel.SetActive(true);
		mainCamera.GetComponent<Camera>().enabled = false;
		transitionCamera.GetComponent<Camera>().enabled = true;
		
		int steps = 100;
		float waitTime = 0.04f;
		
		for (int i = 0; i < steps; i++)
		{
			yield return new WaitForSeconds(waitTime);
			AdjustRenderTextureSize(steps - i - 1, steps);
		}
		
		RestoreOriginalRenderTextureSize();
		pixelatedPanel.SetActive(false);
		transitionCamera.GetComponent<Camera>().enabled = false;
		mainCamera.GetComponent<Camera>().enabled = true;	
	}
	
	private void AdjustRenderTextureSize(int step, int totalSteps)
	{
		RT.Release();
		
		int newWidth = Mathf.Max(1, (int)(originalWidth * (totalSteps - step) / totalSteps));
		int newHeight = Mathf.Max(1, (int)(originalHeight * (totalSteps - step) / totalSteps));

		RT.width = newWidth;
		RT.height = newHeight;
		
		RT.Create();
	}
	
	private void RestoreOriginalRenderTextureSize()
	{
		RT.Release();
		RT.width = originalWidth;
		RT.height = originalHeight;
		RT.Create();
	}
	private void ResetRenderTextureSize()
	{
		if (RT == null) return;
		
		RT.Release();
		RT.width = 1920;
		RT.height = 1080;
		RT.Create();
	}
	
	private void OnApplicationQuit()
	{
		ResetRenderTextureSize();
	}
}
