using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EasterEggMenuUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI easterEggCountText;

    private void OnEnable()
    {
        EasterEggManager.OnEasterEggPickupUpdated.AddListener(UpdateEasterEggCount);
        UpdateEasterEggCount();
    }

    private void OnDisable()
    {
        EasterEggManager.OnEasterEggPickupUpdated.RemoveListener(UpdateEasterEggCount);
    }

    void UpdateEasterEggCount()
    {
        EasterEggManager easterEggManager = FindObjectOfType<EasterEggManager>();
        if (easterEggManager != null && easterEggManager.GetEasterEggCount() <= 10)
        {
            easterEggCountText.text = easterEggManager.GetEasterEggCount().ToString() + "/10";
        }
    }
}