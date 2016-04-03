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
        public StoneBlock() : base()
        {
            EntityName = "minecraft:stone";
            InBackground = false;
            SheetName = "terrain";
            TileIndex = new Vector2(0, 1 * Constants.SpriteSize);
            Position = Vector2.Zero;
            PreferredTool = ToolType.Pickaxe;
            PlaceSoundName = "stone{0}";
            Transparency = TileTransparency.FullyOpague;
            Hardness = 2f;
            Drops = EntityName;
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            var texture = graphics.GetTexture2DByName(SheetName);
            graphics.GetSpriteBatch().Draw(
                texture, 
                Position.ToRectangle(),
                new Rectangle((int)TileIndex.X, (int)TileIndex.Y, Constants.SpriteSize, Constants.SpriteSize),
                Color.White
            );
        }

        public override void Update(GameTime gameTime)
        { 
        }
    }
}
