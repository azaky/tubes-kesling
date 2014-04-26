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

    class TrashImage : Scene
    {
        private static Dictionary<string, Texture2D> images;
        private static Dictionary<string, Rectangle> imgsize;
        public TrashImage(Game game)
        {
            content = game.Content;
        }
        private void LoadImages()
        {
            images = new Dictionary<string, Texture2D>();
            images.Add("bg-organize",content.Load<Texture2D>("img/bg-trashorganizer"));
            images.Add("apple",content.Load<Texture2D>("img/apple"));
            images.Add("bottle",content.Load<Texture2D>("img/bottle"));
            images.Add("leaf",content.Load<Texture2D>("img/leaf"));
            images.Add("can",content.Load<Texture2D>("img/can"));
            images.Add("banana",content.Load<Texture2D>("img/banana"));
            images.Add("snack",content.Load<Texture2D>("img/snack"));
            images.Add("organic-bin", content.Load<Texture2D>("img/organic-bin"));
            images.Add("inorganic-bin", content.Load<Texture2D>("img/inorganic-bin"));
            images.Add("cursor-organize", content.Load<Texture2D>("img/cursor-organize"));
        }
        private void LoadImgSize()
        {
            int f = 4;
            imgsize = new Dictionary<string, Rectangle>();
            imgsize.Add("bg-organize", new Rectangle(0,0,800,600));
            imgsize.Add("apple", new Rectangle(0, 0, 157 / f, 246 / f));
            imgsize.Add("bottle", new Rectangle(0, 0, 136 / f, 230 / f));
            imgsize.Add("leaf", new Rectangle(0, 0, 182 / f, 172 / f));
            imgsize.Add("can", new Rectangle(0, 0, 122 / f, 181 / f));
            imgsize.Add("banana", new Rectangle(0, 0, 234 / f, 165 / f));
            imgsize.Add("snack", new Rectangle(0, 0, 178 / f, 191 / f));
            imgsize.Add("organic-bin", new Rectangle(0, 0, 135 / f * 2, 149 / f * 2));
            imgsize.Add("inorganic-bin", new Rectangle(0, 0, 170 / f *2, 149 / f*2));
            imgsize.Add("cursor-organize", new Rectangle(0, 0, 36, 31));
        }
        public void LoadResources()
        {
            this.LoadImages();
            this.LoadImgSize();
        }

        static int lastRandom = 0;
        public static string GetRandomImageName(TrashType type)
        {
            Random rnd = new Random();
            int val = lastRandom + rnd.Next(0, 10);
            val = val % 3; lastRandom = val;
            if (type == TrashType.ORGANIC){
                switch(val){
                    case 0: return "apple"; 
                    case 1: return "banana";
                    case 2: return "leaf"; 
                }
            } else {
                switch(val){
                    case 0: return "can"; 
                    case 1: return "bottle"; 
                    case 2: return "snack"; 
                }
            }
            return null;
        }
        public static Rectangle GetSize(string name)
        {
            if (TrashImage.imgsize.ContainsKey(name))
            {
                if (name.Equals("cursor-organize"))
                {
                    return TrashImage.imgsize[name] = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 36, 31);
                }
                else
                {
                    return TrashImage.imgsize[name];
                }
                
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }
        public static Texture2D GetImage(string name)
        {
            if (TrashImage.images.ContainsKey(name))
            {
                //System.Diagnostics.Debug.WriteLine("ok");
                return TrashImage.images[name];
            }
            else
            {
                return null;
            }
        }
    }
}
