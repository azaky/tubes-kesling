using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace GarbageCollector
{
    class Trashbin  
    {
        private TrashStatus status;
        private Vector2 pos;
        private int textureId;
        private Rectangle rectDraw;
        private int correctScore, wrongScore;
        private static int totalScore;

        public Trashbin(int correctScore, int wrongScore)
        {
            this.correctScore = correctScore;
            this.wrongScore = wrongScore;
        }
        public static int TotalScore
        {
            get { return Trashbin.totalScore; }
            set { Trashbin.totalScore = value; }
        }
        public TrashStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public int Score(Trash trash){
            if (trash.Status == this.status)
            {
                return this.correctScore;
            }
            else
            {
                return this.wrongScore;
            }
        }

        public Rectangle RectDraw
        {
            get
            {
                return rectDraw;
            }
            set { this.rectDraw = value; }
        }
    }
}
