using BBallMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using static Terraria.ModLoader.ModContent;

namespace BBallMod.Projectiles
{
	public class BBallChaosDunkProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Slam Jam");
		}

		public override void SetDefaults() {
			projectile.width = 40;
			projectile.height = 40;
            //projectile.timeLeft = -1;
            projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.magic = true;
            projectile.tileCollide = false;
            //projectile.hide = true;
            projectile.ai[0] = 0;
            projectile.ai[1] = -1;

            // These 2 help the projectile hitbox be centered on the projectile sprite.
            drawOffsetX = 5;
            drawOriginOffsetY = 5;
        }

		public override void AI() {
            Player player = Main.player[projectile.owner]; //get player

            if (projectile.ai[1] >= 0) projectile.ai[1] -= 1;

            if (player.channel) {
                player.GetModPlayer<BBallPlayer>().dunking = true;
                //projectile.hide = true;

                if (player.velocity.Y != 0f) {
                    player.velocity.Y += (0.5f * player.gravDir);   //only update fall speed if not on ground
                }
                projectile.Center = player.Center;

                //capture max fall speed
                if ((player.velocity.Y * player.gravDir) > projectile.ai[0]) {
                    projectile.ai[0] = (player.velocity.Y * player.gravDir);

                    //check for maxfallspeed
                    if (projectile.ai[0] >= player.maxFallSpeed) {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BBallDunkSiren"), player.position);
                    }
                }

                //check if dunked the ground
                if (player.GetModPlayer<BBallPlayer>().dunked) {
                    player.GetModPlayer<BBallPlayer>().dunked = false;
                    projectile.ai[1] = 3;   //set explosion to 3 frames

                    int baseRad = 3;

                    //set damage from max fall speed reached
                    if (projectile.ai[0] > player.maxFallSpeed) {
                        baseRad += 40;   //bigger radius for chaos dunk
                        projectile.damage = (int)player.maxFallSpeed * projectile.damage * 2; //chaos dunk
                    } else {
                        baseRad += (int) projectile.ai[0] / 10;
                        projectile.damage = (int)(projectile.ai[0] / 5) * projectile.damage;     //make damage proportional to fall speed
                    }

                    //resize projectile for explosion and set damage
                    projectile.alpha = 255;
                    projectile.position = projectile.Center;
                    projectile.width = (baseRad * 16 * 4);
                    projectile.height = projectile.width;
                    projectile.Center = projectile.position;
                    projectile.knockBack = 10f;
                }
            } else {
                player.GetModPlayer<BBallPlayer>().dunking = false;
                player.GetModPlayer<BBallPlayer>().dunked = false; //just in case youre not dunking but somehow dunked
                if (projectile.ai[1] < 0) projectile.Kill();  //kill projectile if not channelling and not already having dunked
            }

            if (projectile.ai[1] == 0) projectile.Kill();   //kill projectile on successful dunk

            UpdatePlayer(player);

			if (Main.rand.NextBool(3)) {
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Flame>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 100, Color.Orange, 1.9f);
            }
            if (Main.rand.NextBool(10)) {
               Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Smoke>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Player player = Main.player[projectile.owner]; //get player
            player.GetModPlayer<BBallPlayer>().dunked = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void OnHitPvp(Player target, int damage, bool crit) {
            Player player = Main.player[projectile.owner]; //get player
            player.GetModPlayer<BBallPlayer>().dunked = true;

            base.OnHitPvp(target, damage, crit);
        }

        public override void Kill(int timeLeft) {
            Player player = Main.player[projectile.owner]; //get player
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BBallDunkSiren").WithVolume(0f), player.position);

            //on a successful dunk
            if (projectile.ai[1] == 0) {
                if (projectile.ai[0] >= player.maxFallSpeed) {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BBallDunkExplosion").WithVolume(1f), player.position);
                }
                Explode();
            } else {
                //stopped channelling, fire a bball explosive?
                //explode on contact
            }

            player.GetModPlayer<BBallPlayer>().dunked = false;  //reset dunked flag

            base.Kill(timeLeft);
        }

        private void UpdatePlayer(Player player) {
            // Multiplayer support here, only run this code if the client running it is the owner of the projectile
            if (projectile.owner == Main.myPlayer) {
                Vector2 diff = Main.MouseWorld - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
            int dir = projectile.direction;
            player.ChangeDir(dir); // Set player direction to where we are shooting
            //player.heldProj = projectile.whoAmI; // Update player's held projectile
            player.itemTime = 1; // Set item time to 2 frames while we are used
            player.itemAnimation = 1; // Set item animation time to 2 frames while we are used
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir); // Set the item rotation to where we are shooting
        }

        private void Explode() {
            Player player = Main.player[projectile.owner]; //get player

            // Play explosion sound
            Main.PlaySound(SoundID.Item14, projectile.position);

            // Smoke Dust spawn
            for (int i = 0; i < 50; i++) {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 4), projectile.position.Y + (projectile.height / 4)), projectile.width / 2, projectile.height / 2, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }

            // Fire Dust spawn
            for (int i = 0; i < 80; i++) {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 4), projectile.position.Y + (projectile.height / 4)), projectile.width / 2, projectile.height / 2, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 4), projectile.position.Y + (projectile.height / 4)), projectile.width / 2, projectile.height / 2, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }

            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++) {
                int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (projectile.width / 2) - 24f, projectile.position.Y + (projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (projectile.width / 2) - 24f, projectile.position.Y + (projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (projectile.width / 2) - 24f, projectile.position.Y + (projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (projectile.width / 2) - 24f, projectile.position.Y + (projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }
            
            //how many blocks to explode
            int explosionRadius = projectile.width / 16 / 4;

            //find min/max x/y tiles in circle around projectile
            int minTileX = (int)(player.Center.X / 16f - (float)explosionRadius);
            int maxTileX = (int)(player.Center.X / 16f + (float)explosionRadius);
            int minTileY = (int)(player.Center.Y / 16f - (float)explosionRadius);
            int maxTileY = (int)(player.Center.Y / 16f + (float)explosionRadius);

            //clamp tile range inside world
            if (minTileX < 0) {
                minTileX = 0;
            }
            if (maxTileX > Main.maxTilesX) {
                maxTileX = Main.maxTilesX;
            }
            if (minTileY < 0) {
                minTileY = 0;
            }
            if (maxTileY > Main.maxTilesY) {
                maxTileY = Main.maxTilesY;
            }

            bool canKillWalls = false;

            //only get walls inside a circle
            for (int x = minTileX; x <= maxTileX; x++) {
                for (int y = minTileY; y <= maxTileY; y++) {
                    float diffX = Math.Abs((float)x - player.Center.X / 16f);
                    float diffY = Math.Abs((float)y - player.Center.Y / 16f);
                    double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distance < (double)explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].wall == 0) {
                        canKillWalls = true;
                        break;
                    }
                }
            }

            //only get tiles inside a circle
            for (int i = minTileX; i <= maxTileX; i++) {
                for (int j = minTileY; j <= maxTileY; j++) {
                    float diffX = Math.Abs((float)i - player.Center.X / 16f);
                    float diffY = Math.Abs((float)j - player.Center.Y / 16f);
                    double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distanceToTile < (double)explosionRadius) {
                        bool canKillTile = true;
                        if (Main.tile[i, j] != null && Main.tile[i, j].active()) {
                            canKillTile = true;
                            if (Main.tileDungeon[(int)Main.tile[i, j].type] || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 21 || Main.tile[i, j].type == 26 || Main.tile[i, j].type == 107 || Main.tile[i, j].type == 108 || Main.tile[i, j].type == 111 || Main.tile[i, j].type == 226 || Main.tile[i, j].type == 237 || Main.tile[i, j].type == 221 || Main.tile[i, j].type == 222 || Main.tile[i, j].type == 223 || Main.tile[i, j].type == 211 || Main.tile[i, j].type == 404) {
                                canKillTile = false;    //cant kill dungeon walls
                            }
                            if (!Main.hardMode && Main.tile[i, j].type == 58) {
                                canKillTile = false;    //cant kill ? walls
                            }
                            if (!TileLoader.CanExplode(i, j)) {
                                canKillTile = false;    //cant kill unexplodable walls
                            }
                            if (canKillTile) {
                                //kill tile
                                WorldGen.KillTile(i, j, false, false, false);
                                if (!Main.tile[i, j].active() && Main.netMode != 0) {
                                    NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                                }
                            }
                        }
                        if (canKillTile) {
                            for (int x = i - 1; x <= i + 1; x++) {
                                for (int y = j - 1; y <= j + 1; y++) {
                                    if (Main.tile[x, y] != null && Main.tile[x, y].wall > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile[x, y].wall)) {
                                        WorldGen.KillWall(x, y, false);
                                        if (Main.tile[x, y].wall == 0 && Main.netMode != 0) {
                                            NetMessage.SendData(17, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


    }
}