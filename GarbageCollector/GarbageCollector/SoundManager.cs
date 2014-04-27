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
    class SoundManager
    {
        private int iEffectBank;
        private Song[] SoundBanks;
        private SoundEffect[] EffectBanks;
        private SoundEffectInstance[] SoundInstance;
        private int SFXDur;
        private int BackDur;

        private void pSFXFadeOut()
        {
            while (EffectVolume > 0)
            {
                EffectVolume -= 0.01f;
                Thread.Sleep(SFXDur);
            }
            if (EffectVolume == 0)
            {
                for (int i = 0; i <= iEffectBank; i++)
                {
                    SoundInstance[i].Stop();
                }
            }
        }

        private void pBackFadeOut()
        {
            while ((BackVolume > 0) && (MediaPlayer.State == MediaState.Playing))
            {
                BackVolume -= 0.01f;
                Thread.Sleep(BackDur);
            }
            if (BackVolume == 0)
                MediaPlayer.Stop();
        }

        public void SFXFadeOut(int Duration)
        {
            Thread pProses;
            SFXDur = Duration;
            pProses = new Thread(pSFXFadeOut);
            pProses.Start();
        }

        public void BackFadeOut(int Duration)
        {
            Thread pProses;
            BackDur = Duration;
            pProses = new Thread(pBackFadeOut);
            pProses.Start();
        }

        private float pEffectVolume;
        public float EffectVolume
        {
            get
            {
                return pEffectVolume;
            }
            set
            {
                for (int i = 0; i <= iEffectBank; i++)
                {
                    SoundInstance[i].Volume = value;
                }
                pEffectVolume = value;
            }
        }

        private float pBackVolume;
        public float BackVolume
        {
            get
            {
                return pBackVolume;
            }
            set
            {
                MediaPlayer.Volume = value;
                pBackVolume = value;
            }
        }

        public void CueBack(int ID)
        {
            BackVolume = 1.0f;
            MediaPlayer.Stop();
            MediaPlayer.Volume = 1.0f;
            MediaPlayer.Play(SoundBanks[ID]);
        }

        public void CueEffect(int ID)
        {
            EffectVolume = 1.0f;
            SoundEffectInstance si = EffectBanks[ID].CreateInstance();
            si.Volume = 1.0f;
            si.Play();
            //SoundInstance[ID].Stop();
            //SoundInstance[ID].Volume = 1.0f;
            //SoundInstance[ID].Play();
        }

        public void StopEffect(int ID)
        {
            SoundInstance[ID].Stop();
        }

        public void StopAllEffect()
        {
            for (int i = 0; i <= iEffectBank; i++)
                SoundInstance[i].Stop();
        }

        public void StopBack()
        {
            MediaPlayer.Stop();
        }

        public void LoadBanks()
        {
            ContentManager content;
            content = Game1.content;

            MediaPlayer.IsRepeating = true;

            SoundBanks = new Song[0];
            EffectBanks = new SoundEffect[3];
            SoundInstance = new SoundEffectInstance[3];
            
            /*
            SoundBanks[0] = content.Load<Song>("Music\\song1");
            SoundBanks[1] = content.Load<Song>("Music\\song2");
            SoundBanks[2] = content.Load<Song>("Music\\song3");
            SoundBanks[3] = content.Load<Song>("Music\\song4");
            SoundBanks[4] = content.Load<Song>("Music\\song5");
            */

            iEffectBank = 0;

            EffectBanks[0] = content.Load<SoundEffect>("sfx/smw_coin");
            EffectBanks[1] = content.Load<SoundEffect>("sfx/correct");
            EffectBanks[2] = content.Load<SoundEffect>("sfx/incorrect");

            SoundInstance[0] = EffectBanks[0].CreateInstance();
            SoundInstance[1] = EffectBanks[1].CreateInstance();
            SoundInstance[2] = EffectBanks[2].CreateInstance();
            
        }
    }
}
