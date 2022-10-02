using UnityEngine;

namespace Murgn
{
    public static class Utilities
    {
        public static bool RandomChance(int chance = 50) => Random.Range(0, 100) <= chance;
        
        public static float RoundToNearest(this float num, float roundTo)
        {
            if (roundTo == 0) return num;
            
            return num = num - (num % roundTo);
        }
    }
}