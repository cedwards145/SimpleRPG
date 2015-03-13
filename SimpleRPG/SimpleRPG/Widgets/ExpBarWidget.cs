using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG.Widgets
{
    public class ExpBarWidget : BarWidget
    {
        protected PlayerBattler battler;
        protected int remainingExp;

        public ExpBarWidget(int width, int height, PlayerBattler reqBattler, int expGiven)
            : base (reqBattler.getExpToNextLevel(), 0, reqBattler.getExp(), width, height, 
                    Color.Black, ColorScheme.selectedTextColor)
        {
            battler = reqBattler;
            remainingExp = expGiven;

            resetBar();
        }

        public override void update()
        {
            base.update();

            // If bar is full, reset it
            if (fillValue >= upperValue)
            {
                resetBar();
            }
        }

        private void resetBar()
        {
            // Adjust bar size
            upperValue = battler.getExpToNextLevel();

            // Set value back to zero
            fillValue = battler.getExp();

            // Give battler next chunk of EXP
            int expToLevel = battler.getRemainingExpToLevel();

            // If there is enough EXP left to level up again, give the battler a level's worth of exp 
            if (remainingExp >= expToLevel)
            {
                // Give EXP
                battler.giveEXP(expToLevel);
                // Animate Bar
                setValue(fillValue + expToLevel, 30);

                // Calculate remaining exp
                remainingExp = remainingExp -= expToLevel;
            }
            // Otherwise, give battler all remaining exp
            else
            {
                battler.giveEXP(remainingExp);
                // Animate Bar
                setValue(remainingExp, 30);
                remainingExp = 0;
            }
        }

        public void giveRemainingEXP()
        {
            battler.giveEXP(remainingExp);
            setUpperValue(battler.getExpToNextLevel());
            setValue(battler.getExp());
        }
    }
}
