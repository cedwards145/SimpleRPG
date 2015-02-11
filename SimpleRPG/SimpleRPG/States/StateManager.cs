using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG.States
{
    public class StateManager
    {
        private List<GameState> gameStates;
        private List<GameState> toAdd;
        private List<GameState> toRemove;

        public StateManager(Game1 game)
        {
            gameStates = new List<GameState>();
            toAdd = new List<GameState>();
            toRemove = new List<GameState>();
        }

        // Adds the given state to the list of states
        public void addState(GameState newState)
        {
            toAdd.Add(newState);
        }

        // Removes the last added state from the list
        public GameState popState()
        {
            GameState removed = null;
            if (gameStates.Count > 0)
            {
                removed = gameStates[gameStates.Count - 1];
                toRemove.Add(removed);
            }

            return removed;
        }

        // Removes a given state from the list
        public GameState removeState(GameState state)
        {
            GameState removed = null;
            if (gameStates.Count > 0)
            {
                removed = state;
                toRemove.Add(removed);
            }
            return removed;
        }

        public void update()
        {
            if (gameStates.Count > 0)
                gameStates[gameStates.Count - 1].update();

            foreach (GameState state in toAdd)
            {
                gameStates.Add(state);
            }

            foreach (GameState state in toRemove)
            {
                gameStates.Remove(state);
            }

            toAdd.Clear();
            toRemove.Clear();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (GameState state in gameStates)
            {
                state.draw(spriteBatch);
            }
        }

        public void clear()
        {
            gameStates.Clear();
        }

        public bool isEmpty()
        {
            return gameStates.Count == 0;
        }
    }
}
