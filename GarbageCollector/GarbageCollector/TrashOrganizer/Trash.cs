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
    
    enum TrashStatus
    {
        IDLE, SELECTED, DISPOSED
    }
    enum TrashType
    {
        ORGANIC,PAPER,PLASTIC
    }
    class Trash
    {
        public static bool IsInRect(Vector2 v, Rectangle rect)
        {
            return (v.X >= rect.X && v.Y >= rect.Y && v.X <= rect.X + rect.Width && v.Y <= rect.Y + rect.Height);
        }
        
        private Vector2 pos;
        private int textureId;
        private TrashStatus status;
        private Rectangle rectDraw;
        public static bool hasSelected; //cuman satu yang bisa diselected

        public Vector2 Pos
        {
            get { return this.pos; }
            set { this.pos = value; }
        }
        public int TextureId
        {
            get { return this.textureId; }
            set { this.textureId = value; }
        }
        public TrashStatus Status
        {
            get { UpdateStatus(); return this.status; }
            set { this.status = value; }
        }
        public Rectangle RectDraw
        {
            get
            {
                UpdateStatus();
                switch (this.status){
                    case TrashStatus.DISPOSED:
                    case TrashStatus.IDLE:
                        break;
                    case TrashStatus.SELECTED:
                        rectDraw.X = (int) (Mouse.GetState().X - 0.5 * rectDraw.Width); rectDraw.Y = (int)(Mouse.GetState().Y - 0.5 * rectDraw.Height);
                        break;
                }
                return rectDraw;
            }
            set { this.rectDraw = value; }
        }
        public void UpdateStatus()
        {
            var isLeftPressed = Mouse.GetState().LeftButton == ButtonState.Pressed;
            Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            var isMouseInObject = Trash.IsInRect(mousePos, rectDraw);

            switch (this.status)
            {
                case TrashStatus.DISPOSED:
                    break;
                case TrashStatus.SELECTED:
                    if (!isLeftPressed || !isMouseInObject)
                    {
                        this.status = TrashStatus.IDLE;
                        hasSelected = false;
                    }
                    break;
                case TrashStatus.IDLE:
                    if (isLeftPressed && isMouseInObject && !hasSelected)
                    {
                        this.status = TrashStatus.SELECTED;
                        hasSelected = true;
                    }
                    break;
            }
        }
        public Trash() { pos.X = pos.Y = 0; rectDraw = new Rectangle(0, 0, 0, 0); status = TrashStatus.IDLE; }
    }
}
