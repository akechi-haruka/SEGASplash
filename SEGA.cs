using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SEGA {
    public class SEGA : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private VideoPlayer videoPlayer;
        private Vector2 size;
        private bool videoStarted;
        private Options opts;
        private DateTime start;
        private double lastWndCheck;
        private double totalTime;
        private bool drawStarted;

        public SEGA(Options opts) {
            this.opts = opts;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize() {
            base.Initialize();
            if (!opts.Windowed) {
                _graphics.PreferredBackBufferWidth = Screen.PrimaryScreen.Bounds.Width;
                _graphics.PreferredBackBufferHeight = Screen.PrimaryScreen.Bounds.Height;
                _graphics.HardwareModeSwitch = false;
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();
            }
            size = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Console.WriteLine(size);
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {

            lastWndCheck += gameTime.ElapsedGameTime.TotalMilliseconds;
            totalTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)) {
                Exit();
            }

            bool IsWindowDetected = false;
            if (opts.WaitForWindow != null && lastWndCheck > 500) {
                foreach (Process pList in Process.GetProcesses()) {
                    if (pList.MainWindowTitle.Contains(opts.WaitForWindow)) {
                        IsWindowDetected = true;
                        break;
                    }
                }
                lastWndCheck = 0;
            }

            if (videoStarted && videoPlayer.State == MediaState.Stopped) {
                if (totalTime > opts.MinimumWait * 1000) {
                    if (opts.WaitForWindow == null || IsWindowDetected) {
                        Exit();
                    }
                }
            }
            if (opts.SkipOnWindow && IsWindowDetected) {
                Exit();
            }

            if (!videoStarted && drawStarted) {
                start = DateTime.Now;
                videoPlayer = new VideoPlayer();
                videoPlayer.Volume = opts.Volume / 100F;
                videoPlayer.Play(Content.Load<Video>("SEGA"));
                videoStarted = true;
            }

            base.Update(gameTime);
        }

        

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            drawStarted = true;

            _spriteBatch.Begin();
            Texture2D tex = videoPlayer?.GetTexture();
            if (tex != null) {
                _spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
