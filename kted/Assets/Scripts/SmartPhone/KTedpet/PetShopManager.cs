using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class PetShopManager : MonoBehaviour, IDataPersistence
{
	// References
	[SerializeField] public List<GameObject> accessories;
	
	
	// Variables
	public SerializableDictionary<Accessory, bool> boughtAccessories;
	public SerializableDictionary<Accessory, bool> equippedAccessories;
	private GameObject currItem;
	private GameObject leftItem;
	private GameObject rightItem;
	private List<GameObject> availableAccessories;
	private Tweener accessoriesAnim;
	private Ktedwork ktedwork;
	private AudioManager audioManager;
 	
	// Code
	private void Start()
	{
		audioManager = FindObjectOfType<AudioManager>();
		ktedwork = FindObjectOfType<Ktedwork>();

		availableAccessories = accessories;
		
		leftItem = availableAccessories[availableAccessories.Count - 1];
		currItem = availableAccessories[0];
		rightItem = availableAccessories[1];
		
		foreach (var item in availableAccessories)
		{
			item.GetComponent<Accessory>().buttonAdjustment();
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
			= leftItem.GetComponentInChildren<UnityEngine.UI.Image>().color;
		curItemImage.a = 0;
		ChangeItemPosition(leftItem, 0, 1f, 1f);
		
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
			= rightItem.GetComponentInChildren<UnityEngine.UI.Image>().color;
		curItemImage.a = 0;
		ChangeItemPosition(rightItem, 0, 1f, 1f);
		
		leftItem = currItem;		
		currItem = rightItem;
		
		if (availableAccessories.IndexOf(rightItem) < availableAccessories.Count - 1)
		{
			rightItem = availableAccessories
				[availableAccessories.IndexOf(rightItem) + 1];		
		}
		else 
		{
			rightItem = availableAccessories[0];	
		}
	}
	
	public void BuyItem()
	{
		if (ktedwork._accBalanceInt >= currItem.GetComponent<Accessory>().Value)
		{
			ktedwork.AccBalanceUIUpdate(
				ktedwork._accBalanceInt - currItem.GetComponent<Accessory>().Value);
			
			audioManager.SFXQuestBitCompletion();
			
			boughtAccessories[currItem.GetComponent<Accessory>()] = true;
			currItem.GetComponent<Accessory>().buttonAdjustment();
			
			int i = Random.Range(0, Pet.instance.onItemPurchasedPetPhrases.Length);
			KTedpet.instance.GenerateMessage(Pet.instance.onItemPurchasedPetPhrases[i], "whatToDo");
		}
	}
	
	public void PutOnItem(Accessory accessory)
	{
		PetAnimations.instance.ChangePetAppearance(accessory.petAppearance);
		accessory.equipped = true;
		
		equippedAccessories[accessory] = true;
		accessory.buttonAdjustment();
	}
	
	public void TakeOffItem(Accessory accessory)
	{
		PetAnimations.instance.RemovePetAppearance(accessory.petAppearance);
		accessory.equipped = false;
		
		equippedAccessories[accessory] = false;
		accessory.buttonAdjustment();
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
	
	// Data
	
	public void LoadData(GameData gameData)
	{
		// Загружаем данные
		boughtAccessories = gameData.boughtAccessoriesInStorage ?? new SerializableDictionary<Accessory, bool>();
		equippedAccessories = gameData.equippedAccessoriesInStorage ?? new SerializableDictionary<Accessory, bool>();

		// Проверяем, что все аксессуары добавлены
		foreach (var accessory in accessories)
		{
			Accessory accessoryComponent = accessory.GetComponent<Accessory>();

			if (!boughtAccessories.ContainsKey(accessoryComponent))
				boughtAccessories.Add(accessoryComponent, false);

			if (!equippedAccessories.ContainsKey(accessoryComponent))
				equippedAccessories.Add(accessoryComponent, false);

			// Настраиваем кнопку
			accessoryComponent.buttonAdjustment();
		}
	}

	
	public void SaveData(ref GameData gameData)
	{
		gameData.boughtAccessoriesInStorage = boughtAccessories;
		gameData.equippedAccessoriesInStorage = equippedAccessories;
	}
}
