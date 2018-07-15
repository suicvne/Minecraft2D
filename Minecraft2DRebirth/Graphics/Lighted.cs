using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RockSolidEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Graphics
{
    public class Lighted : ILighted
    {
        /// <summary>
        /// The multiply blend state used for rendering lights properly in a scene.
        /// </summary>
        public static readonly BlendState Multiply = new BlendState
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero
        };

        private RenderTarget2D _BaseScene;
        public RenderTarget2D BaseScene
        {
            get { return _BaseScene; } set { _BaseScene = value; }
        }

        private List<LightSource> _Lights;
        public IEnumerable<LightSource> Lights
        {
            get { return _Lights; } set { _Lights = (List<LightSource>)value; }
        }

        private RenderTarget2D _LightScene;
        public RenderTarget2D LightScene
        {
            get { return _LightScene; } set { _LightScene = value; }
        }

        public bool RenderLights
        {
            get; set;
        } = true;

#if DEBUG
        public bool DrawLightAtCursor
        {
            get; set;
        } = true;
        public Color CursorLightColor
        {
            get; set;
        } = Color.White;
#endif

        private Color _AmbientLight;
        public Color AmbientLight { get { return _AmbientLight; } set { _AmbientLight = value; } }

        public Lighted(Graphics graphics)
        {
            int width, height;
            width = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width;
            height = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height;

            LightScene = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
                width, height);
            AmbientLight = Color.White; //fully lit by default.

            Lights = new List<LightSource>();

            graphics.ResolutionChanged += (sender, e) =>
            {
                Console.WriteLine($"[Lighted] Recreating lights target.");
                LightScene.Dispose();
                LightScene = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
                    graphics.ScreenRectangle().Width, graphics.ScreenRectangle().Height
                );
            };
        }

        //No need to render full scene first as that will be passed to us.
        //Lights will also be set.
        /// <summary>
        /// Draws the lights to an identical scene
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawLights(Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(_LightScene);
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(AmbientLight);

            graphics.GetSpriteBatch().Begin(blendState: BlendState.Additive);
            _Lights.ForEach(light =>
            {
                graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("circle"),
                    light.Size,
                    light.Color
                );
            });

#if DEBUG
            if (DrawLightAtCursor)
            {
                var point = Minecraft2D.InputHelper.MousePosition;
                point.X -= graphics.GetTexture2DByName("circle").Width / 2;
                point.Y -= graphics.GetTexture2DByName("circle").Height / 2;
                graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("circle"), point, CursorLightColor);
            }
#endif

            graphics.GetSpriteBatch().End();

            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }

        public void Draw(Graphics graphics)
        {
            throw new NotImplementedException("Use 2 null overloads instead.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="singleEntity">A single entity to act as a single use dynamic light.</param>
        /// <param name="entities">A list of dynamic lights.</param>
        public void Draw(Graphics graphics, IDynamicLightEntity singleEntity = null, IEnumerable<IDynamicLightEntity> entities = null)
        {
            if (BaseScene == null)
                throw new Exception("Valid fully lit scene was not given!");

            DrawLights(graphics);

            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.White);
            graphics.GetSpriteBatch().Begin(blendState: Multiply);

            graphics.GetSpriteBatch().Draw(BaseScene, graphics.ScreenRectangle(), Color.White);
            if(RenderLights)
                graphics.GetSpriteBatch().Draw(LightScene, graphics.ScreenRectangle(), Color.White);

            graphics.GetSpriteBatch().End();
        }

        public void DrawLightOnEntity(Graphics graphics, ref IDynamicLightEntity entity)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(_LightScene);
            
            graphics.GetSpriteBatch().Begin(blendState: BlendState.Additive);

            var lightDrawPoint = entity.Position.ToRectangle();
            if (entity.LightOffset != null) //offset the light
            {
                lightDrawPoint.X += (int)entity.LightOffset.X;
                lightDrawPoint.Y += (int)entity.LightOffset.Y;
            }
            if (entity.LightSize > 1.0f)
            {
                lightDrawPoint.Width = (int)(entity.LightSize * lightDrawPoint.Width);
                lightDrawPoint.Height = (int)(entity.LightSize * lightDrawPoint.Height);
            }
            graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("circle"),
                lightDrawPoint,
                entity.LightColor != null ? entity.LightColor : Color.White);

            graphics.GetSpriteBatch().End();
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }
    }
}
