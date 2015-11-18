using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Controls;
using Minecraft2D.Map;
using System.Reflection;

namespace Minecraft2D.Screens
{
    public class DebugInfo : Screen
    {
        public DebugInfo()
        {
            Button doneButton = new Button(new Rectangle(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth),
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 50, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Done", true);
            doneButton.Clicked += () => MainGame.manager.PushScreen(GameScreens.MAIN);

            Label TitleLabel = new Label("CatchLabel", "Here's some debug info :)", new Vector2(16, 10), Color.Gray);
            Label MC2DVer = new Label("MC2DVer", "* Minecraft 2D Version: " + MainGame.GameVersion.ToString(), new Vector2(20, 46), Color.Gray);
            Label NETVer = new Label("NETVer", "* .NET Version: " + Assembly
                     .GetExecutingAssembly()
                     .GetReferencedAssemblies()
                     .Where(x => x.Name == "System.Core").First().Version.ToString(3), new Vector2(20, 62), Color.Gray);
            Label BlockCount = new Label("BlockCount", "* Total Loaded Blocks: " + PresetBlocks.TilesList.Count, new Vector2(20, 78), Color.Gray);
            Label WorkingDirectory = new Label("WorkingDirectory", "* Current Directory: " + Environment.CurrentDirectory, new Vector2(20, 94), Color.Gray);
            Label OSInfo = new Label("OSINFO", $"* OS Info: {Environment.OSVersion.Platform} {Environment.OSVersion.VersionString} 64 Bit: {Environment.Is64BitOperatingSystem}", new Vector2(20, 86 + 24), Color.Gray);
#if DEBUG
            Label DEBUG = new Label("DEBUG", $"#if DEBUG defined", new Vector2(20, 86 + 48), Color.Gray);
            Label AboutDebugControls = new Label("AboutDebugControls", "When running in Debug, you can use the following controls for cool stuff\nF2: Screenshot\nF3: Toggle Debug Info\nF4: Toggle Lights\nAlt + F2: Screenshot w/out Lights", 
                new Vector2(20, 86 + 48 + 24), Color.White);
#endif
            //Label GPU = new Label("GPU", $"* GPU: ", new Vector2(20, 86 + 14 + 14), Color.Gray);

            AddControl(doneButton);
            AddControl(TitleLabel);
            AddControl(MC2DVer);
            AddControl(NETVer);
            AddControl(BlockCount);
            AddControl(WorkingDirectory);
            AddControl(OSInfo);
#if DEBUG
            AddControl(DEBUG);
            AddControl(AboutDebugControls);
#endif
            //AddControl(GPU);
        }

        public override void Draw(GameTime gameTime)
        {
            int tx, ty;
            tx = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Width / 32);
            ty = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Height / 32);

            MainGame.GlobalGraphicsDevice.Clear(Color.CornflowerBlue);

            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            for (int x = 0; x < tx; x++)
                for (int y = 0; y < ty; y++)
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(16 * 2, 0, 16, 16), new Color(100, 100, 100));

            foreach (var c in ControlsList)
                c.Draw(gameTime);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32), Color.White);

            MainGame.GlobalSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var c in ControlsList)
                c.Update(gameTime);   
        }
    }
}
