using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace GarbageCollector
{
    public static class SceneManager
    {
        //ATTRIBUTES
        static private List<Scene> scenes = new List<Scene>();
        static private bool started = false;
        static public Scene activeScene = null;

        //FUNCTIONS&PROCEDURES
        static public void AddScene(Scene S)
        {
            foreach (Scene scn in scenes)
            {
                if (scn.name == S.name) return;
            }
            scenes.Add(S);
        }

        static public void Switch(String name)
        {
            foreach (Scene scene in scenes)
            {
                if (scene.name == name)
                {
                    if (activeScene != null)
                        activeScene.Shutdown();
                    activeScene = scene;
                    scene.Initialize();
                }

            }
        }

        static public void Initialize()
        {
            started = true;
        }

        static public void Update(GameTime gametime)
        {
            if (!started) return;
            if (activeScene != null)
                activeScene.Update(gametime);
        }

        static public void Draw(GameTime gametime)
        {
            if (!started) return;
            if (activeScene != null)
                activeScene.Draw(gametime);
        }

    }
}
