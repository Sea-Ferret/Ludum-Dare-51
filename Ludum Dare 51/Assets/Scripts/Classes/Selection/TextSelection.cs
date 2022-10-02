using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn.UI
{
    public class TextSelection : SelectionBase
    {
        [SerializeField] private Image selectionArrow;
        [SerializeField] private Vector2 selectionArrowOffset;
        [SerializeField] private TextMeshProUGUI[] selectionItems;
        
        [Header("Item Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        
        private int oldSelectedItem;
        private List<SelectionItem> selectionItemClasses = new List<SelectionItem>();

        private bool isDisabled;

        private void Start()
        {
            normalColor = ColorManager.instance.chosenPalette.primaryColor;
            selectedColor = ColorManager.instance.chosenPalette.primaryColor;
            
            for (int i = 0; i < selectionItems.Length; i++)
            {
                SelectionItem item = selectionItems[i].GetComponent<SelectionItem>();
                if(item != null)
                    selectionItemClasses.Add(item);
                else
                    Debug.LogError($"GameObject <b>{selectionItems[i].gameObject.name}</b> doesn't have a <b>SelectionItem</b> component!");
            }
        }
        
        private new void Update()
        {
            base.Update();
            ItemInteraction();
            ItemGraphics();
        }
        
        protected override void ItemOverflow()
        {
            if (selectedItem < 0)
                selectedItem = selectionItems.Length - 1;

            if (selectedItem > selectionItems.Length - 1)
                selectedItem = 0;
        }

        protected override void ItemGraphics()
        {
            // Show selectedItem and arrow as selected
            selectionItems[selectedItem].color = isDisabled ? normalColor : selectedColor;
            selectionArrow.color = isDisabled ? normalColor : selectedColor;

            // Move selectionArrow
            selectionArrow.rectTransform.SetParent(selectionItems[selectedItem].rectTransform);
            selectionArrow.rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            selectionArrow.rectTransform.anchorMax = new Vector2(0.0f, 0.5f);
            selectionArrow.rectTransform.anchoredPosition = selectionArrowOffset;
            selectionArrow.rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Show oldSelectedItem as normal
            if (selectedItem != oldSelectedItem && oldSelectedItem >= 0)
                selectionItems[oldSelectedItem].color = normalColor;
            
            oldSelectedItem = selectedItem;
            
        }
        
        protected override void ItemInteraction()
        {
            if (InteractionDisable())
            {
                isDisabled = true;
                return;
            }
            else isDisabled = false;

            if (input.UI.Interact.WasPerformedThisFrame())
            {
                selectionItemClasses[selectedItem].interactActions.Invoke();
                audioManager.Play("UI/Interact", 0.1f);
            }
        }
    }   
}