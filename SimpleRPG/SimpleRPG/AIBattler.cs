using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.States;

namespace SimpleRPG
{
    public class AIBattler : Battler
    {
        public AIBattler(string reqName, int reqMaxHP, int reqMaxMP, int reqPower, int reqWill)
            : base(reqName, reqMaxHP, reqMaxMP, reqPower, reqWill)
        { }

        public AIBattler()
            : base()
        { }

        public override void takeTurn(BattleState battle)
        {
            base.takeTurn(battle);
            List<Battler> enemies = battle.getEnemies(this);

            for (int enemyIndex = 0; enemyIndex < enemies.Count; enemyIndex++)
            {
                if (enemies[enemyIndex].isAlive())
                {
                    CombatResolver.physicalAttack(this, enemies[enemyIndex], battle);
                    
                    // int damageDealt = physicalAttack(enemies[enemyIndex]);
                    // battle.showCombatResult(string.Format("{0} hits {1} for {2} damage!", name, enemies[enemyIndex].getName(), damageDealt));
                    break;
                }
            }
        }

        public override Battler clone()
        {
            AIBattler clone = new AIBattler();

            clone.hp = hp;
            clone.mp = mp;
            clone.currentHP = hp;
            clone.currentMP = mp;
            clone.power = power;
            clone.will = will;
            clone.name = name;

            return clone;
        }

    }

}
