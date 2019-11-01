using BBallMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BBallMod.Items.Weapons
{
	public class BBallStaff : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("BBall Energy");
			Tooltip.SetDefault("Conjure up the BBall Spirits to manifest a powerful sphere of BBall Energy.");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults() {
			item.damage = 40;
			item.magic = true;
			item.mana = 12;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileType<BBallStaffProjectile>();
			item.shootSpeed = 12f;
        }

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Texture2D texture = GetTexture("BBallMod/Items/Weapons/BBallStaffInv");
            Color color = Lighting.GetColor((int)(item.position.X + item.width * 0.5) / 16, (int)(item.position.Y + item.height * 0.5) / 16);
            Color alpha = item.GetAlpha(color);
            Rectangle rectangle = texture.Frame(1, 1, 0, Main.itemFrame[whoAmI]);
            float num6 = (item.width / 2 - rectangle.Width / 2);
            float num5 = (item.height - rectangle.Height);
            float num4 = item.velocity.X * 0.2f;
            
            Main.spriteBatch.Draw(texture, new Vector2(item.position.X - Main.screenPosition.X + (rectangle.Width / 2) + num6, item.position.Y - Main.screenPosition.Y + (rectangle.Height / 2) + num5), new Rectangle?(rectangle), alpha, num4, rectangle.Size() / 2f, scale, SpriteEffects.None, 0f);

            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            spriteBatch.Draw(GetTexture("BBallMod/Items/Weapons/BBallStaffInv"), position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);

            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}