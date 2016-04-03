using Microsoft.Xna.Framework;

namespace Minecraft2DRebirth.Maps
{
    // TODO: scripting.

    /// <summary>
    /// Defines a default air block.
    /// </summary>
    public class MinecraftBlock : IMinecraftBlock
    {
        private string _Drops;
        public string Drops
        {
            get
            {
                return this.EntityName;
            }

            set
            {
                _Drops = value;
            }
        }

        public Vector2 SpriteSize { get; set; }

        public float Hardness { get; set; }

        public bool InBackground {get; set;}

        public string EntityName { get; set; }

        public string PlaceSoundName { get; set; }

        public Vector2 Position { get; set; }

        public ToolType PreferredTool { get; set; }

        public string SheetName { get; set; }

        public Vector2 TileIndex { get; set; }

        public TileTransparency Transparency { get; set; }

        public MinecraftBlock()
        {
            SheetName = null;
            Position = new Vector2(0, 0);
            PlaceSoundName = null;
            PreferredTool = ToolType.None;
            Transparency = TileTransparency.FullyTransparent; //can pass through
            TileIndex = new Vector2(-1, -1);
            EntityName = "minecraft:air";
            InBackground = false;
            Hardness = -1;
            Drops = null;
        }

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle
                {
                    X = (int)Position.X,
                    Y = (int)Position.Y,
                    Width = Constants.TileSize,
                    Height = Constants.TileSize
                };
            }
            set { }
        }

        public float LeftSide() { return Position.X; }
        public float RightSide() { return Position.X + Constants.TileSize; }
        public float TopSide() { return Position.Y; }
        public float BottomSide() { return Position.Y + Constants.TileSize; }

        public virtual void Draw(Graphics.Graphics graphics) { }

        public virtual void Update(GameTime gameTime) { }
    }
}
