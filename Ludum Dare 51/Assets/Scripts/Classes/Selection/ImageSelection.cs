using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Murgn.UI
{
    public class ImageSelection : SelectionBase
    {
        [SerializeField] private Image selectionArrow;
        [SerializeField] private Vector2[] selectionArrowOffset;
        [SerializeField] private Image[] selectionItems;
        [SerializeField] private Vector2[] selectionItemPositions;
        
        [Header("Item Settings")]
        [SerializeField] private Sprite normalPanel;
        [SerializeField] private Sprite selectedPanel;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Vector2 selectedOffset = new Vector2(0.0f, -1.0f);
        
        private int oldSelectedItem;
        private List<SelectionItem> selectionItemClasses = new List<SelectionItem>();

        private void Start()
        {
            // TODO: Since this is for every selection class, it may reduce peformance, need to think of a way to only 
            // TODO: call it once.
            //Canvas.ForceUpdateCanvases();
            
            for (int i = 0; i < selectionItems.Length; i++)
            {
                RectTransform rectTransform = (RectTransform)selectionItems[i].transform;
                //selectionItemPositions.Add(rectTransform.anchoredPosition);
                //Debug.LogError(rectTransform.anchoredPosition);
                
                SelectionItem item = selectionItems[i].GetComponent<SelectionItem>();
                if(item != null)
                    selectionItemClasses.Add(item);
                else
                    Debug.LogError($"GameObject <b>{selectionItems[i].gameObject.name}</b> doesn't have a <b>SelectionItem</b> component!");
            }
            selectionArrow.color = selectedColor;
        }
        
        private new void Update()
        {
            base.Update();
            ItemInteraction();
        }

        protected override void ItemOverflow()
        {
            // If selectedItem is less than 0, loop back to the end
            if (selectedItem < 0)
                selectedItem = selectionItems.Length - 1;

            // If selectedItem is more than the amount of items, loop back to the start
            if (selectedItem > selectionItems.Length - 1)
                selectedItem = 0;
        }

        protected override void ItemGraphics()
        {
            // Show selectedItem as selected
            selectionItems[selectedItem].sprite = selectedPanel;
            selectionItems[selectedItem].rectTransform.anchoredPosition = selectionItemPositions[selectedItem] + selectedOffset;

            // Move selectionArrow
            selectionArrow.rectTransform.SetParent(selectionItems[selectedItem].rectTransform);
            selectionArrow.rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            selectionArrow.rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            selectionArrow.rectTransform.anchoredPosition = selectionArrowOffset[selectedItem];
            selectionArrow.rectTransform.pivot = new Vector2(0.5f, 1.0f);
           
            // Show oldSelectedItem as normal
            if (selectedItem != oldSelectedItem && oldSelectedItem >= 0)
            {
                selectionItems[oldSelectedItem].sprite = normalPanel;
                selectionItems[oldSelectedItem].rectTransform.anchoredPosition = selectionItemPositions[oldSelectedItem] - selectedOffset;
            }
            
            oldSelectedItem = selectedItem;
        }

        protected override void ItemInteraction()
        {
            if (InteractionDisable()) return;

            if (input.UI.Interact.WasPerformedThisFrame())
            {
                selectionItemClasses[selectedItem].interactActions.Invoke();
                audioManager.Play("UI/Interact", 0.1f);
            }
        }
    }   
}