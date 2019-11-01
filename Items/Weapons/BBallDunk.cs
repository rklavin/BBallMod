using BBallMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BBallMod.Items.Weapons 
{
	public class BBallDunk : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("BBall Dunk");
			Tooltip.SetDefault("Gather all your BBall Energy into one Slam.");
            //Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

		public override void SetDefaults() {
			item.damage = 50;
			item.magic = true;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.channel = true; //to hold the item
            item.mana = 40;
			item.width = 30;
			item.height = 30;
			item.useTime = 1;
			item.useAnimation = 1;
            item.useStyle = 5;
            item.knockBack = 5;
			item.value = 10000;
			item.rare = 4;
			item.shoot = ProjectileType<BBallDunkProjectile>();
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}