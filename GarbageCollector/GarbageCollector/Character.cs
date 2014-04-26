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
    class Character
    {
        private Point frameOffset = new Point(128, 128); // one frame is 128 pixels
        private Rectangle frameSize = new Rectangle(30, 3, 73, 121);
        private Point currentFrame = new Point(0, 0);
        private Point sheetSize = new Point(4, 4);

        private int framestart;
        private const int fps = 30;

        private Texture2D image;
        private Rectangle rectDraw;

        public Character()
        {
            framestart = 0;
        }

        public Texture2D Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public Rectangle RectDraw
        {
            get
            {
                return new Rectangle(
                    currentFrame.X * frameOffset.X + frameSize.X, //first point starts at 0, 0
                    currentFrame.Y * frameOffset.Y + frameSize.Y, //second at 0, 0
                    frameSize.Width, //third, 128, 0
                    frameSize.Height); // fourth, 0,128
            }
            set { }
        }

        public void Update(GameTime gametime)
        {
            
            //update frame
            framestart += gametime.ElapsedGameTime.Milliseconds; //starting from 0, increment frame start at the speed of milliseconds
            if (framestart > fps) //when framstart goes above the max fps (30)
            {
                framestart -= fps; //start the frameSpeed back to 1
                ++currentFrame.X; // move the source rectangle 1 frame to the right
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0; //once the source rectangle moves 4 spaces to the right, reset to the first column
                    ++currentFrame.Y; //move the source rectangle down one
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0; //when at the bottom of the sheet reset the source to the top
                    }
                }
            }
        }
    }
}
