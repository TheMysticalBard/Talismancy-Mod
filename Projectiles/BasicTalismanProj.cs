using Terraria;
using Terraria.ModLoader;

namespace Talismancy.Projectiles
{
    class BasicTalismanProj : TalismanBaseProj
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
        }

        public override void AI()
        {
            //Movement
            projectile.ai[0] += 1;
            if(projectile.ai[0] >= 20)
            {
                projectile.ai[0] = 10;
                projectile.velocity.Y += 0.1f;
            }
            projectile.velocity.X *= projectile.velocity.Y < -1f || projectile.ai[0] < 20 ? 1.01f : 1.0f;
            if(projectile.velocity.Y >= 20f)
            {
                projectile.velocity.Y = 20f;
            }

            //Animation
            if(++projectile.frameCounter >= 2)
            {
                projectile.frameCounter = 0;
                projectile.frame = ++projectile.frame % Main.projFrames[projectile.type];
            }

            //Making sure projectile is facing correct direction
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}
