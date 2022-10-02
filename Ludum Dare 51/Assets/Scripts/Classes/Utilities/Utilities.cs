using UnityEngine;

namespace Murgn
{
    public static class Utilities
    {
        public static bool RandomChance(int chance = 50) => Random.Range(0, 100) < chance;
    }
}