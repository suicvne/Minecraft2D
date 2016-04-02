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
                return this.Name;
            }

            set
            {
                _Drops = value;
            }
        }

        public float Hardness { get; set; }

        public bool InBackground {get; set;}

        public string Name { get; set; }

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
            Name = "minecraft:air";
            InBackground = false;
            Hardness = -1;
            Drops = null;
        }

        public Rectangle Bounds()
        {
            return new Rectangle
            {
                X = (int)Position.X,
                Y = (int)Position.Y,
                Width = Constants.TileSize,
                Height = Constants.TileSize
            };
        }

        public float LeftSide() { return Position.X; }
        public float RightSide() { return Position.X + Constants.TileSize; }
        public float TopSide() { return Position.Y; }
        public float BottomSide() { return Position.Y + Constants.TileSize; }

        public virtual void Draw(Graphics.Graphics graphics) { }

        public virtual void Update(GameTime gameTime) { }
    }
}
