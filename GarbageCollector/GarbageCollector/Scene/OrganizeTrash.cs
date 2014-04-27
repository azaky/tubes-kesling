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
    class OrganizeTrash : Scene
    {
        private Texture2D bgTexture;
        private List<Trash> trashes;
        private List<Trashbin> trashbins;
        private TrashImage trashImage;

        //private List<Texture2D> textures;
        private int score;
        SpriteFont spriteFont;
        private int norganic, ninorganic;
        public static List<Trash> _trashes = null;

        public int Norganic { get { return this.norganic; } set { this.norganic = value; } }
        public int Ninorganic { get { return this.ninorganic; } set { this.ninorganic = value; } } 

        public OrganizeTrash(GraphicsDevice dev, Game game,int norganic,int ninorganic)
        {
            device = dev;
            name = "OrganizeTrash";
            content = game.Content;
            trashes = new List<Trash>();
            trashbins = new List<Trashbin>();
            trashImage = new TrashImage(game);
            //spriteFont = new SpriteFont();
            this.Norganic = norganic;
            this.Ninorganic = ninorganic;
        }

        public void SetPosition()
        {
            int x,y,dx,dy;
            int min_x,max_x,min_y,max_y;
            min_x = 5; max_x = 700;
            min_y = 5; max_y = 400;
            x = 5; y = 5;
            dx = 130; dy = 130;
            for (int i = 0; i < trashes.Count; i++)
            {
                trashes[i].RectDraw = TrashImage.GetSize(trashes[i].Name);
                trashes[i].RectDraw = new Rectangle(x,y, trashes[i].RectDraw.Width, trashes[i].RectDraw.Height);
                y += dy;
                if (y > max_y)
                {
                    y = min_y; x += dx;
                    if (x > max_x)
                    {
                        x = min_x;
                    }
                }
            }
            for (int i = 0; i < 2; ++i)
            {
                trashbins[i].RectDraw = TrashImage.GetSize(trashbins[i].Name);
                trashbins[i].RectDraw = new Rectangle(i*500 + 100, 500, trashbins[i].RectDraw.Width, trashbins[i].RectDraw.Height);
            }
        }
        public override void Initialize()
        {
            this.LoadContent();

            Trash.hasSelected = false;
            trashes = _trashes;
            if (trashes == null)
            {
                trashes = new List<Trash>();
            }
            foreach (var trash in trashes)
            {
                trash.Status = TrashStatus.IDLE;
            }
            
            //load 2 tong
            Trashbin tb = new Trashbin(3, 0);
            tb.Name = "organic-bin";
            tb.Type = TrashType.ORGANIC;
            trashbins.Add(tb);

            tb = new Trashbin(3, 0);
            tb.Name = "inorganic-bin";
            tb.Type = TrashType.INORGANIC;
            trashbins.Add(tb);
            SetPosition();
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(device);
            spriteFont = content.Load<SpriteFont>("font/hugmetight");
            trashImage.LoadResources();
        }
        public override void Shutdown()
        {
            base.Shutdown();
        }
        public override void Draw(GameTime gametime)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(TrashImage.GetImage("bg-organize"), new Rectangle(0, 0, 800, 600), Color.White);
            foreach(var trash in trashes)
            {

                if (trash.Status != TrashStatus.DISPOSED)
                {
                    if (trash.Status == TrashStatus.IDLE)
                        spriteBatch.Draw(TrashImage.GetImage(trash.Name), trash.RectDraw, Color.White);
                    else
                    {
                        Rectangle rect = trash.RectDraw;
                        rect.Width *= 3; rect.Width /= 2;
                        rect.Height *= 3; rect.Height /= 2;
                        spriteBatch.Draw(TrashImage.GetImage(trash.Name), rect, Color.White);
                    }
                }
            }
            foreach (var trashbin in trashbins)
            {
                spriteBatch.Draw(TrashImage.GetImage(trashbin.Name), trashbin.RectDraw, Color.White);
            }
            spriteBatch.DrawString(spriteFont, "Score : "+this.score.ToString(), new Vector2(350, 0), Color.Black);
            spriteBatch.Draw(TrashImage.GetImage("cursor-organize"), TrashImage.GetSize("cursor-organize"), Color.White);
            spriteBatch.End();
        }
        public override void Update(GameTime gametime)
        {
            foreach(var trash in trashes)//(int i = 0; i < trashes.Count; i++)
            {
                if (trash.Status == TrashStatus.SELECTED)
                {
                    foreach (var trashbin in trashbins)
                    {
                        if (trashbin.RectDraw.Intersects(trash.RectDraw))
                        {
                            this.score += trashbin.Score(trash);
                            Trash.hasSelected = false;
                            trash.Status = TrashStatus.DISPOSED;
                            break;
                        }
                    }
                }
            }
            base.Update(gametime);
        }
    }
}
