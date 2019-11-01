using Terraria.ID;
using Terraria.ModLoader;

namespace BBallMod.Items
{
	public class BerserkSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Big Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Its more like a hunk of iron.");
		}

		public override void SetDefaults() 
		{
			item.damage = 350;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 15;
			item.value = 10000;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            item.useTurn = true;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}