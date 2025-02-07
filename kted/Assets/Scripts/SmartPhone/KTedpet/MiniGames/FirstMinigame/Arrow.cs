using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	// Variables
	public int direction;
	
	
	// Code
	void Update()
	{
		this.transform.position += new Vector3(0, 1f, 0);
	}
}
