using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PetShopManager : MonoBehaviour
{
	// References
	[SerializeField] private List<GameObject> accessories;
	
	
	// Variables
	public GameObject currItem;
	public GameObject leftItem;
	public GameObject rightItem;
	public List<GameObject> availableAccessories;
	private Tweener accessoriesAnim;
 	
	// Code
	private void Start()
	{
		availableAccessories = accessories;
		
		leftItem = availableAccessories[availableAccessories.Count - 1];
		currItem = availableAccessories[0];
		rightItem = availableAccessories[1];
		
		foreach (var item in availableAccessories)
		{
			item.SetActive(false);
		}
		
		currItem.SetActive(true);
	}
	
	public void RightArrow()
	{
		if (accessoriesAnim.IsActive()) return;
		ChangeItemPosition(currItem, 500, 0, 0);
		
		leftItem.SetActive(true);
		leftItem.transform.localPosition = new Vector3(-500, 0, 0);
		leftItem.transform.localScale = new Vector3(0, 0, 0); 
		Color curItemImage 
			= leftItem.GetComponent<UnityEngine.UI.Image>().color;
		curItemImage.a = 0;
		ChangeItemPosition(leftItem, 0, 1f, 2f);
		
		rightItem = currItem;		
		currItem = leftItem;
		
		if (availableAccessories.IndexOf(leftItem) != 0)
		{
			leftItem = availableAccessories
				[availableAccessories.IndexOf(leftItem) - 1];		
		}
		else 
		{
			leftItem = availableAccessories
				[availableAccessories.Count - 1];	
		}
	}
	
	public void LeftArrow()
	{
		if (accessoriesAnim.IsActive()) return;
		ChangeItemPosition(currItem, -500, 0, 0);
		
		rightItem.SetActive(true);
		rightItem.transform.localPosition = new Vector3(500, 0, 0);
		rightItem.transform.localScale = new Vector3(0, 0, 0); 
		Color curItemImage 
			= rightItem.GetComponent<UnityEngine.UI.Image>().color;
		curItemImage.a = 0;
		ChangeItemPosition(rightItem, 0, 1f, 2f);
		
		leftItem = currItem;		
		currItem = rightItem;
		
		if (availableAccessories.IndexOf(rightItem) < availableAccessories.Count - 1)
		{
			rightItem = availableAccessories
				[availableAccessories.IndexOf(rightItem) + 1];		
		}
		else 
		{
			rightItem = availableAccessories
				[0];	
		}
	}
	
	private void ChangeItemPosition(GameObject item, float finalMoveX,
		float finalFade, float finalScale)
	{
		accessoriesAnim = item.transform.DOLocalMove(new Vector3(finalMoveX, 0, 0), 0.5f);
		UnityEngine.UI.Image curItemImage 
			= item.GetComponent<UnityEngine.UI.Image>();
		curItemImage.DOFade(finalFade, 0.5f);
		item.transform.DOScale(finalScale, 0.5f).OnComplete(() => 
		{
			if (finalFade == 0) item.SetActive(false);
		});
	}
}
