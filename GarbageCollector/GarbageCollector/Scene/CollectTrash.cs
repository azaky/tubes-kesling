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
        private const float TRASH_SPEED = 250;
        private const float TRASH_FREQUENCY = 1f;
        private const float CHAR_SPEED = 250;
        private const int GAME_TIME = 5;
        private const int TILE_SIZE = 80;

        private SoundManager sm;
        private Texture2D sidewalk;
        private Texture2D asphalt;
        private Texture2D whiteTile;
        private Texture2D blackTile;
        private SpriteFont spriteFont;

        private Character character;
        private Vector2 characterPosition;

        private List<Trash> trashes;
        private List<Trash> collected;
        private TrashImage trashImage;
        private List<Trashbin> trashbins;

        private int Norganik, Ninorganik;
        private int leftTile;
        private int leftBin;
        private int parity;
        
        public CollectTrash(GraphicsDevice dev, Game game)
        {
            device = dev;
            name = "CollectTrash";
            content = game.Content;
            character = new Character();
            trashes = new List<Trash>();
            collected = new List<Trash>();
            trashImage = new TrashImage(game);
            trashbins = new List<Trashbin>();
            sm = new SoundManager();
        }

        public override void Initialize()
        {
            this.LoadContent();
            trashImage.LoadResources();

            characterPosition = new Vector2(50, 100);
            leftTile = 0;
            leftBin = (int)(TRASH_SPEED * GAME_TIME);
            parity = 0;

            Trash.hasSelected = true;

            Trashbin tb = new Trashbin(3, 0);
            tb.Name = "organic-bin";
            tb.Type = TrashType.ORGANIC;
            trashbins.Add(tb);

            tb = new Trashbin(3, 0);
            tb.Name = "inorganic-bin";
            tb.Type = TrashType.INORGANIC;
            trashbins.Add(tb);

            //sound effects
            sm.LoadBanks();
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(device);
            spriteFont = content.Load<SpriteFont>("font/hugmetight");
            character.Image = content.Load<Texture2D>("img/character");
            sidewalk = content.Load<Texture2D>("img/sidewalk_tile");
            asphalt = content.Load<Texture2D>("img/asphalt");
            whiteTile = content.Load<Texture2D>("img/white_tile");
            blackTile = content.Load<Texture2D>("img/black_tile");
        }
        public override void Shutdown()
        {
            base.Shutdown();
        }
        public override void Draw(GameTime gametime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(asphalt, new Rectangle(leftTile, 0, 900, 100), Color.White);
            for (int i = 0; i < 1000 / TILE_SIZE; i++)
            {
                if ((i + parity) % 2 == 0)
                {
                    spriteBatch.Draw(blackTile, new Rectangle(leftTile + i * TILE_SIZE, 70, TILE_SIZE, 30), Color.White);
                }
                else
                {
                    spriteBatch.Draw(whiteTile, new Rectangle(leftTile + i * TILE_SIZE, 70, TILE_SIZE, 30), Color.White);
                }
            }
            for (int i = 0; i < 1000 / TILE_SIZE; i++)
            {
                for (int j = 0; j < 800 / TILE_SIZE; j++)
                {
                    spriteBatch.Draw(sidewalk, new Rectangle(leftTile + i * TILE_SIZE, 100 + j * TILE_SIZE, TILE_SIZE, TILE_SIZE), Color.White);
                }
            }

            spriteBatch.Draw(character.Image, characterPosition, character.RectDraw, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            foreach (var trash in trashes)
            {
                if (trash.Status != TrashStatus.DISPOSED)
                    spriteBatch.Draw(TrashImage.GetImage(trash.Name), trash.RectDraw, Color.White);
            }

            foreach (var trashbin in trashbins)
            {
                trashbin.RectDraw = new Rectangle(trashbin.Type == TrashType.ORGANIC ? leftBin + 70 : leftBin + 150, 90, TrashImage.GetSize(trashbin.Name).Width, TrashImage.GetSize(trashbin.Name).Height);
                if (trashbin.RectDraw.X < 800)
                {
                    spriteBatch.Draw(TrashImage.GetImage(trashbin.Name), trashbin.RectDraw, Color.White);
                }
            }

            spriteBatch.Draw(TrashImage.GetImage("cursor-organize"), TrashImage.GetSize("cursor-organize"), Color.White);

            //spriteBatch.DrawString(spriteFont, "Organik : " + Norganik + ", Inorganik : " + Ninorganik, new Vector2(350, 0), Color.White);
            spriteBatch.DrawString(spriteFont, "Waktu   : " + (GAME_TIME - gametime.TotalGameTime.Seconds) + " detik", new Vector2(550, 0), Color.White);

            spriteBatch.End();
        }

        public override void Update(GameTime gametime)
        {
            //Kalo udah abis, ganti scene
            if (gametime.TotalGameTime.Seconds >= GAME_TIME)
            {
                OrganizeTrash._trashes = collected;
                SceneManager.Switch("OrganizeTrash");
            }

            leftTile -= (int)((float)gametime.ElapsedGameTime.TotalSeconds * TRASH_SPEED);
            if (leftTile < -TILE_SIZE)
            {
                leftTile += TILE_SIZE;
                parity = 1 - parity;
            }
            leftBin -= (int)((float)gametime.ElapsedGameTime.TotalSeconds * TRASH_SPEED);

            Rectangle bound = new Rectangle((int)characterPosition.X, (int)characterPosition.Y, character.RectDraw.Width, character.RectDraw.Height);
            foreach (var trash in trashes)//(int i = 0; i < trashes.Count; i++)
            {
                if (trash.Status != TrashStatus.DISPOSED)
                {
                    //Gerakin sampah
                    Rectangle position = trash.RectDraw;
                    position.X -= (int) (TRASH_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds);
                    trash.RectDraw = position;

                    if (bound.Intersects(trash.RectDraw))
                    {
                        trash.Status = TrashStatus.DISPOSED;
                        collected.Add(trash);
                        if (trash.Type == TrashType.ORGANIC)
                        {
                            Norganik++;
                        }
                        else
                        {
                            Ninorganik++;
                        }
                        //bunyi?
                        sm.CueEffect(0);
                    }

                    //hancurin kalo udah nggak keliatan
                    if (trash.RectDraw.X + trash.RectDraw.Width < 0)
                    {
                        trash.Status = TrashStatus.DISPOSED;
                    }
                }
            }

            //make some new trashes
            Random rnd = new Random((int)DateTime.Now.Ticks);
            //Debug.WriteLine("Ticks = " + (int)DateTime.Now.Ticks);
            //Debug.WriteLine("humbala = " + (60000 * gametime.TotalGameTime.Minutes + 1000 * gametime.TotalGameTime.Seconds + gametime.TotalGameTime.Milliseconds));
            //Debug.WriteLine("huba = " + gametime.ElapsedGameTime.TotalSeconds);
            if (rnd.NextDouble() < TRASH_FREQUENCY * gametime.ElapsedGameTime.TotalSeconds)
            {
                Trash t = new Trash();
                t.Type = rnd.Next(2) == 1 ? TrashType.ORGANIC : TrashType.INORGANIC;
                t.Name = TrashImage.GetRandomImageName(t.Type);
                t.RectDraw = TrashImage.GetSize(t.Name);
                do
                {
                    t.RectDraw = new Rectangle(800, rnd.Next(90, 500), t.RectDraw.Width, t.RectDraw.Height);
                } while (t.RectDraw.Intersects(trashbins[0].RectDraw) || t.RectDraw.Intersects(trashbins[1].RectDraw));
                trashes.Add(t);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (characterPosition.Y < 450)
                {
                    characterPosition.Y += CHAR_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (characterPosition.Y > 30)
                {
                    characterPosition.Y -= CHAR_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds;
                }
            }

            character.Update(gametime);

            base.Update(gametime);
        }
    }
}
