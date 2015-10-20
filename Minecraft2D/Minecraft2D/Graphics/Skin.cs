using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public class Skin
    {
        public Texture2D FullSkin { get; internal set; }

        public SkinRegion HeadRegion { get; internal set; }
        public SkinRegion HeadTop { get; internal set; }
        public SkinRegion HeadLeft { get; internal set; }
        public SkinRegion HeadRight { get; internal set; }
        public SkinRegion HeadBack { get; internal set; }

        public SkinRegion OutsideLeg { get; internal set; }
        public SkinRegion FrontLeg { get; internal set; }
        public SkinRegion InsideLeg { get; internal set; }
        public SkinRegion BackLeg { get; internal set; }
        public SkinRegion RightTorso { get; internal set; }

        public SkinRegion FrontTorso { get; internal set; }
        public SkinRegion BackTorso { get; internal set; }

        public SkinRegion LeftTorso { get; internal set; }

        public SkinRegion OutsideArm { get; internal set; }
        public SkinRegion FrontArm { get; internal set; }
        public SkinRegion InsideArm { get; internal set; }
        public SkinRegion BackArm { get; internal set; }

        private Skin() { }

        public Skin(Texture2D FullSkin)
        {
            this.FullSkin = FullSkin;

            HeadRegion = new SkinRegion(8, 8, 16, 16);
            HeadTop = new SkinRegion(8, 0, 16, 16);
            HeadRight = new SkinRegion(0, 8, 16, 16);
            HeadLeft = new SkinRegion(16, 8, 16, 16);
            HeadBack = new SkinRegion(32, 8, 16, 16);

            OutsideLeg = new SkinRegion(0, 20, 4, 12);
            FrontLeg = new SkinRegion(4, 20, 4, 12);
            InsideLeg = new SkinRegion(8, 20, 4, 12);
            BackLeg = new SkinRegion(12, 20, 4, 12);
            RightTorso = new SkinRegion(16, 20, 4, 12);

            FrontTorso = new SkinRegion(20, 20, 8, 12);
            LeftTorso = new SkinRegion(28, 20, 4, 12);
            BackTorso = new SkinRegion(32, 20, 8, 12);

            OutsideArm = new SkinRegion(40, 20, 8, 12);
            FrontArm = new SkinRegion(44, 20, 8, 12);
            InsideArm = new SkinRegion(48, 20, 8, 12);
            BackArm = new SkinRegion(52, 20, 8, 12);
        }
    }
}
