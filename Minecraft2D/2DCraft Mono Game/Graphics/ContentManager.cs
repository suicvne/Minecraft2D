using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public class ContentManager
    {
        private List<KeyValuePair<string, Texture2D>> content = new List<KeyValuePair<string, Texture2D>>();
        private List<KeyValuePair<string, SpriteFont>> fonts = new List<KeyValuePair<string, SpriteFont>>();
        private List<KeyValuePair<string, SoundEffect>> sounds = new List<KeyValuePair<string, SoundEffect>>();

        public ContentManager() { }

        public void AddTexture(string name, Texture2D texture) => content.Add(new KeyValuePair<string, Texture2D>(name, texture));
        public Texture2D GetTexture(string name) => content.Find(x => x.Key == name).Value;

        public void AddSpriteFont(string name, SpriteFont font) => fonts.Add(new KeyValuePair<string,SpriteFont>(name, font));
        public SpriteFont GetFont(string name) => fonts.Find(x => x.Key == name).Value;

        public void AddSoundEffect(string name, SoundEffect effect) => sounds.Add(new KeyValuePair<string, SoundEffect>(name, effect));
        public SoundEffect GetSoundEffect(string name) => sounds.Find(x=>x.Key == name).Value;
    }
}
