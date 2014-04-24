using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GarbageCollector
{
    public abstract class Scene
    {
        public String name;
        protected GraphicsDevice device = null;
        protected SpriteBatch spriteBatch;
        protected ContentManager content;

        public Scene()
        {
        }

        public Scene(GraphicsDevice dev, Game game)
        {
            device = dev;
            spriteBatch = new SpriteBatch(dev);
            content = game.Content;
        }

        public virtual void Initialize() { }

        public virtual void Shutdown() { }

        public virtual void Draw(GameTime gametime)
        { }

        public virtual void Update(GameTime gametime)
        { }
    }
}
