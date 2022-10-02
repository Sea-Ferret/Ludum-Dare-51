using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn
{
    public class EventManager
    {
        #region ColorManager
        
        public static Action<SpriteRenderer> SpriteSetPrimary;
        public static Action<SpriteRenderer> SpriteSetSecondary;
        public static Action<Image> ImageSetPrimary;
        public static Action<Image> ImageSetSecondary;
        public static Action<TextMeshProUGUI> TextSetPrimary;
        public static Action<TextMeshProUGUI> TextSetSecondary;
        
        #endregion
        
        #region PlayerController

        public static Action<Vector2Int> PlayerMove;

        #endregion

        public static Action TimerMax;
    }
}