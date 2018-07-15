using System;
using Microsoft.Xna.Framework;
using RockSolidEngine.Graphics;

namespace RockSolidEngine.Maps
{
    public class RockTile : ITile
    {
        public RockTile()
        {
        }

        public string SheetName { get; set; }
        public Vector2 TileIndex { get; set; }
        public TileTransparency Transparency { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 SpriteSize { get; set; }
        public string EntityName { get; set; }
        public Rectangle Hitbox { get; set; }
        public int Angle { get; internal set; }

        public float BottomSide()
        {
            throw new NotImplementedException();
        }

        public void Draw(Graphics.Graphics graphics)
        {
            
        }

        public float LeftSide()
        {
            throw new NotImplementedException();
        }

        public float RightSide()
        {
            throw new NotImplementedException();
        }

        public float TopSide()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
