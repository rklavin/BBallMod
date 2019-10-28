using Terraria.ModLoader;
using Terraria.ID;

namespace BBallMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class BBallMask : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BBall Mask");
            Tooltip.SetDefault("BBall Mask.");
        }

        public override void SetDefaults() {
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.vanity = true;
		}

		public override bool DrawHead() {
			return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Topaz, 1);
            //recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}