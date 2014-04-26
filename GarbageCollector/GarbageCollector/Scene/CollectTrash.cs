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
    class CollectTrash : Scene
    {
        private Texture2D bgTexture;
        private SpriteFont spriteFont;

        private Character character;
        private Vector2 characterPosition;

        private List<Trash> trashes;
        private TrashImage trashImage;
        
        public CollectTrash(GraphicsDevice dev, Game game)
        {
            device = dev;
            name = "CollectTrash";
            content = game.Content;
            character = new Character();
            trashes = new List<Trash>();
            trashImage = new TrashImage(game);
        }

        public override void Initialize()
        {
            this.LoadContent();
            trashImage.LoadResources();

            characterPosition = new Vector2(100, 100);

            //randomize sampah
            Random rnd = new Random();
            for (int i = 0; i < 100; ++i)
            {
                Trash t = new Trash();
                t.Type = rnd.Next(2) == 1 ? TrashType.ORGANIC : TrashType.INORGANIC;
                t.Name = TrashImage.GetRandomImageName(t.Type);
                trashes.Add(t);
                System.Diagnostics.Debug.WriteLine(t.Name);
            }

            int x, y, dx, dy;
            int min_x, max_x, min_y, max_y;
            min_x = 5; max_x = 700;
            min_y = 5; max_y = 400;
            x = 5; y = 5;
            dx = 70; dy = 70;
            for (int i = 0; i < trashes.Count; i++)
            {
                trashes[i].RectDraw = TrashImage.GetSize(trashes[i].Name);
                trashes[i].RectDraw = new Rectangle(x, y, trashes[i].RectDraw.Width, trashes[i].RectDraw.Height);
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
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(device);
            spriteFont = content.Load<SpriteFont>("font/hugmetight");
            character.Image = content.Load<Texture2D>("img/character");
        }
        public override void Shutdown()
        {
            base.Shutdown();
        }
        public override void Draw(GameTime gametime)
        {
            spriteBatch.Begin();
            
            spriteBatch.Draw(character.Image, characterPosition, character.RectDraw, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            foreach (var trash in trashes)
            {
                if (trash.Status != TrashStatus.DISPOSED)
                    spriteBatch.Draw(TrashImage.GetImage(trash.Name), trash.RectDraw, Color.White);
            }
            spriteBatch.Draw(TrashImage.GetImage("cursor-organize"), TrashImage.GetSize("cursor-organize"), Color.White);
//            spriteBatch.Draw(TrashImage.GetImage("bg-organize"), new Rectangle(0, 0, 800, 600), Color.White);

            spriteBatch.End();
        }

        public override void Update(GameTime gametime)
        {
            Rectangle bound = new Rectangle((int)characterPosition.X, (int)characterPosition.Y, character.RectDraw.Width, character.RectDraw.Height);
            foreach (var trash in trashes)//(int i = 0; i < trashes.Count; i++)
            {
                if (trash.Status != TrashStatus.DISPOSED)
                {
                    //System.Diagnostics.Debug.WriteLine("bound(" + bound.X + ", " + bound.Y + ", " + bound.Width + ", " + bound.Height + "), " +
                    //    "trash(" + trash.RectDraw.X + ", " + trash.RectDraw.Y + ", " + trash.RectDraw.Width + ", " + trash.RectDraw.Height + ")");
                    if (bound.Intersects(trash.RectDraw))
                    {
                        Trash.hasSelected = false;
                        trash.Status = TrashStatus.DISPOSED;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                characterPosition.Y += 100 * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                characterPosition.Y -= 100 * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                characterPosition.X -= 100 * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                characterPosition.X += 100 * (float)gametime.ElapsedGameTime.TotalSeconds;
            }

            character.Update(gametime);

            base.Update(gametime);
        }
    }
}
