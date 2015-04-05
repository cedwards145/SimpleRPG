using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Items;
using SimpleRPG.States;
using Microsoft.Xna.Framework;

namespace SimpleRPG
{
    /// <summary>
    /// Class representing a player controlled Battler
    /// </summary>
    public class PlayerBattler : Battler
    {
        /// <summary>
        /// The maximum level a battler can reach
        /// </summary>
        public readonly int MAX_LEVEL = 100;

        /// <summary>
        /// The Power value a battler at maximum level will have
        /// </summary>
        protected int maxLevelPower;

        /// <summary>
        /// The Will value a battler at maximum level will have
        /// </summary>
        protected int maxLevelWill;

        /// <summary>
        /// The Max Health a battler at maximum level will have
        /// </summary>
        protected int maxLevelHP;

        /// <summary>
        /// The Max MP a battler at maximum level will have
        /// </summary>
        protected int maxLevelMP;

        /// <summary>
        /// The Battler's currently equipped weapon
        /// </summary>
        protected Weapon weapon;

        /// <summary>
        /// The Battler's currently equipped weapon
        /// </summary>
        protected Armour armour;

        /// <summary>
        /// The Battler's current level
        /// </summary>
        protected int level = 5;

        /// <summary>
        /// The amount of EXP a Battler has collected this level
        /// </summary>
        protected int exp;

        /// <summary>
        /// The amount of EXP a Battler needs to level up
        /// </summary>
        protected int expToNextLevel;

        public PlayerBattler(string reqName, int reqMaxHP, int reqMaxMP, int reqMaxPower, int reqMaxWill, string battlerImageName)
            : base(reqName, 0, 0, 0, 0)
        {
            mapObject = new MapObject(Utilities.getGameRef(), battlerImageName, 0, 0);
            mapObject.givesOffLight("light", Color.Yellow, true);

            exp = 0;
            expToNextLevel = 100;

            maxLevelHP = reqMaxHP;
            maxLevelMP = reqMaxMP;
            maxLevelPower = reqMaxPower;
            maxLevelWill = reqMaxWill;

            calculateStats();
        }

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
            clone.mapObject = mapObject;

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

        /// <summary>
        /// Gives a PlayerBattler a specified amount of EXP
        /// </summary>
        /// <param name="amount">The amount of EXP to give</param>
        /// <returns>Returns true if the Battler levels up from this</returns>
        public bool giveEXP(int amount)
        {
            exp += amount;
            return tryLevel();
        }

        public virtual bool tryLevel()
        {
            if (exp >= expToNextLevel)
            {
                // Calculate how much exp a battler has left after levelling
                int extraExp = expToNextLevel - exp;

                // Increase the battler's level
                level++;

                calculateStats();

                // Remove the exp used to level, and check if there is still enough exp to level again
                exp = extraExp;
                tryLevel();
                return true;
            }
            return false;
        }

        public int getLevel()
        {
            return level;
        }

        /// <summary>
        /// Gets the amount of EXP a battler has collected this level
        /// </summary>
        /// <returns>Current EXP earned</returns>
        public int getExp()
        {
            return exp;
        }

        /// <summary>
        /// Gets the total amount of EXP a battler needs to hit the next level
        /// </summary>
        /// <returns>Total EXP needed to level</returns>
        public int getExpToNextLevel()
        {
            return expToNextLevel;
        }

        /// <summary>
        /// Gets the difference between EXP to next level and current EXP
        /// </summary>
        /// <returns>EXP remaining until level up</returns>
        public int getRemainingExpToLevel()
        {
            return expToNextLevel - exp;
        }

        protected void calculateStats()
        {
            // Calculate y = (level) ^ 2, where level is shrank to fit between 0 and 10
            double x = 1 + (level / 20.0);
            x = Math.Pow(x, 2);

            // Calculate current (level) ^ 2 as a percentage of (MAX_LEVEL) ^ 2
            double percentage = x / Math.Pow(1 + (MAX_LEVEL / 20.0), 2);

            hp = (int)(maxLevelHP * percentage);
            mp = (int)(maxLevelMP * percentage);
            power = (int)(maxLevelPower * percentage);
            will = (int)(maxLevelWill * percentage);

            fullHeal();
        }
    }
}
