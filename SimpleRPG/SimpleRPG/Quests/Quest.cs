using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleRPG.Items;

namespace SimpleRPG.Quests
{
    public class Quest
    {
        /// <summary>
        /// The name of the quest
        /// </summary>
        protected string name;

        /// <summary>
        /// A short description of the quest
        /// </summary>
        protected string description;

        /// <summary>
        /// The amount of EXP earned for completing this quest
        /// </summary>
        protected int expEarned;

        /// <summary>
        /// A list of items earned for completing this quest
        /// </summary>
        protected List<Item> rewards;

        /// <summary>
        /// The amount of money earned for completing this quest
        /// </summary>
        protected int moneyEarned;

        /// <summary>
        /// Allows for quests to be made up of multiple other quests
        /// </summary>
        protected List<Quest> subQuests = new List<Quest>();

        public Quest(string questName, string questDescription)
        {
            name = questName;
            description = questDescription;
        }

        public string getName()
        {
            return name;
        }

        public string getDescription()
        {
            return description;
        }

        public virtual bool isCompleted()
        {
            if (subQuests.Count == 0)
                return true;
            else
            {
                bool allComplete = true;
                foreach (Quest q in subQuests)
                    if (!q.isCompleted())
                        allComplete = false;
                return allComplete;
            }
        }
    }
}
