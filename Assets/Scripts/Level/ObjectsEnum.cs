using UnityEngine;

namespace OSB.Editor
{
    public static class LevelSpawnSprites
    {
        public static Sprite GENERIC_SQUARE;
        public static Sprite GENERIC_CIRCLE;
        public static Sprite BOMB;

        /// <summary>
        /// CALL THIS AT THE BEGINNING OF EVERY LEVEL OR WHEN THE LEVEL EDITOR'S LOADED (ACTUALLY NOT ANYMORE)
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        public static void LoadSprites()
        {
            GENERIC_SQUARE = Resources.Load<Sprite>("Textures/LvlObjs/GenericSquare");
            GENERIC_CIRCLE = Resources.Load<Sprite>("Textures/LvlObjs/GenericCircle");
            BOMB = Resources.Load<Sprite>("Textures/LvlObjs/SpikeBomb");
        }
    }
}
