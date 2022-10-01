using System;
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
        #endregion
    }
}