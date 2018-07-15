using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Entity
{
    /// <summary>
    /// An entity that is basically a normal entity but can create light.
    /// Will define extra goodies like a light color, light offset, light size, etc.
    /// </summary>
    public interface IDynamicLightEntity : IAnimatedEntity
    {
        /// <summary>
        /// The color of the light said entity emits.
        /// </summary>
        Color LightColor { get; set; }

        /// <summary>
        /// The offset of the light relative to the entity.
        /// </summary>
        Vector2 LightOffset { get; set; }

        /// <summary>
        /// Size of the light.
        /// </summary>
        float LightSize { get; set; }
    }
}
