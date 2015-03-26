using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    public enum WindowAnimationType { None, Fade, FadeSlow };

    public class WindowAnimation
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

        public static void animateIn(Drawable drawable, WindowAnimationType animation)
        {
            animateIn(drawable, animation, 30);
        }
        public static void animateIn(Drawable drawable, WindowAnimationType animation, int frames)
        {
            if (animation == WindowAnimationType.Fade)
                fadeIn(drawable);
            else if (animation == WindowAnimationType.FadeSlow)
                fadeIn(drawable, defaultFramesPerAnim * 4);
            else if (animation == WindowAnimationType.None)
                noneIn(drawable);
        }

        public static void animateOut(Drawable drawable, WindowAnimationType animation)
        {
            animateOut(drawable, animation, 30);
        }
        public static void animateOut(Drawable drawable, WindowAnimationType animation, int frames)
        {
            if (animation == WindowAnimationType.Fade)
                fadeOut(drawable);
            else if (animation == WindowAnimationType.FadeSlow)
                fadeOut(drawable, defaultFramesPerAnim * 4);
            else if (animation == WindowAnimationType.None)
                noneOut(drawable);
        }
    }
}
