using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG
{
    public class Drawable
    {
        protected float opacity = 1;
        private float opacityStep = 0.0f;
        private float targetOpacity;

        public virtual void update()
        {
            opacity += opacityStep;
            if ((opacityStep > 0 && opacity > targetOpacity) ||
                (opacityStep < 0 && opacity < targetOpacity))
            {
                opacity = targetOpacity;
                opacityStep = 0;
            }
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {   }

        public void setOpacity(float value)
        {
            opacity = value;
            opacityStep = 0;
        }

        public void setOpacity(float value, int framesToFade)
        {
            targetOpacity = value;
            opacityStep = (value - opacity) / framesToFade;
        }

        public float getOpacity()
        {
            return opacity;
        }
    }
}
