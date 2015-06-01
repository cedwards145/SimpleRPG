using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleRPG
{
    public class Particle : Drawable
    {
        protected Vector2 position;
        protected Vector2 velocity;

        protected Texture2D lightTexture;
        protected Color lightColor;
        protected bool emitsLight;

        protected Texture2D texture;
        protected bool hasTexture;

        public Particle(bool reqEmitsLight, string reqLightTexture, Color reqColor,
                        bool reqHasTexture, string reqTexture,
                        Vector2 startPosition, Vector2 reqVelocity, int ttl)
        {
            Game1 gameRef = Utilities.getGameRef();

            emitsLight = reqEmitsLight;
            if (emitsLight)
            {
                lightTexture = gameRef.Content.Load<Texture2D>(@"graphics\" + reqLightTexture);
                lightColor = reqColor;
            }

            hasTexture = reqHasTexture;
            if (hasTexture)
                texture = gameRef.Content.Load<Texture2D>(@"graphics\" + reqTexture);

            position = startPosition;
            velocity = reqVelocity;

            setOpacity(0, ttl);
        }

        public override void update()
        {
            base.update();
            position += velocity;
        }

        public void drawLight(SpriteBatch spriteBatch)
        {
            if (emitsLight)
            {
                Camera camera = Utilities.getGameRef().getCamera();
                Vector2 drawPos = new Vector2(position.X - (lightTexture.Width / 2),
                                              position.Y - (lightTexture.Height / 2));
                int scale = Utilities.getGameRef().getGraphicsScale();
                drawPos.X *= scale;
                drawPos.Y *= scale;
                drawPos = camera.transformVector(drawPos);

                spriteBatch.Draw(lightTexture, new Rectangle((int)drawPos.X,
                                                             (int)drawPos.Y,
                                                             lightTexture.Width * scale,
                                                             lightTexture.Height * scale), lightColor * opacity);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            if (hasTexture)
            {
                Camera camera = Utilities.getGameRef().getCamera();
                Vector2 drawPos = new Vector2(position.X - (texture.Width / 2),
                                              position.Y - (texture.Height / 2));
                int scale = Utilities.getGameRef().getGraphicsScale();
                drawPos.X *= scale;
                drawPos.Y *= scale;
                drawPos = camera.transformVector(drawPos);

                spriteBatch.Draw(texture, new Rectangle((int)drawPos.X,
                                                             (int)drawPos.Y,
                                                             texture.Width * scale,
                                                             texture.Height * scale), Color.White * opacity);
            }
        }
    }
}
