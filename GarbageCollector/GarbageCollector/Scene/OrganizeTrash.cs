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
        private List<Texture2D> textures;

        public OrganizeTrash(GraphicsDevice dev, Game game)
        {
            device = dev;
            name = "OrganizeTrash";
            content = game.Content;
            trashes = new List<Trash>();
            textures = new List<Texture2D>();
        }

        public override void Initialize()
        {
            this.LoadContent();
            //load 6 buah sampah
            for (int i = 0; i < 6; ++i)
            {
                Trash t = new Trash();
                t.RectDraw = new Rectangle(400,10+i*100,100,100);
                t.Pos = new Vector2(400, 10 + i * 105);
                t.TextureId = i % 3;
                trashes.Add(t);
            }
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(device);
            bgTexture = content.Load<Texture2D>("EzekielJackson4");
            textures.Add(content.Load<Texture2D>("img/sampah-1"));
            textures.Add(content.Load<Texture2D>("img/sampah-2"));
            textures.Add(content.Load<Texture2D>("img/sampah-3"));
            textures.Add(content.Load<Texture2D>("img/tongsampah-1"));
            textures.Add(content.Load<Texture2D>("img/tongsampah-2"));
            textures.Add(content.Load<Texture2D>("img/tongsampah-3"));
        }
        public override void Shutdown()
        {
            base.Shutdown();
        }
        public override void Draw(GameTime gametime)
        {
            
            spriteBatch.Begin();
            //spriteBatch.Draw(bgTexture, new Rectangle(0, 0, 800, 600), Color.White);
            int i = 0;
            foreach(var trash in trashes)
            {
                spriteBatch.Draw(textures[trash.TextureId], trash.RectDraw, Color.White);
                if (i == 0) 
                    System.Diagnostics.Debug.WriteLine(trash.RectDraw.X + " " + trash.RectDraw.Y);
                i++;
            }
            spriteBatch.End();
        }
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }
    }
}
