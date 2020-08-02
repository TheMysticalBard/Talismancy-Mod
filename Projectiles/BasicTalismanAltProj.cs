using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Talismancy.Projectiles
{
    class BasicTalismanAltProj : TalismanBaseAltProj
    {
        //This is how many frames are in the sprite that correspond to it's movement. The rest are for flipping over once the trap has been activated.
        private const int _mainFrames = 18;
        private const int _glowFrames = 12;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 22;
        }

        public override void SetDefaults()
        {
            //Will probably move some stuff to the basic projectile later.
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            //A value of 1 means the AI is currently flying, a value of 2 means it is a readied trap.
            projectile.ai[1] = 1;

            drawOffsetX = -24;
            drawOriginOffsetX = 12;
            drawOriginOffsetY = -4;
        }

        public override void AI()
        {
            if (!activeTrap)
            {
                //Movement
                projectile.ai[0] += 1;
                //Isn't affected by gravity for the first 20 frames, to better hit ground targets from the ground.
                if (projectile.ai[0] >= 20)
                {
                    projectile.ai[0] = 10;
                    projectile.velocity.Y += 0.1f;
                }

                //Clamp vertical velocity.
                if (projectile.velocity.Y >= 20f)
                {
                    projectile.velocity.Y = 20f;
                }

                //Animation
                projectile.frame = ++projectile.frame % _mainFrames;

                //Making sure projectile is facing correct direction
                projectile.rotation = projectile.velocity.ToRotation();

                //This is so the AI knows what state was in the previous AI step.
                projectile.ai[1] = 1;
            }
            else
            {
                //If the projectile was not an active trap last turn, reset the ai fields.
                if(projectile.ai[1] == 1)
                {
                    projectile.ai[0] = 0;
                    projectile.ai[1] = 2;
                }

                //Keep velocity at 0.
                projectile.velocity.X = 0;
                projectile.velocity.Y = 0;
                projectile.timeLeft = 2;

                //Now keeps track of how long (in frames) the projectile has been set as a trap.
                projectile.ai[0]++;

                //Increases frame every 4 AI calls (every 4 in-game frames) to the last frame, then stay there.
                projectile.frame = Math.Min((int)(Math.Floor(projectile.ai[0]/6)) + 18, 21);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //TODO: Apply curse to hit NPC. If trap hasn't been active long, light curse. (Rewards more anticipatory trap placement)

            //Make it so when an active trap hits an NPC it becomes inactive, so the player may spawn more traps.
            activeTrap = false;
        }

        public override void Kill(int timeLeft)
        {
            activeTrap = false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //If the card has flipped over, then we will finish the animation. This will only be true when the trap is set, so we don't have to worry about that.
            if(projectile.frame == 21)
            {
                Texture2D glowTexture = mod.GetTexture("Projectiles/BasicTalismanAltProj_Glow");
                int glowFrameHeight = glowTexture.Height / _glowFrames;
                int glowFrame = Math.Min((int)(projectile.ai[0]-21) / 10, 11);
                int startHeight = glowFrameHeight * glowFrame;
                Rectangle sourceRectangle = new Rectangle(0, startHeight, glowTexture.Width, glowFrameHeight);
                Vector2 origin = sourceRectangle.Size() * 0.5f;
                origin.X -= drawOffsetX * 0.5f;
                spriteBatch.Draw
                (
                    glowTexture,
                    projectile.Center - Main.screenPosition,
                    sourceRectangle,
                    Color.White,
                    projectile.rotation,
                    origin,
                    projectile.scale,
                    SpriteEffects.None,
                    0f
                ) ;
            }
        }
    }
}
