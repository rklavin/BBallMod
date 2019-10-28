using BBallMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BBallMod.Projectiles
{
	public class BBallStaffProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Balls");
		}

		public override void SetDefaults() {
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = 400;
			projectile.penetrate = 15;
			projectile.friendly = true;
			projectile.magic = true;
			
			// These 2 help the projectile hitbox be centered on the projectile sprite.
			drawOffsetX = -5;
			drawOriginOffsetY = -5;
		}

		public override void AI() {
			//projectile.velocity.Y += projectile.ai[0];
            projectile.velocity.Y += 0.125f;

            // Rotation increased by velocity.X 
            projectile.rotation += projectile.velocity.X * 0.05f;
			
			if (Main.rand.NextBool(3)) {
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Flame>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 100, Color.Orange, 1.9f);
            }
            if (Main.rand.NextBool(10)) {
               Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Smoke>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (projectile.velocity.X != oldVelocity.X) {
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y) {
				projectile.velocity.Y = -oldVelocity.Y;
			}

			projectile.velocity *= 0.85f;

			if (projectile.soundDelay == 0) {
				Main.PlaySound(SoundID.Item10, projectile.position);
			}
			projectile.soundDelay = 10;
			return false;
		}

		public override void Kill(int timeLeft) {
			//for (int k = 0; k < 5; k++) {
			//	Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Sparkle>(), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
			//}
			//Main.PlaySound(SoundID.Item25, projectile.position);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			projectile.ai[0] += 0.1f;
			projectile.velocity *= 0.85f;
            projectile.velocity.X = -projectile.velocity.X;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            projectile.ai[0] += 0.1f;
            projectile.velocity *= 0.85f;
            projectile.velocity.X = -projectile.velocity.X;

            base.OnHitPvp(target, damage, crit);
        }
    }
}