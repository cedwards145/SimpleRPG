using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG.Quests
{
    /// <summary>
    /// Container for a player's quests
    /// </summary>
    public static class QuestLog
    {
        private static List<Quest> quests = new List<Quest>();

        public static void addQuest(Quest newQuest)
        {
            quests.Add(newQuest);
        }

        public static void removeQuest(Quest toRemove)
        {
            quests.Remove(toRemove);
        }

        public static List<Quest> getQuests()
        {
            return quests;
        }

        public static int getNumberOfQuests()
        {
            return quests.Count;
        }

    }
}
