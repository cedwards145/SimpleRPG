using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleRPG.Tilemap;

namespace SimpleRPG
{
    public class ParticleEmitter : MapObject
    {
        protected List<Particle> particles;

        protected int framesPerEmission = 1;
        protected int framesSinceEmission = 0;

        public ParticleEmitter(Game1 game, string textureName, int xCoord, int yCoord)
            :base(game, textureName, xCoord, yCoord)
        {
            particles = new List<Particle>();
            emitsLight = true;
        }

        public override void update()
        {
            base.update();

            if (framesSinceEmission == 0)
            {
                emitParticle();

            }

            framesSinceEmission++;
            if (framesSinceEmission >= framesPerEmission)
                framesSinceEmission = 0;

            foreach (Particle particle in particles)
            {
                particle.update();
            }
        }

        protected void emitParticle()
        {
            int tileSize = containingMap.getTileSize();
            Vector2 particlePosition = new Vector2(location.X * tileSize + (tileSize / 2),
                                                   location.Y * tileSize + (tileSize / 2));

            Random r = Utilities.getRandom();

            particles.Add(new Particle(true, "star", Color.Purple,
                                       false, "",
                                       particlePosition, 
                                       new Vector2((float)(1.5f - r.NextDouble()),
                                                                     (float)(1.5f - r.NextDouble())),
                                       (int)(300 * r.NextDouble())));
        }

        public override void draw(SpriteBatch spriteBatch, float opacity, Point mapOffset, int scale)
        {
            base.draw(spriteBatch, opacity, mapOffset, scale);

            foreach (Particle particle in particles)
            {
                particle.draw(spriteBatch);
            }
        }

        public override void drawLight(SpriteBatch spriteBatch, Point mapOffset)
        {
            //base.drawLight(spriteBatch);

            foreach (Particle particle in particles)
            {
                particle.drawLight(spriteBatch);
            }
        }
    }
}
