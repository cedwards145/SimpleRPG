using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Items;
using SimpleRPG.States;

namespace SimpleRPG
{
    /// <summary>
    /// Class representing a player controlled Battler
    /// </summary>
    public class PlayerBattler : Battler
    {
        /// <summary>
        /// The Battler's currently equipped weapon
        /// </summary>
        protected Weapon weapon;

        /// <summary>
        /// The Battler's currently equipped weapon
        /// </summary>
        protected Armour armour;

        public PlayerBattler(string reqName, int reqMaxHP, int reqMaxMP, int reqPower, int reqWill)
            : base(reqName, reqMaxHP, reqMaxMP, reqPower, reqWill)
        { }

        public PlayerBattler()
            : base()
        { }

        public override void takeTurn(BattleState battle)
        {
            StateManager manager = battle.getStateManager();
            Game1 gameRef = battle.getGameRef();

            BattleActionSelectState state = new BattleActionSelectState(gameRef, null, manager, battle);
            manager.addState(state);
        }

        public override Battler clone()
        {
            PlayerBattler clone = new PlayerBattler();

            clone.hp = hp;
            clone.mp = mp;
            clone.currentHP = hp;
            clone.currentMP = mp;
            clone.power = power;
            clone.will = will;
            clone.name = name;

            return clone;
        }

        public override int calculateDamage()
        {
            int baseDamage = base.calculateDamage();

            if (weapon != null)
                baseDamage += weapon.getBaseDamage();

            return baseDamage;
        }

        public override int defend()
        {
            int baseDefence = base.defend();

            if (armour != null)
                baseDefence += armour.getBaseDefence();

            return baseDefence;
        }

    }
}
