using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemDescription;
    public GameObject description;
    public TextMeshProUGUI descriptionText;
    public GameObject upgradeCrownCost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
        descriptionText.text = itemDescription;
        upgradeCrownCost.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) { 
        description.SetActive(false);
        upgradeCrownCost.SetActive(false);
    }
}
