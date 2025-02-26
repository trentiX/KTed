using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	// Variables
	public int direction;
	public GameObject killPos;
	
	/*
	0 = left
	1 = down
	2 = up
	3 = right
	*/
	
	// Code
	private void Start()
	{
		this.transform.localScale = new Vector3(0, 0, 0);
		this.transform.DOScale(new Vector3(1.6f, 1.6f, 1.6f), 1.5f).SetEase(Ease.InSine);
	}
	
	void FixedUpdate()
	{
		this.transform.position += new Vector3(0, 5f, 0);
	}
}
