using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ColorManager : Singleton<ColorManager>
    {
        [SerializeField] private ColorPalette[] colorPalettes;
        [SerializeField] private SpriteRenderer[] primarySprites;
        [SerializeField] private Image[] primaryImages;
        [SerializeField] private TextMeshProUGUI[] primaryText;
        [SerializeField] private SpriteRenderer[] secondarySprites;
        [SerializeField] private Image[] secondaryImages;
        [SerializeField] private TextMeshProUGUI[] secondaryText;
        
        private Manager manager;
        public ColorPalette chosenPalette;

        private void OnEnable()
        {
            EventManager.SpriteSetPrimary += OnSpriteSetPrimary;
            EventManager.SpriteSetSecondary += OnSpriteSetSecondary;
            EventManager.ImageSetPrimary += OnImageSetPrimary;
            EventManager.ImageSetSecondary += OnImageSetSecondary;
            EventManager.TextSetPrimary += OnTextSetPrimary;
            EventManager.TextSetSecondary += OnTextSetSecondary;
        }
        
        private void OnDisable()
        {
            EventManager.SpriteSetPrimary -= OnSpriteSetPrimary;
            EventManager.SpriteSetSecondary -= OnSpriteSetSecondary;
            EventManager.ImageSetPrimary -= OnImageSetPrimary;
            EventManager.ImageSetSecondary -= OnImageSetSecondary;
            EventManager.TextSetPrimary -= OnTextSetPrimary;
            EventManager.TextSetSecondary -= OnTextSetSecondary;
        }

        public override void Awake()
        {
            base.Awake();
            manager = Manager.instance;
            chosenPalette = colorPalettes[Random.Range(0, colorPalettes.Length)];

            manager.mainCamera.backgroundColor = chosenPalette.secondaryColor;
            
            for (int i = 0; i < primarySprites.Length; i++)
                primarySprites[i].color = chosenPalette.primaryColor;
            for (int i = 0; i < primaryImages.Length; i++)
                primaryImages[i].color = chosenPalette.primaryColor;
            for (int i = 0; i < primaryText.Length; i++)
                primaryText[i].color = chosenPalette.primaryColor;
            
            for (int i = 0; i < secondarySprites.Length; i++)
                secondarySprites[i].color = chosenPalette.secondaryColor;
            for (int i = 0; i < secondaryImages.Length; i++)
                secondaryImages[i].color = chosenPalette.secondaryColor;
            for (int i = 0; i < secondaryText.Length; i++)
                secondaryText[i].color = chosenPalette.secondaryColor;
        }

        private void OnSpriteSetPrimary(SpriteRenderer spriteRenderer)
            => spriteRenderer.color = chosenPalette.primaryColor;
        
        private void OnSpriteSetSecondary(SpriteRenderer spriteRenderer)
            => spriteRenderer.color = chosenPalette.secondaryColor;
        
        private void OnImageSetPrimary(Image image)
            => image.color = chosenPalette.primaryColor;
        
        private void OnImageSetSecondary(Image image)
            => image.color = chosenPalette.secondaryColor;
        
        private void OnTextSetPrimary(TextMeshProUGUI text)
            => text.color = chosenPalette.primaryColor;
        
        private void OnTextSetSecondary(TextMeshProUGUI text)
            => text.color = chosenPalette.secondaryColor;
    }   
}