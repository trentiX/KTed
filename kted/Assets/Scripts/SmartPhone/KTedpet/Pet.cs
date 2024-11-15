using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
	// Phrases
	private string[] onPlayerJoin = {"Hello", "Hi!"};
	
	
	// References
	private KTedpet kTedpet;
	// Code
	private void Start()
	{
		kTedpet = FindObjectOfType<KTedpet>();
		
		kTedpet.GenerateMessage(onPlayerJoin
			[Random.Range(0, onPlayerJoin.Length)], "start");
	}
}
