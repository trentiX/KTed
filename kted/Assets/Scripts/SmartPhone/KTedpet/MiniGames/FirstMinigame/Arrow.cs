using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
	// Variables
	public int direction;
	public GameObject killPos;
	public Color color;
	
	/*
	0 = left
	1 = down
	2 = up
	3 = right
	*/
	
	// Code
	private void Start()
	{
		GetComponent<CanvasGroup>().alpha = 0;
		GetComponent<CanvasGroup>().DOFade(1, 2f);
	}
	
	void FixedUpdate()
	{
		this.transform.position += new Vector3(0, 5f, 0);
	}

	public void SetColor(Color32 color)
	{
	    this.GetComponent<RawImage>().color = color;
	}
}
