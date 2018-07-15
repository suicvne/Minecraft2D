using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RockSolidEngine.Controls;
using RockSolidEngine.Native;
using System.Collections.Generic;

namespace RockSolidEngine.Screens
{
    public abstract class IScreen
    {
        public List<IControl> Controls = new List<IControl>();
        public void AddControl(IControl control)
        {
            Controls.Add(control);
            //TODO: auto de-select all other controls on selection of TextBox
        }

        /// <summary>
        /// Purely optional method for auto-magically drawing the <see cref="IControl"/>s in the <see cref="IScreen"/>
        /// </summary>
        /// <param name="graphics"></param>
        public void DrawControls(Graphics.Graphics graphics)
        {
            Controls.ForEach(x =>
            {
                x.Draw(graphics);
            });
        }

        /// <summary>
        /// Purely optional method for auto-magically updating the <see cref="IControl"/>s in the <see cref="IScreen"/>
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateControls(GameTime gameTime)
        {
            Controls.ForEach(x =>
            {
                x.UpdateCenterLocation(gameTime);
                x.Update(gameTime);
            });
        }

        /// <summary>
        /// Purely optional method for auto-magically drawing the mouse cursor.
        /// </summary>
        /// <param name="graphics"></param>
        public void DrawCursor(Graphics.Graphics graphics)
        {
            var destinationRect = new Rectangle(Minecraft2D.InputHelper.MousePosition.ToPoint(),
                new Point(32, 32));
            graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("cursor"), destinationRect, Color.White);
        }
        
        public abstract string ScreenName { get; internal set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(Graphics.Graphics graphics);
    }
}
