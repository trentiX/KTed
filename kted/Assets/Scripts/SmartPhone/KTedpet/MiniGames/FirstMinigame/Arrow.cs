using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	// Variables
	public int direction;
	
	/*
	0 = left
	1 = down
	2 = up
	3 = right
	*/
	
	// Code
	void Update()
	{
		this.transform.position += new Vector3(0, 1f, 0);
	}
}
