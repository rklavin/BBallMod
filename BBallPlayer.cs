using BBallMod.Dusts;
using BBallMod.Items;
using BBallMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace BBallMod
{
	public class BBallPlayer : ModPlayer {

        public float dunkstart;
        public bool dunking;
        public bool dunked;

        public override void ResetEffects() {
            dunking = false;

            base.ResetEffects();
        }

        public override void PreUpdate() {
            //get dunk distance
            float fall = (player.position.Y / 16f) - dunkstart;
            if (!dunking) fall = 0f;

            if (dunking) {
                player.noFallDmg = true;
                player.maxFallSpeed = 50f;

                if ((player.velocity.Y * player.gravDir) <= 0f) {
                    dunkstart = (player.position.Y / 16f);
                } else {
                }

                //check if dunked the ground
                if ((player.velocity.Y == 0f) && (fall > 10)) {
                    dunked = true;
                }
            } else {
                dunkstart = (player.position.Y / 16f);
                dunked = false;
            }

            base.PreUpdate();
        }

        public override void PostUpdate() {

            base.PostUpdate();
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
			ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if (damageSource.SourceOtherIndex == 0) {
                if (dunking) {
                    //damage = 0;
                }
            }

			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if (damageSource.SourceOtherIndex == 0) {
                damageSource = PlayerDeathReason.ByCustomReason(" FELL HARD"); 
            }
            if (damageSource.SourceOtherIndex == 8) {
				damageSource = PlayerDeathReason.ByCustomReason(" was dissolved by holy powers");
			}
			return true;
		}

		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			//add glow for dunk here?
		}
	}
}
