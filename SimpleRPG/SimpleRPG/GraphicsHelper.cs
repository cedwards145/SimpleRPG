using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SimpleRPG
{
    public class GraphicsHelper
    {
        public static int gameScreenWidth, gameScreenHeight;
        public static Game game;
        private static Texture2D oneByOne;

        /// <summary>
        /// Multiply blending
        /// </summary>
        public static BlendState Multiply = new BlendState()
        {
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero,
            ColorBlendFunction = BlendFunction.Add
        }; 

        public static void drawImageToBox(SpriteBatch spriteBatch, Texture2D image, Rectangle rect)
        {
            int width = rect.Width;
            int height = rect.Height;
            Vector2 position = new Vector2(rect.X, rect.Y);

            // Draw top left corner
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, image.Width / 3, image.Height / 3),
                             new Rectangle(0, 0, image.Width / 3, image.Height / 3), Color.White);
            // Draw top right
            spriteBatch.Draw(image, new Rectangle((int)position.X + width - (image.Width / 3), (int)position.Y, image.Width / 3, image.Height / 3),
                             new Rectangle((image.Width / 3) * 2, 0, image.Width / 3, image.Height / 3), Color.White);

            // Draw bottom left corner
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y + height - (image.Height / 3), image.Width / 3, image.Height / 3),
                             new Rectangle(0, (image.Height / 3) * 2, image.Width / 3, image.Height / 3), Color.White);

            // Draw bottom right corner
            spriteBatch.Draw(image, new Rectangle((int)position.X + width - (image.Width / 3), (int)position.Y + height - (image.Height / 3), image.Width / 3, image.Height / 3),
                             new Rectangle((image.Width / 3) * 2, (image.Height / 3) * 2, image.Width / 3, image.Height / 3), Color.White);

            // Draw top row
            spriteBatch.Draw(image, new Rectangle((int)position.X + (image.Width / 3), (int)position.Y, width - ((image.Width / 3) * 2), image.Height / 3),
                             new Rectangle(image.Width / 3, 0, image.Width / 3, image.Height / 3), Color.White);

            // Draw bottom row
            spriteBatch.Draw(image, new Rectangle((int)position.X + (image.Width / 3), (int)position.Y + height - (image.Height / 3), width - ((image.Width / 3) * 2), image.Height / 3),
                             new Rectangle(image.Width / 3, (image.Height / 3) * 2, image.Width / 3, image.Height / 3), Color.White);

            // Draw left edge
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y + (image.Height / 3), image.Width / 3, height - ((image.Height / 3) * 2)),
                             new Rectangle(0, image.Height / 3, image.Width / 3, image.Height / 3), Color.White);

            // Draw right edge
            spriteBatch.Draw(image, new Rectangle((int)position.X + width - (image.Width / 3), (int)position.Y + (image.Height / 3), image.Width / 3, height - ((image.Height / 3) * 2)),
                             new Rectangle((image.Width / 3) * 2, image.Height / 3, image.Width / 3, image.Height / 3), Color.White);

            // Draw inside
            spriteBatch.Draw(image, new Rectangle((int)position.X + (image.Width / 3), (int)position.Y + (image.Height / 3), width - ((image.Width / 3) * 2), height - ((image.Height / 3) * 2)),
                                   new Rectangle(image.Width / 3, image.Height / 3, image.Width / 3, image.Height / 3), Color.White);
        }

        public static Vector2 calculateCenterPosition(int objectWidth, int objectHeight)
        {
            return new Vector2((gameScreenWidth - objectWidth) / 2.0f, (gameScreenHeight - objectHeight) / 2.0f);
        }

        public static Point calculateCenterPositionP(int objectWidth, int objectHeight)
        {
            return new Point((gameScreenWidth - objectWidth) / 2, (gameScreenHeight - objectHeight) / 2);
        }

        public static Vector2 centerText(SpriteFont font, string text, Rectangle box)
        {
            Vector2 textSize = font.MeasureString(text);
            float x = (box.Width - textSize.X) / 2.0f;
            float y = (box.Height - textSize.Y) / 2.0f;

            return new Vector2(box.X + x, box.Y + y);
        }

        public static Vector2 centerTextP(SpriteFont font, string text, Rectangle box)
        {
            Vector2 textSize = font.MeasureString(text);
            float x = (box.Width - textSize.X) / 2.0f;
            float y = (box.Height - textSize.Y) / 2.0f;

            return new Vector2((int)(box.X + x), (int)(box.Y + y));
        }

        public static void setWidthHeight(int width, int height)
        {
            gameScreenWidth = width;
            gameScreenHeight = height;
        }

        public static void setGame(Game reqGame)
        {
            game = reqGame;
        }

        public static void setup()
        {
            oneByOne = new Texture2D(game.GraphicsDevice, 1, 1);
        }

        public static Texture2D Flip(Texture2D source, bool vertical, bool horizontal)
        {
            Texture2D flipped = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] flippedData = new Color[data.Length];

            source.GetData<Color>(data);

            for (int x = 0; x < source.Width; x++)
                for (int y = 0; y < source.Height; y++)
                {
                    int idx = (horizontal ? source.Width - 1 - x : x) + ((vertical ? source.Height - 1 - y : y) * source.Width);
                    flippedData[x + y * source.Width] = data[idx];
                }

            flipped.SetData<Color>(flippedData);

            return flipped;
        }

        public static string[] fitStringToBox(string text, SpriteFont font, Rectangle box)
        {
            string[] words = text.Split(' ');
            List<string> messages = new List<string>();

            string textSoFar = "";
            Vector2 size;
            for (int currentWord = 0; currentWord < words.Length; currentWord++)
            {
                // Attempt to include the next word on the same line
                string textWithoutAdjustments = textSoFar + words[currentWord];
                size = font.MeasureString(textWithoutAdjustments);

                // If the text is too wide, insert a linebreak and put the word on the next lined
                string textFittedHorizontally = textSoFar;
                if (size.X > box.Width)
                    textFittedHorizontally += "\n";

                textFittedHorizontally += words[currentWord] + " ";

                size = font.MeasureString(textFittedHorizontally);

                // Check that the message fits vertically in the target box
                if (size.Y > box.Height)
                {
                    // If the word does not fit in this box, add the text built so far as a complete message
                    // then start a new message with the current word
                    messages.Add(textSoFar);
                    textSoFar = words[currentWord] + " ";
                }
                else
                    // Otherwise, textFittedHorizontally also fits vertically, so it can be saved in 
                    // textSoFar
                    textSoFar = textFittedHorizontally;

                StringBuilder builder = new StringBuilder(textSoFar);
                if (builder[0] == '\n')
                    builder.Remove(0, 1);
                textSoFar = builder.ToString();
            }
            messages.Add(textSoFar);

            return messages.ToArray();
        }


        private static DrawableRectangle drawableRect;

        public static void fillRectangle(SpriteBatch sb, Rectangle rect, Color color)
        {
            if (drawableRect == null)
                drawableRect = new DrawableRectangle(rect, game);

            drawableRect.setRectangle(rect);
            drawableRect.fill(sb, color);
        }

        public static void drawRectangle(SpriteBatch sb, Rectangle rect, Color color)
        {
            drawRectangle(sb, rect, color, 1);
        }

        public static void drawRectangle(SpriteBatch sb, Rectangle rect, Color color, int width)
        {
            if (drawableRect == null)
                drawableRect = new DrawableRectangle(rect, game);

            drawableRect.setRectangle(rect);
            drawableRect.draw(sb, color, width);
        }

        public static void fillAngleRectangle(SpriteBatch sb, Rectangle rect, Color color)
        {
            for (int x = 0; x < rect.Height; x++)
            {
                fillRectangle(sb, new Rectangle(rect.X + rect.Height - x, rect.Y + x, rect.Width - rect.Height, 1), color);
            }
        }
    }
}
