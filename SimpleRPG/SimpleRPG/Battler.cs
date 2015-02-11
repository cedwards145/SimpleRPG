using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SimpleRPG.States;

namespace SimpleRPG
{
    public class Battler
    {
        public static int DAMAGE_SCALING = 1;
        public static Random random = new Random();

        protected int hp, currentHP;
        protected int mp, currentMP;
        protected int power, will;

        protected string name;

        protected MapObject mapObject = null;

        public Battler()
            : this("", 0, 0, 0, 0)
        {  }

        public Battler(string reqName, int reqMaxHP, int reqMaxMP, int reqPower, int reqWill)
        {
            name = reqName;
            hp = reqMaxHP;
            currentHP = hp;
            mp = reqMaxMP;
            currentMP = mp;

            power = reqPower;
            will = reqWill;
        }

        // ACCESSOR METHODS
        public int getHP()
        {
            return currentHP;
        }
        public int getMaxHP()
        {
            return hp;
        }
        public int getMP()
        {
            return currentMP;
        }
        public int getMaxMP()
        {
            return mp;
        }
        public int getPower()
        {
            return power;
        }
        public int getWill()
        {
            return will;
        }
        public string getName()
        {
            return name;
        }
        public MapObject getMapObject()
        {
            return mapObject;
        }

        protected virtual bool isCrit()
        {
            return random.Next(100) > 90;
        }

        public bool isAlive()
        {
            return currentHP > 0;
        }

        public virtual int calculateDamage()
        {
            int criticalModifier = (isCrit() ? 2 : 1);
            double randomModifier = random.Next(30) + 85;
            randomModifier /= 100;
            
            return (int)(power * DAMAGE_SCALING * criticalModifier * randomModifier);
        }

        public virtual int defend()
        {
            return (int)(power * 2/3 * DAMAGE_SCALING);
        }

        /// <summary>
        /// Causes a Battler to directly lose a given amount of damage from their HP
        /// </summary>
        /// <param name="damage">Amount to subtract from the Battler's current HP</param>
        public virtual void takeDamage(int damage)
        {
            currentHP = (int)MathHelper.Clamp(currentHP - damage, 0, hp);
        }

        public virtual Battler clone()
        {
            Battler clone = new Battler();

            clone.hp = hp;
            clone.mp = mp;
            clone.currentHP = hp;
            clone.currentMP = mp;
            clone.power = power;
            clone.will = will;
            clone.name = name;

            return clone;
        }

        public virtual void takeTurn(BattleState battle)
        { }

        /// <summary>
        /// Causes this battler to physically attack a given other battler
        /// </summary>
        /// <param name="other">The target battler that this battler will attack</param>
        /// <returns>The amount of damage the target actually suffered</returns>
        public int physicalAttack(Battler other)
        {
            int damage = calculateDamage();
            int defence = other.defend();
            
            int resultantDamage = (int)MathHelper.Clamp(damage - defence, 1, damage);
            other.takeDamage(resultantDamage);

            return resultantDamage;
        }

        public void addHP(int value)
        {
            currentHP = (int)MathHelper.Clamp(currentHP + value, 0, hp);
        }

        public void addMP(int value)
        {
            currentMP = (int)MathHelper.Clamp(currentMP + value, 0, mp);
        }

        public void setMapObject(MapObject o)
        {
            mapObject = o;
        }
    }
}
