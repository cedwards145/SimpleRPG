using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Windows;
using SimpleRPG.Widgets;

namespace SimpleRPG.States
{
    public class BattleState :  GameState
    {
        protected List<Battler> playerParty, enemyParty, combatants;
        protected Battler currentBattler;
        protected Queue<Battler> battleQueue;
        protected StateManager battleStateManager;
        protected List<Window> windows;
        protected List<TextWidget> widgets;

        public BattleState(Game1 game, GameState parent, StateManager manager)
            :base(game, parent, manager)
        {
            // Set up state specific settings
            inAnimation = AnimationType.Fade;
            popOnEscape = false;

            widgets = new List<TextWidget>();

            // Add battle information to Player class
            Player.enterBattle(this);

            // Initialize battle state manager
            battleStateManager = new StateManager(game);

            // Set up the battle queue
            playerParty = Player.getParty();
            battleQueue = new Queue<Battler>();

            enemyParty = new List<Battler>();
            enemyParty.Add(EnemyManager.getEnemy("Goblin"));
            //enemyParty.Add(EnemyManager.getEnemy("Goblin"));
            //enemyParty.Add(EnemyManager.getEnemy("Goblin"));

            combatants = new List<Battler>();
            foreach (Battler battler in playerParty)
                combatants.Add(battler);
            foreach (Battler battler in enemyParty)
                combatants.Add(battler);
            foreach (Battler battler in combatants)
                battleQueue.Enqueue(battler);

            // Create windows for hp display
            int scale = gameRef.getGraphicsScale();
            int gameWidth = gameRef.getWidth();

            windows = new List<Window>();

            PlayerBattleStatusWindow statusWindow = new PlayerBattleStatusWindow(game, new Point(), "windowskin");
            statusWindow.setToBottom();
            statusWindow.setToLeft();
            windows.Add(statusWindow);

            EnemyBattleStatusWindow enemyWindow;
            for (int enemyIndex = 0; enemyIndex < enemyParty.Count; enemyIndex++)
            {
                Battler current = enemyParty[enemyIndex];
                enemyWindow = new EnemyBattleStatusWindow(game, new Point(game.getWidth() - (75 * scale), (5 + enemyIndex * 7) * scale), current);
                windows.Add(enemyWindow);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            int scale = gameRef.getGraphicsScale();
            int gameWidth = gameRef.getWidth();

            foreach (Window window in windows)
            {
                window.setOpacity(opacity);
                window.draw(spriteBatch);
            }

            foreach (TextWidget widget in widgets)
            {
                widget.setOpacity(opacity);
                widget.draw(spriteBatch);
            }

            battleStateManager.draw(spriteBatch);
        }

        public override void update()
        {
            base.update();
            battleStateManager.update();

            if (checkBattleEnd())
                stateManager.addState(new BattleCompleteState(gameRef, this, stateManager));

            foreach (Window window in windows)
            {
                window.update();
            }

            foreach (TextWidget widget in widgets)
            {
                widget.update();
            }

            if (currentBattler == null && battleStateManager.isEmpty())
                nextTurn();
        }

        /// <summary>
        /// Checks if the enemy party has any living members, and that there are no messages on screen
        /// </summary>
        /// <returns>Returns false if there are some living enemies, or true if the battle is done</returns>
        protected virtual bool checkBattleEnd()
        {
            bool allDead = true;
            foreach (Battler battler in enemyParty)
            {
                if (battler.isAlive())
                    allDead = false;
            }

            return allDead && battleStateManager.isEmpty();
        }

        protected void nextTurn()
        {
            currentBattler = battleQueue.Dequeue();
            while (!currentBattler.isAlive())
            {
                battleQueue.Enqueue(currentBattler);
                currentBattler = battleQueue.Dequeue();
            }
            currentBattler.takeTurn(this);
        }

        public StateManager getStateManager()
        {
            return battleStateManager;
        }

        public Game1 getGameRef()
        {
            return gameRef;
        }

        /// <summary>
        /// Gets the enemies of a specific battler
        /// </summary>
        /// <param name="battler">The battler whose enemies are wanted</param>
        /// <returns>The enemies of the specified battler</returns>
        public List<Battler> getEnemies(Battler battler)
        {
            if (playerParty.Contains(battler))
                return enemyParty;
            else
                return playerParty;
        }

        /// <summary>
        /// Gets the enemies of the current battler
        /// </summary>
        /// <returns>The enemies of the current battler</returns>
        public List<Battler> getEnemies()
        {
            return getEnemies(currentBattler);
        }

        public Battler getCurrentBattler()
        {
            return currentBattler;
        }

        public void showCombatResult(string result)
        {
            battleStateManager.clear();
            battleStateManager.addState(new MessageState(gameRef, null, battleStateManager, result));

            battleQueue.Enqueue(currentBattler);
            currentBattler = null;
        }

        public void showCombatResult(Battler attacker, Battler defender, int damage)
        {
            string result = string.Format("{0} hits {1} for {2} damage!", attacker.getName(), defender.getName(), damage);
            showCombatResult(result);
        }

        public void showCombatResult(CombatResult result)
        {
            NumberWidget widget = new NumberWidget(gameRef.getFont(), Color.White, new Vector2(100, 100), result.damageDealt);
            widgets.Add(widget);
            showCombatResult(result.ToString());
        }

        public List<Battler> getAllCombatants()
        {
            return combatants;
        }
    }
}
