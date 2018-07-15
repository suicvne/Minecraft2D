using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RockSolidEngine.Entity;
using RockSolidEngine.Graphics;
using RockSolidEngine.Screens;

namespace RockSolidEngine.Scenes
{
    public class BasicLightableScene : ILightableScene
    {
        #region Header implementation
        public Camera2D Camera { get; set; }
        private Color _AmbientLight = Color.White; //fully lit.
        public Color AmbientLight
        {
            get{return _AmbientLight;}
            set{ _AmbientLight = value; }
        }

        private RenderTarget2D _BaseScene;
        public RenderTarget2D BaseScene
        {
            get{return _BaseScene;}
            set{ _BaseScene = value; }
        }

        private List<IAnimatedEntity> _Entities;
        public IEnumerable<IAnimatedEntity> Entities
        {
            get{return _Entities;}
            set { _Entities = (List<IAnimatedEntity>)value; }
        }

        public IEnumerable<LightSource> Lights
        {
            get
            {
                return StaticLights;
            }
            set
            {
                StaticLights = value;
            }
        }

        private RenderTarget2D _LightScene;
        public RenderTarget2D LightScene
        {
            get { return _LightScene; }

            set { _LightScene = value; }
        }

        private IScreen _Parent;
        public IScreen Parent
        {
            get{ return _Parent; }
            set{ _Parent = value; }
        }

        private bool _RenderLights = true;
        public bool RenderLights
        {
            get { return _RenderLights; }
            set { _RenderLights = value; }
        }

        private List<LightSource> _StaticLights;
        public IEnumerable<LightSource> StaticLights
        {
            get { return _StaticLights; } set { _StaticLights = (List<LightSource>)value; }
        }
        #endregion

        public void AddEntity(IAnimatedEntity entity)
        {
            _Entities.Add(entity);
        }

        public void AddStaticLight(LightSource light)
        {
            _StaticLights.Add(light);
        }

        public BasicLightableScene(Graphics.Graphics graphics)
        {
            int width, height;
            width = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width;
            height = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height;

            Camera = new Camera2D();

            LightScene = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
                width, height);
            BaseScene = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
                width, height);

            _Entities = new List<IAnimatedEntity>();
            _StaticLights = new List<LightSource>();

            if (!Minecraft2D.ScaleGame)
            {
                graphics.ResolutionChanged += (sender, e) =>
                {
                    Console.WriteLine("[BasicLightableScene] Destroying and recreating render targets.");

                    width = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width;
                    height = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height;
                    LightScene.Dispose();
                    BaseScene.Dispose();

                    LightScene = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
                        width, height);
                    BaseScene = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
                        width, height);
                };
            }
        }

        /// <summary>
        /// Draws the base, fullylit scene.
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawBaseScene(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(BaseScene);
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.White);
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone,
                transformMatrix: Camera.Transformation(graphics.GetGraphicsDeviceManager().GraphicsDevice));

            //Draws the regular entites and their sprites and whatnot.
            _Entities.ForEach(entity =>
            {
                entity.Draw(graphics);
            });

            graphics.GetSpriteBatch().End();

            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Draws the full lights.
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawLights(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(LightScene);
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(AmbientLight);
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone,
                transformMatrix: Camera.Transformation(graphics.GetGraphicsDeviceManager().GraphicsDevice));

            var texture = graphics.GetTexture2DByName("circle");

            _StaticLights.ForEach(light =>
            {
                graphics.GetSpriteBatch().Draw(
                    texture, light.Size, light.Color
                );
            });

            //The dynamic lights
            foreach(var entity in _Entities.Where(x=>x is IDynamicLightEntity))
            {
                if(entity is IDynamicLightEntity)
                {
                    var entityAsLight = (IDynamicLightEntity)entity;
                    var drawingPoint = entityAsLight.Position.ToRectangle();
                    drawingPoint.Width = texture.Width;
                    drawingPoint.Height = texture.Height;
                    if(entityAsLight.LightSize != 1.0f)
                    {
                        drawingPoint.Width = (int)(drawingPoint.Width * entityAsLight.LightSize);
                        drawingPoint.Height = (int)(drawingPoint.Height * entityAsLight.LightSize);
                    }
                    
                    if (entityAsLight.LightOffset != null)
                    {
                        drawingPoint.X += (int)(entityAsLight.LightOffset.X - (drawingPoint.Width / 2));
                        drawingPoint.Y += (int)(entityAsLight.LightOffset.Y - (drawingPoint.Height / 2));
                    }

                    graphics.GetSpriteBatch().Draw(texture, drawingPoint, entityAsLight.LightColor);
                }
            }

            graphics.GetSpriteBatch().End();

            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }

        public void Draw(Graphics.Graphics graphics)
        {
            DrawBaseScene(graphics);
            DrawLights(graphics);

            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.White);
            graphics.GetSpriteBatch().Begin(blendState: Lighted.Multiply, samplerState: SamplerState.PointClamp);

            var screenRect = graphics.ScreenRectangle();
            graphics.GetSpriteBatch().Draw(BaseScene, graphics.ScreenRectangle(), Color.White);

            if(RenderLights)
                graphics.GetSpriteBatch().Draw(LightScene, graphics.ScreenRectangle(), Color.White);

            graphics.GetSpriteBatch().End();
        }

        public void Update(GameTime gameTime)
        {
            _Entities.ForEach(entity => entity.Update(gameTime));
        }
    }
}
