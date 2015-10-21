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

        public ContentManager() { }

        public void AddTexture(string name, Texture2D texture)
        {
            content.Add(new KeyValuePair<string, Texture2D>(name, texture));
        }

        public Texture2D GetTexture(string name)
        {
            return content.Find(x => x.Key == name).Value;
        }

        public void AddSpriteFont(string name, SpriteFont font)
        { fonts.Add(new KeyValuePair<string,SpriteFont>(name, font)); }

        public SpriteFont GetFont(string name)
        { return fonts.Find(x => x.Key == name).Value; }
    }
}
