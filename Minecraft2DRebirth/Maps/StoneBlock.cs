using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Graphics;

namespace Minecraft2DRebirth.Maps
{
    /// <summary>
    /// TODO Script this
    /// </summary>
    class StoneBlock : MinecraftBlock
    {
        private Rectangle Region { get; set; }
        public StoneBlock() : base()
        {
            EntityName = "minecraft:grass";
            InBackground = false;
            SheetName = "terrain";
            TileIndex = new Vector2(1 * Constants.SpriteSize, 3 * Constants.SpriteSize);
            Position = Vector2.Zero;
            PreferredTool = ToolType.Pickaxe;
            PlaceSoundName = "stone{0}";
            Transparency = TileTransparency.FullyOpague;
            Hardness = 2f;
            Drops = EntityName;

            Region = new Rectangle((int)TileIndex.X, (int)TileIndex.Y, Constants.SpriteSize, Constants.SpriteSize);
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            var texture = graphics.GetTexture2DByName(SheetName);
            graphics.GetSpriteBatch().Draw(
                texture, 
                Position.ToRectangle(),
                Region,
                Color.White
            );
        }

        public override void Update(GameTime gameTime)
        { 
        }
    }
}
