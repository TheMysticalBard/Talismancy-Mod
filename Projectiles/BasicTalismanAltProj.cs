using Terraria;
using Terraria.ModLoader;

namespace Talismancy.Projectiles
{
    class BasicTalismanAltProj : TalismanBaseAltProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            //Will probably move some stuff to the basic projectile later.
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            //A value of 1 means the AI is currently flying, a value of 2 means it is a readied trap.
            projectile.ai[1] = 1;
        }

        public override void AI()
        {
            if (!activeTrap)
            {
                //Movement
                projectile.ai[0] += 1;
                if (projectile.ai[0] >= 10)
                {
                    projectile.ai[0] = 10;
                    projectile.velocity.Y += 0.05f;
                }
                projectile.velocity.X *= 1.01f;

                //Animation
                if (++projectile.frameCounter >= 2)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = ++projectile.frame % Main.projFrames[projectile.type];
                }

                //Making sure projectile is facing correct direction
                projectile.rotation = projectile.velocity.ToRotation();

                //This is so the AI knows what state was in the previous AI step.
                projectile.ai[1] = 1;
            }
            else
            {
                if(projectile.ai[1] == 1)
                {
                    projectile.ai[0] = 0;
                    projectile.ai[1] = 2;
                }
                projectile.velocity.X = 0;
                projectile.velocity.Y = 0;
                projectile.frame = 2;
                projectile.timeLeft = 2;
                projectile.ai[0]++;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //TODO: Apply curse to hit NPC. If trap hasn't been active long, light curse. (Rewards more anticipatory trap placement)

            //Make it so when an active trap hits an NPC it becomes inactive, so the player may spawn more traps.
            activeTrap = false;
        }
    }
}
