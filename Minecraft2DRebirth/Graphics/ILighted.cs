using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Graphics
{
    /// <summary>
    /// Defines a basic light source given a color and a size.
    /// </summary>
    public struct LightSource
    {
        /// <summary>
        /// Defines the X and Y position of the light.
        /// </summary>
        public Rectangle Size;

        /// <summary>
        /// Defines the color of the light.
        /// </summary>
        public Color Color;
    }

    /// <summary>
    /// Implement this class and its methods to render lighting in your scene.
    /// </summary>
    public interface ILighted
    {
        /// <summary>
        /// Whether or not to render the lights in the scene.
        /// </summary>
        bool RenderLights { get; set; }

        /// <summary>
        /// The <see cref="IEnumerable{LightSource}"/> of Lights to render in the scene.
        /// Where T is <see cref="LightSource"/>
        /// </summary>
        IEnumerable<LightSource> Lights { get; set; }

        /// <summary>
        /// The base scene, without any lighting, to render first.
        /// </summary>
        RenderTarget2D BaseScene { get; set; }

        /// <summary>
        /// The scene containing the lights, as rendered from the <see cref="Lights"/>
        /// </summary>
        RenderTarget2D LightScene { get; set; }
        
        /// <summary>
        /// The ambient light color for the scene.
        /// </summary>
        Color AmbientLight { get; set; }

        /// <summary>
        /// Draws.
        /// </summary>
        /// <param name="graphics"></param>
        void Draw(Graphics graphics);
    }
}
