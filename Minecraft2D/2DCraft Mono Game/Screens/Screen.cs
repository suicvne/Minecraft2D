using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Screens
{
    public abstract class Screen
    {
        public List<Control> ControlsList = new List<Control>();

        public Screen()
        {
        }

        public void AddControl(Control ctrl)
        {
            ControlsList.Add(ctrl);
            if(ctrl.GetType() == typeof(TextBox))
            {
                TextBox casted = (TextBox)ctrl;
                casted.MouseClicked += () =>
                {
//                    casted.HasFocus = true;
                    foreach (var ct in ControlsList)
                        if (ct.Name != casted.Name)
                            ct.HasFocus = false;
                };
            }
        }

        /// <summary>
        /// Do NOT add labels with this
        /// </summary>
        /// <param name="ctrl1"></param>
        /// <param name="label"></param>
        public void AddButtonWithLabel(Button ctrl1, Label label)
        {
            ControlsList.Add(ctrl1);
            label.Position = new Rectangle(((Button)ctrl1).Position.X, ((Button)ctrl1).Position.Y - 20, label.Position.Width, label.Position.Height);
            ControlsList.Add(label);
        }

        public void AddTextBoxWithLabel(TextBox ctrl1, Label label)
        {
            ControlsList.Add(ctrl1);
            label.Position = new Rectangle(((TextBox)ctrl1).Position.X, ((TextBox)ctrl1).Position.Y - 20, label.Position.Width, label.Position.Height);
            ControlsList.Add(label);
        }

        /// <summary>
        /// This is where you do input stuffs
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// This is where you handle your drawing logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);
    }
}
