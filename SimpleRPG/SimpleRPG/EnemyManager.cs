using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    public class EnemyManager
    {
        private static Dictionary<string, Battler> enemies = new Dictionary<string, Battler>();

        public static void addEnemy(Battler newEnemy)
        {
            enemies[newEnemy.getName()] = newEnemy;
        }

        public static Battler getEnemy(string enemyName)
        {
            if (enemies.ContainsKey(enemyName))
                return enemies[enemyName].clone();
            else
                return null;
        }

        public static void initialize()
        {
            addEnemy(new AIBattler("Goblin", 30, 0, 10, 2));
        }


    }
}
