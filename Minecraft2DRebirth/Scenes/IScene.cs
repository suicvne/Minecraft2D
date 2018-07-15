using Microsoft.Xna.Framework;
using RockSolidEngine.Entity;
using RockSolidEngine.Graphics;
using RockSolidEngine.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Scenes
{
    /// <summary>
    /// A scene is different from a screen.
    /// A screen is good for quick and dirty graphics display--drawing individual textures, some UI, whatever.
    /// 
    /// Scenes are like the complete package deal for the basics of any game -- you have entities in its most basic form. 
    /// Scenes are also aware of a parent screen.
    /// </summary>
    public interface IScene
    {
        // TODO: Define other properties like a background, music, etc.

        /// <summary>
        /// All of the entities in the scene.
        /// </summary>
        IEnumerable<IAnimatedEntity> Entities { get; set; }
        
        IScreen Parent { get; set; }

        Camera2D Camera { get; set; }

        void Draw(Graphics.Graphics graphics);
        void Update(GameTime gameTime);
    }
}
