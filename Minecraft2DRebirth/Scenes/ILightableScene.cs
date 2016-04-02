using Minecraft2DRebirth.Entity;
using Minecraft2DRebirth.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Scenes
{
    /// <summary>
    /// To extend on scenes, the ILightableScene directly integrates with the ILighted interface to bring dynamic
    /// 2D lighting via blending modes to an IScene.
    /// </summary>
    public interface ILightableScene : IScene, ILighted
    {
        /// <summary>
        /// Defines static lights in the scene.
        /// </summary>
        IEnumerable<LightSource> StaticLights { get; set; }

        void AddStaticLight(LightSource light);
        void AddEntity(IEntity entity);

        /*
         Note to readers.

        Because ILightableScene implements the IScene, we have our entities list to peek at already.
        What we'll do is see if the class is of type IDynamicLightEntity for lighting stuff.

        We also already implement an update and draw method. So, we're good.
         */
    }
}
