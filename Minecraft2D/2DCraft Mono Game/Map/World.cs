using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Minecraft2D.Map
{
    public class World
    {
        private Tile[,] tiles = new Tile[256, 100];
        public int[,] Lightmap { get; set; }

        private Random ran = new Random((int)DateTime.Now.Millisecond * 69);
        private PresetBlocks presets = new PresetBlocks();
        private long worldTime = 0;
        public Color BlockTint = Color.White; //Full daytime
        public Color SkyColor = Color.CornflowerBlue;

        Color shading = Color.White;
        int Lighting = 0;

        public Vector2 WorldSize { get; set; }
        public long WorldTime { get { return worldTime; } set { worldTime = value; } }

        public int RenderedLights { get; internal set; }

        public Player player { get; internal set; }

        public World()
        {
            Lightmap = new int[256, 100];
            WorldSize = new Vector2(tiles.GetLength(1) * 32, tiles.GetLength(0) * 32);
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if (y > 32)
                    {
                        if (y == 33)
                        {
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:grass").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                        else if (y <= 37)
                        {
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:dirt").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                        else if (y == 255)
                        {
                            //TODO: bedrock
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:bedrock").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                        else
                        {
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:stone").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                    }
                    else
                    {
                        Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:air").AsTile();
                        t.Position = new Vector2(x * 32, y * 32);
                        tiles[y, x] = t;
                        t = null;
                    }
                }
            }
            if (File.Exists("World1.mc2dwld"))
                LoadWorld("World1.mc2dwld");
            GenerateLightmap();

        }

        private void GenerateLightmap()
        {
            for (int x = 0; x < tiles.GetLength(1); x++)
            {
                for (int y = 0; y < tiles.GetLength(0); y++)
                {
                    if (tiles[y, x].Type == TileType.Air || tiles[y, x].IsBackground)
                        Lightmap[y, x] = 5;
                    else if (tiles[y, x].Type == TileType.Torch)
                        Lightmap[y, x] = 12;
                }
            }
        }

        public void SaveWorld(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    for (int y = 0; y < tiles.GetLength(0); y++)
                    {
                        string format = 
                            string.Format("{0};{1};{2};{3}", tiles[y, x].Name, tiles[y, x].Position.X, tiles[y, x].Position.Y, tiles[y, x].IsBackground);
                        sw.WriteLine(format);
                    }
                }
                sw.Flush();
            }
        }

        public void LoadWorld(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string input = "";
                while (!sr.EndOfStream)
                {
                    input = sr.ReadLine();
                    if (input.Trim() != String.Empty)
                    {
                        string[] split = input.Split(new char[] { ';' }, 4);
                        int x, y;
                        bool isBg = bool.Parse(split[3]);
                        x = (int)Math.Floor((double)Int32.Parse(split[1]) / 32);
                        y = (int)Math.Floor((double)Int32.Parse(split[2]) / 32);
                        string tileDataName = split[0];
                        Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == tileDataName.Trim()) != null ? PresetBlocks.TilesList.Find(srch => srch.Name == tileDataName.Trim()).AsTile() : new Tile();
                        t.Position = new Vector2(x * 32, y * 32);
                        t.IsBackground = isBg;
                        tiles[y, x] = t;
                    }
                }
            }
        }


        public void Update(GameTime gameTime)
        {
            worldTime++;

            if (WorldTime > 24000)
                WorldTime = 0;

            if (WorldTime < 12000)
            {
                BlockTint = Color.White;
            }
            else
            {
                BlockTint = Color.DimGray;
            }

            //Adjust lighting levels at a square root curve.
            SkyColor = new Color(0, (float)Math.Sqrt(shading.G / 255f * Lighting * 0.00001), (float)Math.Sqrt(shading.B / 255f * Lighting * 0.001));

            //Slowly illuminate and darken terrain.
            if (Lighting != WorldTime && WorldTime <= 12000)
            {
                Lighting = (int)worldTime;

            }
            else
            {
                if (worldTime < 17250) //17250 or whenever night should begin.
                {
                    Lighting = (int)(worldTime * -1) + 24000;
                }
                else
                {
                    Lighting = (int)(worldTime * -1) + 18000;
                }

            }

        }

        public Rectangle viewportRect { get; set; }

        /// <summary>
        /// Absolute x and y values are used here
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            int tX = (int)Math.Floor((double)(x + 16) / 32); //add half of the cross hair so it's centered
            int tY = (int)Math.Floor((double)(y + 16) / 32);
            if (tX > tiles.GetLength(1) - 1 || tX < 0 || tY > tiles.GetLength(0) - 1 || tY < 0)
                return null;
            else
                return tiles[tY, tX];
        }

        private bool SoundPlaying = false;

        /// <summary>
        /// Absolute x and y values are used here.
        /// Also sets the tile position.
        /// </summary>
        public void SetTile(int x, int y, Tile toReplace)
        {
            int tX = (int)Math.Floor((double)(x + 16) / 32); //add half of the cross hair so it's centered
            int tY = (int)Math.Floor((double)(y + 16) / 32);
            if (tX > tiles.GetLength(1) || tX < 0 || tY > tiles.GetLength(0) || tY < 0)
                return;
            if (tiles[tY, tX].Hardness == -1)
                return;

            else
            {
                if (toReplace.Type == TileType.Air)
                {
                    if (tiles[tY, tX].PlaceSoundName != null)
                    {
                        if (placeSoundSEI == null)
                        {
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(tiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                        }
                        else
                        {
                            //if (placeSoundSEI.State == SoundState.Paused)
                            //{
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(tiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                            //}
                        }
                    }
                    tiles[tY, tX] = toReplace;
                }
                else
                {
                    tiles[tY, tX] = toReplace;
                    if (tiles[tY, tX].PlaceSoundName != null)
                    {
                        if (placeSoundSEI == null)
                        {
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(tiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                        }
                        else
                        {
                            //if (placeSoundSEI.State == SoundState.Stopped)
                            //{
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(tiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                            //}
                        }
                    }
                }
                tiles[tY, tX].Position = new Vector2(tX * 32, tY * 32);

                //if (tiles[tY, tX].Type == TileType.Air || tiles[tY, tX].IsBackground)
                //    Lightmap[tY, tX] = 5;
                //else if (tiles[tY, tX].Light == TileType.Torch)
                //    Lightmap[tY, tX] = 12;
                //else
                //    Lightmap[tY, tX] = 0;
                Lightmap[tY, tX] = tiles[tY, tX].Light;
                if (tiles[tY, tX].IsBackground)
                    Lightmap[tY, tX] = 5;

            }
        }

        private SoundEffectInstance placeSoundSEI;

        public void DrawLightmap(GameTime gameTime)
        {
            RenderedLights = 0;
            viewportRect = new Rectangle((int)MainGame.GameCamera.Pos.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2),
                    (int)MainGame.GameCamera.Pos.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2),
                    MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height);
            MainGame.GlobalGraphicsDevice.Clear(Color.Black);



            for (int x = 0; x < Lightmap.GetLength(1); x++)
            {
                for (int y = 0; y < Lightmap.GetLength(0); y++)
                {

                    //Adjust shadow during "sun" movement.
                    switch (worldTime)
                    {
                        case 0:
                            tiles[y, x].LightOffset -= 1;
                            break;
                        case 6000:
                            tiles[y, x].LightOffset -= 1;
                            break;
                        case 12000:
                            tiles[y, x].LightOffset += 1;
                            break;
                        case 18000:
                            tiles[y, x].LightOffset += 1;
                            break;
                    }
                    Rectangle objectBounds = new Rectangle(x * 32, y * 32, 32 * 5, 32 * 5); //radius of the lightmap
                    if (viewportRect.Intersects(objectBounds))
                    {
                        if (Lightmap[y, x] > 0f)
                        {
                            if (y < 42) //ensures the underground is dark
                            {
                                if (tiles[y, x].IsBackground)
                                {
                                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                                            new Rectangle(x * 32 - 64, y * 32 - 64, 32 * 5, 32 * 5), Color.White);
                                    RenderedLights++;
                                }
                                else
                                {
                                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                                        new Rectangle(x * 32 - tiles[y, x].LightOffset, y * 32 - tiles[y, x].LightOffset, 32 * Lightmap[y, x], 32 * Lightmap[y, x]), Color.White);
                                    RenderedLights++;
                                }
                            }
                            if (tiles[y, x].Light > 0 && tiles[y, x].Type != TileType.Air)
                            {
                                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                                    new Rectangle(x * 32 - tiles[y, x].LightOffset, y * 32 - tiles[y, x].LightOffset, 32 * Lightmap[y, x], 32 * Lightmap[y, x]), Color.White);
                                RenderedLights++;
                            }
                        }

                    }
                }
            }
        }

        private Vector2 LightSize = new Vector2(32 * 5, 32 * 5);
        public void Draw(GameTime gameTime)
        {
            if (player == null)
                player = new Player();

            /*MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));*/
            List<Tile> tilesToBeRendered = new List<Tile>();

            viewportRect = new Rectangle((int)MainGame.GameCamera.Pos.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2),
                    (int)MainGame.GameCamera.Pos.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2),
                    MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height);
            foreach (var block in tiles)
            {
                Rectangle objectBounds = new Rectangle((int)block.Position.X, (int)block.Position.Y, 34, 34);
                int x, y;
                x = (int)Math.Floor((double)block.Position.X / 32);
                y = (int)Math.Floor((double)block.Position.Y / 32);
                if (viewportRect.Intersects(objectBounds))
                {
                    tilesToBeRendered.Add(block);
                    //if(block.Type == TileType.Air)
                    //    Lightmap[y, x] = 1; //it'll either be 1f or 0f i guess
                }
            }

            foreach (Tile block in tilesToBeRendered)
            {
                //if (block.Type == TileType.Air)
                //    AddLight(stationaryLightMap, (int)Math.Floor(block.Position.X / 32), (int)Math.Floor(block.Position.Y / 32));
                if (block.Type != TileType.Air)
                    if (!block.IsBackground)
                    {
                        int x = (int)Math.Floor(block.Position.X / 32);
                        int y = (int)Math.Floor(block.Position.Y / 32);

                        MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"),
                            new Rectangle(Convert.ToInt32(block.Position.X), Convert.ToInt32(block.Position.Y), 32, 32), //scaling to 32
                            new Rectangle(block.TextureRegion.X, block.TextureRegion.Y, 16, 16),
                            BlockTint,
                            0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0f);
                    }
                    else
                    {
                        MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"),
                            new Rectangle(Convert.ToInt32(block.Position.X), Convert.ToInt32(block.Position.Y), 32, 32), //scaling to 32
                            new Rectangle(block.TextureRegion.X, block.TextureRegion.Y, 16, 16),
                            Color.Gray,
                            0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0f);
                    }
            }
            //MainGame.GlobalSpriteBatch.End();

            player.Draw(gameTime);
        }
    }
}
