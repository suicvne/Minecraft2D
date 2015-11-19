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
using Minecraft2D.Saves;
using Minecraft2D.Map.Entities;

namespace Minecraft2D.Map
{
    public class World
    {
        public WorldObjects WorldObj;
        //private Tile[,] tiles = new Tile[256, 100];
        //public int[,] Lightmap { get; set; }

        private Random ran = new Random((int)DateTime.Now.Millisecond * 69);
        private PresetBlocks presets = new PresetBlocks();
        private long worldTime = 0;
        public Color BlockTint = Color.White; //Full daytime
        public Color SkyColor = Color.CornflowerBlue;

        Color shading = Color.White;
        int Lighting = 0;
        
        public long WorldTime { get { return worldTime; } set { worldTime = value; } }

        public int RenderedLights { get; internal set; }

        //public Player player { get; internal set; }
        public List<Player> players { get; internal set; }
        public List<Entity> entities { get; internal set; }

        public World(int SaveIndex = -1)
        {
            if(SaveIndex != -1)
            {
                WorldObj = new WorldObjects(this);
                WorldObj.WorldIndex = SaveIndex;
                WorldObj.LoadWorld();
                WorldObj.GenerateInitialLightmap();
            }
            else
            {
                WorldObj = new WorldObjects(this);
                WorldObj.GenerateFlatlands();
                WorldObj.GenerateInitialLightmap();
            }

            entities = new List<Entity>();
            
            if(players == null || GetClientPlayer() == null)
            {
                players = new List<Player>();
                Player p = new Player();
                p.Position = new Vector2(50 * 32, 50 * 32);
                p.IsClientPlayer = true;
                p.Username = MainGame.GameOptions.Username;
                players.Add(p);
            }   
        }

        
        private long oldTime;
        public void Update(GameTime gameTime)
        {
            AdvanceTime();
            UpdateTimedBlocks();

            if(MainGame.GlobalInputHelper.IsNewPress(Keys.P))
            {
                EntityCthulu e = new EntityCthulu();
                e.id = GenerateID();
                entities.Add(e);
            }

            if(players != null)
                foreach (var p in players)
                    p.Update(gameTime);

            if (entities != null && entities.Count > 0)
            {
                foreach (dynamic e in entities)
                {
                    e.Update(gameTime);
                }
            }
        }

        private int GenerateID()
        {
        regen:
            int testID = MainGame.RandomGenerator.Next(0001, 9999);
            if (IdAlreadyExists(testID))
                goto regen;
            else
                return testID;
        }
        private bool IdAlreadyExists(int id)
        {
            return entities.Find(x => x.id == id) != null;
        }

        private void AdvanceTime()
        {
            oldTime = worldTime;
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

        private void UpdateTimedBlocks()
        {
            int[] ranges = GetMaxRenderPoints();

            for (int y = (int)ranges[2]; y < ranges[3] + 2; y++)
            {
                for (int x = (int)ranges[0]; x < ranges[1] + 2; x++)
                {
                    if (x > WorldObj.ForegroundLayerTiles.GetLength(1) - 1)
                        continue;
                    if (y > WorldObj.ForegroundLayerTiles.GetLength(0) - 1)
                        continue;

                    if(WorldObj.ForegroundLayerTiles[y, x].Name.Contains("furnace") && WorldObj.ForegroundLayerTiles[y, x].Light > 0)
                    {
                        long deltaTime = worldTime - WorldObj.ForegroundLayerTiles[y, x].TimePlaced;
                        if(deltaTime > 3000)
                        {
                            Tile replacementTile = PresetBlocks.TilesList.Find(i => i.Name == "minecraft:furnace").AsTile();
                            replacementTile.Position = WorldObj.ForegroundLayerTiles[y, x].Position;
                            replacementTile.TimePlaced = worldTime;
                            WorldObj.ForegroundLayerTiles[y, x] = replacementTile;
                            WorldObj.Lightmap[y, x] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Absolute x and y values are used here
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            int tX = (int)Math.Floor((double)(x + 16) / 32); //add half of the cross hair so it's centered
            int tY = (int)Math.Floor((double)(y + 16) / 32);
            if (tX > WorldObj.ForegroundLayerTiles.GetLength(1) - 1 || tX < 0 || tY > WorldObj.ForegroundLayerTiles.GetLength(0) - 1 || tY < 0)
                return null;
            else
                return WorldObj.ForegroundLayerTiles[tY, tX];
        }


        /// <summary>
        /// Absolute x and y values are used here.
        /// Also sets the tile position.
        /// </summary>
        public void SetTile(int x, int y, Tile toReplace)
        {
            int tX = (int)Math.Floor((double)(x + 16) / 32); //add half of the cross hair so it's centered
            int tY = (int)Math.Floor((double)(y + 16) / 32);
            if (tX > WorldObj.ForegroundLayerTiles.GetLength(1) || tX < 0 || tY > WorldObj.ForegroundLayerTiles.GetLength(0) || tY < 0)
                return;
            if (WorldObj.ForegroundLayerTiles[tY, tX].Hardness == -1)
                return;

            else
            {
                if (toReplace.Type == TileType.Air)
                {
                    if (WorldObj.ForegroundLayerTiles[tY, tX].PlaceSoundName != null)
                    {
                        if (placeSoundSEI == null)
                        {
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(WorldObj.ForegroundLayerTiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                        }
                        else
                        {
                            //if (placeSoundSEI.State == SoundState.Paused)
                            //{
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(WorldObj.ForegroundLayerTiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                            //}
                        }
                    }
                    WorldObj.ForegroundLayerTiles[tY, tX] = toReplace;
                }
                else
                {
                    WorldObj.ForegroundLayerTiles[tY, tX] = toReplace;
                    if (WorldObj.ForegroundLayerTiles[tY, tX].PlaceSoundName != null)
                    {
                        if (placeSoundSEI == null)
                        {
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(WorldObj.ForegroundLayerTiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                        }
                        else
                        {
                            //if (placeSoundSEI.State == SoundState.Stopped)
                            //{
                            int soundIndex = ran.Next(1, 5);
                            placeSoundSEI = MainGame.CustomContentManager.GetSoundEffect(string.Format(WorldObj.ForegroundLayerTiles[tY, tX].PlaceSoundName, soundIndex)).CreateInstance();
                            placeSoundSEI.Play();
                            //}
                        }
                    }
                }
                WorldObj.ForegroundLayerTiles[tY, tX].Position = new Vector2(tX * 32, tY * 32);

                //if (tiles[tY, tX].Type == TileType.Air || tiles[tY, tX].IsBackground)
                //    Lightmap[tY, tX] = 5;
                //else if (tiles[tY, tX].Light == TileType.Torch)
                //    Lightmap[tY, tX] = 12;
                //else
                //    Lightmap[tY, tX] = 0;
                WorldObj.Lightmap[tY, tX] = WorldObj.ForegroundLayerTiles[tY, tX].Light;
                if (WorldObj.ForegroundLayerTiles[tY, tX].IsBackground)
                {
                    WorldObj.Lightmap[tY, tX] = 5;
                    WorldObj.ForegroundLayerTiles[tY, tX].TransparencyOfTile = TileTransparency.FullyTransparent;
                }
            }
        }

        private SoundEffectInstance placeSoundSEI;

        public void DrawLightmap(GameTime gameTime)
        {
            RenderedLights = 0;
            //viewportRect = new Rectangle((int)MainGame.GameCamera.Pos.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2),
            //        (int)MainGame.GameCamera.Pos.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2),
            //        MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height);
            MainGame.GlobalGraphicsDevice.Clear(Color.Black);

            int[] ranges = this.GetMaxRenderPoints();

            for (int y = (int)ranges[2]; y < ranges[3] + 2; y++)
            {
                for (int x = (int)ranges[0]; x < ranges[1] + 2; x++)
                {
                    //Console.WriteLine($"Length(0): {tiles.GetLength(0)}; Length(1): {tiles.GetLength(1)}");
                    if (x > WorldObj.ForegroundLayerTiles.GetLength(1) - 1)
                        continue;
                    if (y > WorldObj.ForegroundLayerTiles.GetLength(0) - 1)
                        continue;

                    if (WorldObj.Lightmap[y, x] > 0f)
                    {
                        if (y < 42) //ensures the underground is dark
                        {
                            if (WorldObj.ForegroundLayerTiles[y, x].IsBackground)
                            {
                                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                                        new Rectangle(x * 32 - 64, y * 32 - 64, 32 * 5, 32 * 5), Color.White);
                                RenderedLights++;
                            }
                            else
                            {
                                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                                    new Rectangle(x * 32 - WorldObj.ForegroundLayerTiles[y, x].LightOffset, y * 32 - WorldObj.ForegroundLayerTiles[y, x].LightOffset, 32 * WorldObj.Lightmap[y, x], 32 * WorldObj.Lightmap[y, x]), Color.White);
                                RenderedLights++;
                            }
                        }
                        if (WorldObj.ForegroundLayerTiles[y, x].Light > 0 && WorldObj.ForegroundLayerTiles[y, x].Type != TileType.Air)
                        {
                            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                                new Rectangle(x * 32 - WorldObj.ForegroundLayerTiles[y, x].LightOffset, y * 32 - WorldObj.ForegroundLayerTiles[y, x].LightOffset, 32 * WorldObj.Lightmap[y, x], 32 * WorldObj.Lightmap[y, x]), Color.White);
                            RenderedLights++;
                        }
                    }
                }

                //for (int x = 0; x < Lightmap.GetLength(1); x++)
                //{
                //    for (int y = 0; y < Lightmap.GetLength(0); y++)
                //    {

                //        //Adjust shadow during "sun" movement.
                //        switch (worldTime)
                //        {
                //            case 0:
                //                tiles[y, x].LightOffset -= 1;
                //                break;
                //            case 6000:
                //                tiles[y, x].LightOffset -= 1;
                //                break;
                //            case 12000:
                //                tiles[y, x].LightOffset += 1;
                //                break;
                //            case 18000:
                //                tiles[y, x].LightOffset += 1;
                //                break;
                //        }
                //        Rectangle objectBounds = new Rectangle(x * 32, y * 32, 32 * 5, 32 * 5); //radius of the lightmap
                //        if (CalculateViewport().Intersects(objectBounds))
                //        {
                //            if (Lightmap[y, x] > 0f)
                //            {
                //                if (y < 42) //ensures the underground is dark
                //                {
                //                    if (tiles[y, x].IsBackground)
                //                    {
                //                        MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                //                                new Rectangle(x * 32 - 64, y * 32 - 64, 32 * 5, 32 * 5), Color.White);
                //                        RenderedLights++;
                //                    }
                //                    else
                //                    {
                //                        MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                //                            new Rectangle(x * 32 - tiles[y, x].LightOffset, y * 32 - tiles[y, x].LightOffset, 32 * Lightmap[y, x], 32 * Lightmap[y, x]), Color.White);
                //                        RenderedLights++;
                //                    }
                //                }
                //                if (tiles[y, x].Light > 0 && tiles[y, x].Type != TileType.Air)
                //                {
                //                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                //                        new Rectangle(x * 32 - tiles[y, x].LightOffset, y * 32 - tiles[y, x].LightOffset, 32 * Lightmap[y, x], 32 * Lightmap[y, x]), Color.White);
                //                    RenderedLights++;
                //                }
                //            }

                //        }
                //    }
                //}
            }
        }

        [Obsolete]
        private Rectangle CurrentViewport;
        [Obsolete]
        public Rectangle CalculateViewport()
        {
            Rectangle viewportRect = new Rectangle((int)MainGame.GameCamera.Pos.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2),
                    (int)MainGame.GameCamera.Pos.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2),
                    MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height);
            return viewportRect;
        }

        public Player GetClientPlayer()
        {
            if(players == null)
            {
                players = new List<Player>();
                Player p = new Player();
                p.IsClientPlayer = true;
                p.Username = MainGame.GameOptions.Username;
                players.Add(p);
            }
            return players.Find(x => x.IsClientPlayer);
        }

        public bool HasRoomForEntity(Rectangle toCheck, bool sim, bool tileCheck)
        {
            if (!sim)
            {
                if (toCheck.X < 0)
                    return false;
                if (toCheck.Y < 0)
                    return false;
                if (toCheck.X > (WorldObj.MetaFile.WorldSize.X - 32))
                    return false;
                if (toCheck.Y > (WorldObj.MetaFile.WorldSize.Y - 32))
                    return false;
            }

            if(tileCheck)
            {
                Rectangle realBounds = new Rectangle((int)((toCheck.X / 32d) * 32), (int)((toCheck.Y / 32d) * 32), toCheck.Width, toCheck.Height);

                if (realBounds.Intersects(new Rectangle((int)GetClientPlayer().Position.X, (int)GetClientPlayer().Position.Y, GetClientPlayer().Hitbox.Width, GetClientPlayer().Hitbox.Height)))
                    return false;
                else
                    return true;
            }
            
            int[] ranges = this.GetMaxRenderPoints();

            for (int y = (int)ranges[2]; y < ranges[3] + 2; y++)
            {
                for (int x = (int)ranges[0]; x < ranges[1] + 2; x++)
                {
                    //Console.WriteLine($"Length(0): {tiles.GetLength(0)}; Length(1): {tiles.GetLength(1)}");
                    if (x > WorldObj.ForegroundLayerTiles.GetLength(1) - 1)
                        continue;
                    if (y > WorldObj.ForegroundLayerTiles.GetLength(0) - 1)
                        continue;
                    if (WorldObj.ForegroundLayerTiles[y, x].TransparencyOfTile == TileTransparency.FullyOpague && WorldObj.ForegroundLayerTiles[y, x].Bounds.Intersects(toCheck))
                        return false;
                }
            }
            return true;
        }

        public Vector2 WhereCanIGo(Vector2 originalPosition, Vector2 destination, Rectangle bounds)
        {
            Vector2 movementToTry = destination - originalPosition;
            Vector2 furthestAvailable = originalPosition;
            int numberOfStepsToBreakInto = (int)(movementToTry.Length() * 2) + 1;
            Vector2 oneStep = movementToTry / numberOfStepsToBreakInto;

            for (int i = 1; i <= numberOfStepsToBreakInto; i++)
            {
                Vector2 positionToTry = originalPosition + oneStep * i;
                Rectangle newBoundary =
                    CreateRectangleAtPosition(positionToTry, bounds.Width, bounds.Height);
                if (HasRoomForEntity(newBoundary, false, false)) { furthestAvailable = positionToTry; }
                else
                {
                    bool isDiagonalMove = movementToTry.X != 0 && movementToTry.Y != 0;
                    if(isDiagonalMove)
                    {
                        int stepsLeft = numberOfStepsToBreakInto - (i - 1);
                        Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * stepsLeft;
                        Vector2 finalPositionIfMovingHorizontally = furthestAvailable + remainingHorizontalMovement;
                        furthestAvailable =
                        WhereCanIGo(furthestAvailable, finalPositionIfMovingHorizontally, bounds);

                        Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * stepsLeft;
                        Vector2 finalPositionIfMovingVertically = furthestAvailable + remainingVerticalMovement;
                        furthestAvailable =
                        WhereCanIGo(furthestAvailable, finalPositionIfMovingVertically, bounds);
                    }
                    break;
                }
            }
            return furthestAvailable;
        }
        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }
        
        
        private Vector2 LightSize = new Vector2(32 * 5, 32 * 5);

        /// <summary>
        /// size 4 array, meant for tile index not abs X positions
        /// 0: minX
        /// 1: maxX
        /// 2: minY
        /// 3: maxY
        /// </summary>
        /// <returns>
        /// </returns>
        private int[] GetMaxRenderPoints()
        {
            int minX = (int)Math.Floor(((float)MainGame.GameCamera.Pos.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2)) / 32);
            int maxX = (int)Math.Floor(((float)MainGame.GameCamera.Pos.X + (MainGame.GlobalGraphicsDevice.Viewport.Width / 2)) / 32);
            int minY = (int)Math.Floor(((float)MainGame.GameCamera.Pos.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2)) / 32);
            int maxY = (int)Math.Floor(((float)MainGame.GameCamera.Pos.Y + (MainGame.GlobalGraphicsDevice.Viewport.Height / 2)) / 32);
            
            return new int[4] { Math.Abs(minX), Math.Abs(maxX), Math.Abs(minY), Math.Abs(maxY) }; ;
        }

        public void Draw(GameTime gameTime)
        {
            if(players == null)
            {
                players = new List<Player>();
                Player p = new Player();
                p.IsClientPlayer = true;
                p.Username = MainGame.GameOptions.Username;
                players.Add(p);
            }
            if(players.Count == 0)
            {
                Player p = new Player();
                p.IsClientPlayer = true;
                p.Username = MainGame.GameOptions.Username;
                players.Add(p);
            }
            
            DrawTiles(gameTime);
            DrawEntities(gameTime);
        }

        private void DrawTiles(GameTime gameTime)
        {
            int[] ranges = this.GetMaxRenderPoints();

            for (int y = ranges[2]; y < ranges[3] + 2; y++)
            {
                for (int x = ranges[0]; x < ranges[1] + 2; x++)
                {
                    //Console.WriteLine($"Length(0): {tiles.GetLength(0)}; Length(1): {tiles.GetLength(1)}");
                    if (x > WorldObj.ForegroundLayerTiles.GetLength(1) - 1)
                        continue;
                    if (y > WorldObj.ForegroundLayerTiles.GetLength(0) - 1)
                        continue;

                    if (!WorldObj.ForegroundLayerTiles[y, x].Name.Trim().Contains("air"))
                    {
                        if (!WorldObj.ForegroundLayerTiles[y, x].IsBackground)
                        {
                            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"),
                                new Rectangle((int)WorldObj.ForegroundLayerTiles[y, x].Position.X, (int)WorldObj.ForegroundLayerTiles[y, x].Position.Y, 32, 32),
                                new Rectangle(WorldObj.ForegroundLayerTiles[y, x].TextureRegion.X, WorldObj.ForegroundLayerTiles[y, x].TextureRegion.Y, 16, 16),
                                BlockTint,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f);
                        }
                        else
                        {
                            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"),
                                new Rectangle((int)WorldObj.ForegroundLayerTiles[y, x].Position.X, (int)WorldObj.ForegroundLayerTiles[y, x].Position.Y, 32, 32),
                                new Rectangle(WorldObj.ForegroundLayerTiles[y, x].TextureRegion.X, WorldObj.ForegroundLayerTiles[y, x].TextureRegion.Y, 16, 16),
                                Color.Gray,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f);
                        }
                    }
                }
            }
        }

        private void DrawEntities(GameTime gameTime)
        {
            foreach (var p in players)
                p.Draw(gameTime);

            if (entities != null && entities.Count > 0)
                foreach (var e in entities)
                    e.Draw(gameTime);
        }
    }
}
