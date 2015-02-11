using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    public enum AnimationType { None, Fade, FadeSlow };

    public class Animation
    {
        private static int defaultFramesPerAnim = 10;

        // FADE FUNCTIONS
        public static void fadeIn(Drawable drawable)
        {
            fadeIn(drawable, defaultFramesPerAnim);
        }
        public static void fadeIn(Drawable drawable, int frames)
        {
            drawable.setOpacity(0);
            drawable.setOpacity(1, frames);
        }
        public static void fadeOut(Drawable drawable)
        {
            fadeOut(drawable, defaultFramesPerAnim);
        }
        public static void fadeOut(Drawable drawable, int frames)
        {
            drawable.setOpacity(0, frames);
        }

        // NO ANIMATION
        public static void noneIn(Drawable drawable)
        {
            drawable.setOpacity(1);
        }
        public static void noneOut(Drawable drawable)
        {
            drawable.setOpacity(0);
        }

        public static void animateIn(Drawable drawable, AnimationType animation)
        {
            animateIn(drawable, animation, 30);
        }
        public static void animateIn(Drawable drawable, AnimationType animation, int frames)
        {
            if (animation == AnimationType.Fade)
                fadeIn(drawable);
            else if (animation == AnimationType.FadeSlow)
                fadeIn(drawable, defaultFramesPerAnim * 4);
            else if (animation == AnimationType.None)
                noneIn(drawable);
        }

        public static void animateOut(Drawable drawable, AnimationType animation)
        {
            animateOut(drawable, animation, 30);
        }
        public static void animateOut(Drawable drawable, AnimationType animation, int frames)
        {
            if (animation == AnimationType.Fade)
                fadeOut(drawable);
            else if (animation == AnimationType.FadeSlow)
                fadeOut(drawable, defaultFramesPerAnim * 4);
            else if (animation == AnimationType.None)
                noneOut(drawable);
        }
    }
}
