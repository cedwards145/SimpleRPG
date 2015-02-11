using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    /// <summary>
    /// A contained class to hold details about a turn of combat.
    /// All attributes are public, class is simply used to collect lots of different
    /// information between states
    /// </summary>
    public class CombatResult
    {
        public Battler attacker, defender;
        public enum ActionPerformed { Attack, Skill, Item };
        public ActionPerformed actionPerformed;
        public int damageDealt;
        public Items.Item itemUsed;

        public CombatResult()
        { }

        public override string ToString()
        {
            if (actionPerformed == ActionPerformed.Attack)
                return attacker.getName() + " hits " + defender.getName() + " dealing " + damageDealt + " damage";
            else if (actionPerformed == ActionPerformed.Item)
                return "Used " + itemUsed.getName() + " on " + defender.getName() +
                    (damageDealt > 0 ? " dealing " : " healing ") + damageDealt + " damage";
            else
                return "";
        }
        
    }
}
