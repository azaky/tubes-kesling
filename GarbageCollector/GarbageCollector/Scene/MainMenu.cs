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
    class MainMenu : Scene
    {
        private Texture2D bgTexture;

        public MainMenu(GraphicsDevice dev, Game game)
        {
            device = dev;
            name = "MainMenu";
            spriteBatch = new SpriteBatch(dev);
            content = game.Content;
        }

        public override void Initialize()
        {
            bgTexture = content.Load<Texture2D>("Desert");
        }

        public void LoadContent()
        {
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Draw(GameTime gametime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gametime)
        {
            
        }
    }
}