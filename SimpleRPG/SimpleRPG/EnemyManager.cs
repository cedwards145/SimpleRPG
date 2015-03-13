using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    public class EnemyManager
    {
        private static Dictionary<string, AIBattler> enemies = new Dictionary<string, AIBattler>();

        public static void addEnemy(AIBattler newEnemy)
        {
            enemies[newEnemy.getName()] = newEnemy;
        }

        public static AIBattler getEnemy(string enemyName)
        {
            if (enemies.ContainsKey(enemyName))
                return (AIBattler)enemies[enemyName].clone();
            else
                return null;
        }

        public static void initialize()
        {
            addEnemy(new AIBattler("Goblin", 30, 0, 15, 2, 120));
            addEnemy(new AIBattler("Troll", 70, 0, 20, 1, 300));
        }

    }
}
