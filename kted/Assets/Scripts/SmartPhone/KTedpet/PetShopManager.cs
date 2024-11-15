using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UIElements;

public class PetShopManager : MonoBehaviour
{
	// References
	[SerializeField] private List<GameObject> accessories;
	
	
	// Variables
	private GameObject currItem;
	private GameObject leftItem;
	private GameObject rightItem;
	private List<GameObject> availableAccessories;
	private int itemIndex;
 	
	// Code
	private void Start()
	{
		availableAccessories = accessories;
	}
	
	public void RightItem()
	{
		currItem.transform.DOMoveX(140, 0.5f);
		currItem.GetComponent<UnityEngine.UIElements.Image>().sprite.dofa
	}
	
	public void LeftItem()
	{
		
	}
}
