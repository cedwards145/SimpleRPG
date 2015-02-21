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
        protected List<Battler> playerParty, combatants;
        protected List<AIBattler> enemyParty;
        protected Battler currentBattler;
        protected Queue<Battler> battleQueue;
        protected StateManager battleStateManager;
        protected List<Window> windows;
        protected List<TextWidget> widgets;
        protected List<MapObject> addedToMap;

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
            addedToMap = new List<MapObject>();

            enemyParty = new List<AIBattler>();
            enemyParty.Add(EnemyManager.getEnemy("Goblin"));
            enemyParty.Add(EnemyManager.getEnemy("Goblin"));
            enemyParty.Add(EnemyManager.getEnemy("Troll"));

            // Get the points to spawn MapObjects
            TileMap map = Player.getCurrentMap();
            Point playerPos = Player.getPlayerMapObject().getPosition();
            List<Point> battleSpace = map.walk(playerPos, 20);

            // Generate clusters
            List<Point>[] clusters = Clusterer.cluster(battleSpace);

            List<Point> playerSpace, enemySpace;

            if (clusters[0].Contains(playerPos))
            {
                playerSpace = clusters[0];
                enemySpace = clusters[1];
            }
            else
            {
                playerSpace = clusters[1];
                enemySpace = clusters[0];
            }

            // DEBUG 
            // tint possible tiles
            Color tintColor = Color.Green * 0.5f;
            foreach (Point p in playerSpace)
                map.tintTile(p.X, p.Y, tintColor);
            
            tintColor = Color.Red * 0.5f;
            foreach (Point p in enemySpace)
                map.tintTile(p.X, p.Y, tintColor);

            Random random = Utilities.getRandom();

            // Remove Player's character's location
            playerSpace.Remove(playerPos);

            // Place player party
            foreach (Battler battler in playerParty)
            {
                MapObject o = battler.getMapObject();
                // Check the battler doesn't already have a mapobject on the current map
                if (!o.isOnMap(map))
                {
                    map.addObject(o);
                    int positionIndex = random.Next(playerSpace.Count);
                    Point position = playerSpace[positionIndex];
                    o.setPosition(position.X, position.Y);
                    playerSpace.RemoveAt(positionIndex);
                    addedToMap.Add(o);
                }
            }

            // Create Enemy map objects
            foreach (Battler battler in enemyParty)
            {
                Point position = enemySpace[random.Next(enemySpace.Count)];
                enemySpace.Remove(position);
                MapObject o = new MapObject(game, "enemy", position.X, position.Y);
                o.face(playerPos);
                map.addObject(o);
                addedToMap.Add(o);
                battler.setMapObject(o);
            }

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
                stateManager.addState(new BattleCompleteState(gameRef, this, stateManager, this));

            foreach (Window window in windows)
            {
                window.update();
            }

            foreach (TextWidget widget in widgets)
            {
                widget.update();
            }

            if (currentBattler == null && battleStateManager.isEmpty() && !closing)
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
                // Cast each AIBattler in enemy party to a battler
                return enemyParty.ConvertAll(x => (Battler)x);
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

        public int getTotalExp()
        {
            int totalSoFar = 0;
            foreach (AIBattler battler in enemyParty)
                totalSoFar += battler.getExpEarned();

            return totalSoFar;
        }

        public override void exit()
        {
            base.exit();
            TileMap map = Player.getCurrentMap();
            foreach (MapObject mapObject in addedToMap)
                map.removeObject(mapObject);
        }
    }
}
