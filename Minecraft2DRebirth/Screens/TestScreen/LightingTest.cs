using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RockSolidEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RockSolidEngine.Screens.TestScreen
{
    struct QuadLightSource
    {
        public Rectangle rectangle;
        public Color color;


        private static Random rng = new Random((int)DateTime.Now.Ticks);

        public static QuadLightSource MakeWithRandomColor(Point point)
        {
            return new QuadLightSource
            {
                rectangle = new Rectangle(point.X, point.Y, 100, 100),
                color = new Color(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255))
            };
        }

        public static QuadLightSource MakeWhiteLight(Point point)
        {
            return new QuadLightSource
            {
                rectangle = new Rectangle(point.X, point.Y, 100, 100),
                color = new Color(255, 255, 255)
            };
        }

        public static QuadLightSource MakeWithColor(Point point, Color color)
        {
            return new QuadLightSource
            {
                rectangle = new Rectangle(point.X, point.Y, 100, 100),
                color = color
            };
        }
    }

    public class LightingTest : IScreen
    {
        private List<QuadLightSource> Lights;

        /// <summary>
        /// Used for multiplying between the two layers
        /// 1. Fully lit scene
        /// 2. Lights
        /// </summary>
        BlendState Multiply = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero
        };

        private bool RenderLight = true;

        private RenderTarget2D FullyLitWorld;
        private RenderTarget2D Lightpass;

        public override string ScreenName
        {
            get
            {
                return "LightingTest";
            }
            internal set { }
        }

        public LightingTest(Graphics.Graphics graphics)
        {
            int width, height;
            width = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width;
            height = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height;

            FullyLitWorld = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, width, height);
            Lightpass = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, width, height);

            Lights = new List<QuadLightSource>();

            graphics.ResolutionChanged += (sender, e) =>
            {
                width = e.Width;
                height = e.Height;
                Console.WriteLine("Destroying lighting test textures.");
                FullyLitWorld.Dispose();
                FullyLitWorld = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, width, height);
                Lightpass.Dispose();
                Lightpass = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, width, height);
            };
        }

        private void DrawFullyLit(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(FullyLitWorld);
            graphics.GetSpriteBatch().Begin();

            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.CornflowerBlue);

            graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("trivium"),
                new Rectangle(0, 0, 800, 350), Color.White);

            graphics.GetSpriteBatch().End();
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }

        private Texture2D MakeRectangle(Graphics.Graphics graphics, Color color)
        {
            var rectangle = new Texture2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, 1, 1);
            rectangle.SetData(new[] { color });
            return rectangle;
        }

        private void DrawLight(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(Lightpass);
            graphics.GetSpriteBatch().Begin(blendState: BlendState.Additive);
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(new Color(75, 75, 75, 100)); //ambient light

            Lights.ForEach(light =>
            {
                graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("circle"),
                    light.rectangle,
                    light.color);
            });

            var point = Minecraft2D.InputHelper.MousePosition.ToPoint();
            point.X -= (graphics.GetTexture2DByName("circle").Width / 2);
            point.Y -= (graphics.GetTexture2DByName("circle").Height / 2);

            graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("circle"), point.ToVector2(), Color.White);

            graphics.GetSpriteBatch().End();
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            DrawFullyLit(graphics);
            DrawLight(graphics);

            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.White);
            //graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, Multiply, null, null, null, null, null);
            graphics.GetSpriteBatch().Begin(blendState: Multiply, sortMode: SpriteSortMode.Immediate);
            graphics.GetSpriteBatch().Draw(FullyLitWorld, graphics.ScreenRectangle(), Color.White);
            if(RenderLight)
                graphics.GetSpriteBatch().Draw(Lightpass, graphics.ScreenRectangle(), Color.White);
            graphics.GetSpriteBatch().End();
        }

        public override void Update(GameTime gameTime)
        {
            if (Minecraft2D.InputHelper.IsNewPress(Keys.Space) && Minecraft2D.InputHelper.IsMouseInsideWindow())
                RenderLight = !RenderLight;

            if(Minecraft2D.InputHelper.IsNewPress(Input.MouseButtons.LeftButton))
            {
                var point = Minecraft2D.InputHelper.MousePosition.ToPoint();
                point.X -= 50;
                point.Y -= 50;
                Lights.Add(QuadLightSource.MakeWithColor(point, new Color(255, 255, 255)));
            }
        }
    }
}
