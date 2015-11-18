using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Minecraft2D.Controls
{
    public class Minecraft2DMessage
    {
        public string Sender { get; set; }
        public DateTime SentAt { get; set; }
        public bool Global { get; set; }
        public string Content { get; set; }
        public string To { get; set; }
        public int ID { get; set; }

        public Minecraft2DMessage()
        {
            SentAt = DateTime.Now;
            ID = MainGame.RandomGenerator.Next(0, 999999);
        }
    }

    public delegate void MessageReceived(int messageID);
    public class MessageQueue : Control
    {
        public event MessageReceived MessageReceived;

        public List<Minecraft2DMessage> MessageList { get; set; }
        public bool Visible { get; set; }
        public float OpacityMod { get; set; }
        private int QueueTimeout = 1000;

        private int TimeoutCounter = 0;

        public MessageQueue()
        {
            MessageList = new List<Minecraft2DMessage>();
#if DEBUG
            Minecraft2DMessage debugMessage = new Minecraft2DMessage();
            debugMessage.Sender = "GAME";
            debugMessage.Global = true;
            debugMessage.Content = "Your game is running in debug mode, be warned.";
            debugMessage.To = null;

            AddMessage(debugMessage);
#endif
        }

        public void AddMessage(Minecraft2DMessage msg)
        {
            MessageList.Add(msg);

            OpacityMod = .8f;
            TimeoutCounter = QueueTimeout;

            if (MessageReceived != null)
                MessageReceived(msg.ID);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsHelper.DrawRectangle(new Rectangle(0,
                MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - Math.Min(MessageList.Count * 12, 300),
                500,
                Math.Min(MessageList.Count * 12, 300)), 
                Color.Gray, 
                OpacityMod);
            if (MessageList.Count > 25)
            {
                for (int i = MessageList.Count; i > 25; i--)
                {

                }
            }
            else
            {
                int y = 0;
                for(int i = MessageList.Count - 1; i >= 0; i--)
                {
                    MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"),
                        MessageList[i].Sender.ToUpper() == "GAME" ? 
                        MessageList[i].Content : $"<{MessageList[i].Sender}> {MessageList[i].Content}", 
                            new Vector2(0, MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 12 - y), Color.White * OpacityMod);

                    y += 12;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (TimeoutCounter > 0)
            {
                if (TimeoutCounter < 250)
                {
                    if (OpacityMod > 0f)
                        OpacityMod -= .1f * (float)gameTime.ElapsedGameTime.TotalSeconds * QueueTimeout / 64;
                }
                TimeoutCounter--;
            }
        }
    }
}
