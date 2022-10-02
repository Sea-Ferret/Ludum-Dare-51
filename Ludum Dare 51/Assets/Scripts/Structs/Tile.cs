using UnityEngine;

namespace Murgn
{
    [System.Serializable]
    public struct Tile
    {
        public TileController tileController;
        public SpriteRenderer spriteRenderer;
        public bool enabled;
    }
}