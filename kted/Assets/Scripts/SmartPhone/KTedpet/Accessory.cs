using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Accessory : MonoBehaviour
{
	// Serialization
	[SerializeField] public int Value;
	[SerializeField] public GameObject image;	
	[SerializeField] public UnityEngine.UI.Button buyButton;
	[SerializeField] public PetAppearance petAppearance;
		
	// Variables
	public bool purchased;
	public bool equipped;
	private Tweener itemAnim;
	private PetShopManager petShopManager;
	
	// Code
	private void Awake()
	{
		petShopManager = FindObjectOfType<PetShopManager>();
		
		if (!petShopManager.boughtAccessories.ContainsKey(this))
			petShopManager.boughtAccessories.Add(this, false);
	}
	
	public void buttonAdjustment()
	{	
		buyButton.onClick.RemoveAllListeners();
		if (!equipped && petShopManager.boughtAccessories[this])
		{
			purchased = true;
			buyButton.gameObject.GetComponentInChildren
				<TextMeshProUGUI>().text = "Надеть";
			buyButton.onClick.AddListener(() => petShopManager.PutOnItem(this));
		}	
		else if (equipped && petShopManager.boughtAccessories[this])
		{
			purchased = true;
			buyButton.gameObject.GetComponentInChildren
				<TextMeshProUGUI>().text = "Снять";
			buyButton.onClick.AddListener(() => petShopManager.TakeOffItem(this));
		}
		else
		{
			purchased = false;
			buyButton.gameObject.GetComponentInChildren
				<TextMeshProUGUI>().text = "Купить";
			buyButton.onClick.AddListener(petShopManager.BuyItem);
		}
	}
	
	private void OnEnable()
	{
		image.transform.Rotate(0, 0, -0.5f);
		itemAnim = image.transform.DORotate(new Vector3(0, 0, 1), 1f)
			.SetLoops(-1, LoopType.Yoyo)
			.SetEase(Ease.InOutSine);
	}
	private void OnDisable()
	{
		itemAnim.Kill();
	}
}
