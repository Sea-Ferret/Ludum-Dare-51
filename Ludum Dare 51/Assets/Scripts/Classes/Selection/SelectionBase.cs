using System;
using Murgn.Audio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Murgn.UI
{
    public enum SelectionDirection
    {
        Horizontal,
        Vertical
    }
    
    public abstract class SelectionBase : MonoBehaviour
    {
        [Header("Selection Setup")]
        [SerializeField] private SelectionDirection selectionDirection;
        [SerializeField] private int firstSelected;
        [SerializeField] private GameObject OnEnableInteractionDisable;

        [HideInInspector]
        public int selectedItem = -1;
        public UIInput input;
        
        [HideInInspector] public AudioManager audioManager;

        protected void Awake()
        {
            input = new UIInput();
            audioManager = AudioManager.instance;
        }

        private void OnEnable()
        {
            input.Enable();
            selectedItem = firstSelected;
        }
        
        private void OnDisable() 
        {
            input.Disable();
        }

        protected void Update()
        {
            ItemSelector();
            ItemGraphics();
            ItemInteraction();
        }

        private void ItemSelector()
        {
            if (InteractionDisable()) return;
            
            switch (selectionDirection)
            {
                case SelectionDirection.Horizontal:
                    float horizontal = input.UI.Horizontal.ReadValue<float>();

                    if (input.UI.Horizontal.WasPerformedThisFrame())
                    {
                        if (horizontal > 0)
                            selectedItem++;
                
                        else if (horizontal < 0)
                            selectedItem--;
                        ItemOverflow();
                        audioManager.Play("UI/Select", 0.1f);
                    }
                    return;
                
                case SelectionDirection.Vertical:
                    float vertical = input.UI.Vertical.ReadValue<float>();

                    if (input.UI.Vertical.WasPerformedThisFrame())
                    {
                        if (vertical < 0)
                            selectedItem++;
                
                        else if (vertical > 0)
                            selectedItem--;
                        
                        ItemOverflow();
                        audioManager.Play("UI/Select", 0.1f);
                    }
                    return;
            }
        }

        protected abstract void ItemOverflow();
        
        protected abstract void ItemGraphics();
        
        protected abstract void ItemInteraction();

        protected bool InteractionDisable()
        {
            if (OnEnableInteractionDisable != null && OnEnableInteractionDisable.activeSelf == true) 
                return true;
            
            return false;
        }
    }   
}