using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map
{
    public class Player
    {
        private Skin skin { get; set; }
        public Rectangle Hitbox { get; set; }
        public Vector2 Position { get; set; }
        public int Direction { get; set; }

        public Player()
        {
            Hitbox = new Rectangle(0, 0, 32, Convert.ToInt32(32 * 2.5));
            Position = new Vector2(22 * 32, 30 * 32);
            skin = new Skin(MainGame.CustomContentManager.GetTexture("default"));
            Direction = 0; //left
        }

        public void Move(Vector2 pos)
        {
            Position += pos;
        }

        public void Update(GameTime gameTime)
        { }

        public void Draw(GameTime gameTime)
        {
            
            /*MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));*/
            if (Direction == 0)
            {
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, new Rectangle(
                    (int)Position.X, (int)Position.Y, 24, 24
                    ), new Rectangle(skin.HeadLeft.X, skin.HeadLeft.Y, 8, 8), Color.White);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, 
                    new Rectangle((int)Position.X + 4, (int)Position.Y + 24, 6 * 3,  12 * 3), 
                    new Rectangle(skin.LeftTorso.X, skin.LeftTorso.Y, 6, 12), Color.White);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                    new Rectangle((int)Position.X + 8, (int)Position.Y + 24, 4 * 2, 12 * 2),
                    new Rectangle(skin.OutsideArm.X, skin.OutsideArm.Y, 4, 12), Color.White);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                    new Rectangle((int)Position.X + 8, (int)Position.Y + 60, 4 * 2, 12 * 2),
                    new Rectangle(skin.OutsideLeg.X, skin.OutsideLeg.Y, 4, 12), Color.White);
            }
            //MainGame.GlobalSpriteBatch.End();

        }
    }
}
