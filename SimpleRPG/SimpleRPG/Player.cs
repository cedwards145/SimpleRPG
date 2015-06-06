using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.States;
using SimpleRPG.Items;
using Microsoft.Xna.Framework;
using SimpleRPG.Tilemap;

namespace SimpleRPG
{
    public class Player
    {
        protected static ItemContainer inventory = new ItemContainer();
        protected static List<Battler> party = new List<Battler>();

        protected static BattleState currentBattle;
        protected static bool inBattle;
        protected static bool menuAccess = true;

        public static void initialize(Game1 game)
        {
            PlayerBattler battler = new PlayerBattler("Mage", 6500, 3000, 200, 400, "player");
            party.Add(battler);

            battler = new PlayerBattler("Berserker", 9999, 0, 500, 200, "npc");
            party.Add(battler);
        }

        public static ItemContainer getInventory()
        {
            return inventory;
        }

        public static void giveItem(string itemName)
        {
            inventory.addItem(itemName);
        }

        public static void giveItem(Item item)
        {
            inventory.addItem(item);
        }

        public static void takeItem(string itemName)
        {
            inventory.removeItem(itemName);
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

        public static TileMap getCurrentMap()
        {
            MapObject playerObject = party[0].getMapObject();
            return playerObject.getContainingMap();
        }

        public static MapObject getPlayerMapObject()
        {
            return party[0].getMapObject();
        }

        public void emitLight(string lightName, Color color, bool flicker)
        {
            MapObject mapObject = party[0].getMapObject();
            mapObject.givesOffLight(lightName, color, flicker);
        }

        public void stopEmittingLight()
        {
            MapObject mapObject = party[0].getMapObject();
            mapObject.stopGivingOffLight();
        }

        #region Menu Access Methods

        public static bool canAccessMenu()
        {
            return menuAccess;
        }

        public static void setCanAccessMenu(bool value)
        {
            menuAccess = value;
        }

        #endregion

        #region Movement Access Methods

        public static bool canMove()
        {
            return party[0].getMapObject().ableToMove();
        }

        public static void setCanMove(bool value)
        {
            party[0].getMapObject().setCanMove(value);
        }

        #endregion
    }
}
