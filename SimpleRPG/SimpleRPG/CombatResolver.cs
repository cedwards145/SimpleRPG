using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.States;
using Microsoft.Xna.Framework;

namespace SimpleRPG
{
    /// <summary>
    /// Simple class to handle combat, so that all calculations are done in one place
    /// </summary>
    public static class CombatResolver
    {
        /// <summary>
        /// Defines the maximum amount of damage it is possible to deal with a single blow
        /// </summary>
        public static readonly int MAX_DAMAGE = 999999;

        /// <summary>
        /// Handles one battler physically attacking another battler.
        /// If a BattleState is passed, this method will also advance a turn and display a result
        /// </summary>
        /// <param name="attacker">The Battler initiating the attack</param>
        /// <param name="defender">The Battler receiving the attack</param>
        /// <param name="battle">A BattleState representing the current battle. If null is passed, no battle actions will
        ///                      happen, combatants stats will simply change</param>
        public static void physicalAttack(Battler attacker, Battler defender, BattleState battle)
        {
            // Physical damage formula:
            // damageDealt = attackerStrength - defenderDefence
            // attackerStrength = attackerBaseStr +- 10%
            // defenderDefence = defenderBaseStr * 2/3

            Random r = Utilities.getRandom();

            float attackerStrength = attacker.getPower() * ((90 + r.Next(0, 20)) / 100f);
            float defenderDefence = defender.getPower() * (2f / 3f);

            int damageDealt = (int)(attackerStrength - defenderDefence);

            // However, make it impossible to deal 0 damage, so that it is never impossible to kill something
            // with physical attacks, just very hard. Allows monsters to still cause some amount of damage to players
            // even though the stat difference is huge

            damageDealt = (int)MathHelper.Clamp(damageDealt, 1, MAX_DAMAGE);

            // Deal damage
            defender.takeDamage(damageDealt);

            //  If a BattleState was passed to the method, report the action and advance turn
            if (battle != null)
            {
                CombatResult result = new CombatResult();
                result.attacker = attacker;
                result.defender = defender;
                result.actionPerformed = CombatResult.ActionPerformed.Attack;
                result.damageDealt = damageDealt;

                battle.showCombatResult(result);
            }
        }

        public static void useItemOn(Battler target, Items.UsableItem item, BattleState battle)
        {
            int damage = item.use(target);

            if (battle != null)
            {
                CombatResult result = new CombatResult();

                result.actionPerformed = CombatResult.ActionPerformed.Item;
                result.defender = target;
                result.itemUsed = item;
                result.damageDealt = damage;

                battle.showCombatResult(result);
            }
        }

    }
}
