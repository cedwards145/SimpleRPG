using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    /// <summary>
    /// Class containing assorted useful references
    /// </summary>
    public static class Utilities
    {
        private static Random random;
        private static Game1 game;

        /// <summary>
        /// Must be called before any methods from this class can be used
        /// </summary>
        /// <param name="gameRef">Reference to the current Game object</param>
        public static void initialize(Game1 gameRef)
        {
            game = gameRef;
            random = new Random();
        }

        /// <summary>
        /// Access the current game
        /// </summary>
        /// <returns>Reference to the current Game object</returns>
        public static Game1 getGameRef()
        {
            return game;
        }

        /// <summary>
        /// Provides an initialized random number generator. Useful for cases when creating lots of
        /// Random objects would cause them to use identical seeds, and repeat numbers
        /// </summary>
        /// <returns>Random number generator</returns>
        public static Random getRandom()
        {
            return random;
        }
    }
}
