using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public class ContentManager
    {
        private List<KeyValuePair<string, Texture2D>> content = new List<KeyValuePair<string, Texture2D>>();
        private List<KeyValuePair<string, BitmapFont>> fonts = new List<KeyValuePair<string, BitmapFont>>();
        private List<KeyValuePair<string, SoundEffect>> sounds = new List<KeyValuePair<string, SoundEffect>>();
        public SpriteFont SplashFont = MainGame.GlobalContentManager.Load<SpriteFont>("minecraft-fnt");

        public ContentManager() { }

        public void AddTexture(string name, Texture2D texture) => content.Add(new KeyValuePair<string, Texture2D>(name, texture));
        public Texture2D GetTexture(string name) => content.Find(x => x.Key == name).Value;

        public void AddSpriteFont(string name, BitmapFont font) => fonts.Add(new KeyValuePair<string,BitmapFont>(name, font));
        public BitmapFont GetFont(string name) => fonts.Find(x => x.Key == name).Value;

        public void AddSoundEffect(string name, SoundEffect effect) => sounds.Add(new KeyValuePair<string, SoundEffect>(name, effect));
        public SoundEffect GetSoundEffect(string name) => sounds.Find(x=>x.Key == name).Value;
    }
}
