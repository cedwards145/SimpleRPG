using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.States;
using SimpleRPG.Items;

namespace SimpleRPG
{
    public class Player
    {
        
        protected static ItemContainer inventory = new ItemContainer();
        protected static List<Battler> party = new List<Battler>();

        protected static BattleState currentBattle;
        protected static bool inBattle;

        public static void initialize(Game1 game)
        {
            PlayerBattler battler = new PlayerBattler("Mage", 100, 150, 12, 50);
            MapObject o = new MapObject(game, "player", 0, 0);
            battler.setMapObject(o);
            party.Add(battler);

            battler = new PlayerBattler("Berserker", 200, 0, 20, 20);
            o = new MapObject(game, "npc", 0, 0);
            //battler.setMapObject(o);
            party.Add(battler);
        }

        public static ItemContainer getInventory()
        {
            return inventory;
        }

        public static void giveItem(Item item)
        {
            inventory.addItem(item);
        }

        public static void takeItem(Item item)
        {
            inventory.removeItem(item);
        }

        public static List<Battler> getParty()
        {
            return party;
        }

        public static bool isInBattle()
        {
            return inBattle;
        }

        public static BattleState getBattle()
        {
            return currentBattle;
        }

        public static void enterBattle(BattleState state)
        {
            inBattle = true;
            currentBattle = state;
        }

        public static void exitBattle()
        {
            inBattle = false;
            currentBattle = null;
        }
    }
}
