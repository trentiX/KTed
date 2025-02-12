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
	void FixedUpdate()
	{
		this.transform.position += new Vector3(0, 5f, 0);
	}
}
