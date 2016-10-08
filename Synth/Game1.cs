using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Synthesizer;

namespace SynthTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private enum OscillatorTypes
        {
            Sine,
            Triangle,
            Square,
            Sawtooth,
            Moag
        }

        GamePadState previousGamePadState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Synth _synth;
        private OscillatorTypes _oscillatorType = OscillatorTypes.Triangle;
        private SpriteFont _spriteFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _synth = new Synth();
            ApplyOscillator();
            previousGamePadState = GamePad.GetState(PlayerIndex.One);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteFont = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get the current gamepad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            // Process input only if connected.
            if (currentState.IsConnected)
            {
                // Increase vibration if the player is tapping the A button.
                // Subtract vibration otherwise, even if the player holds down A
                if (currentState.Buttons.A == ButtonState.Pressed)
                {
                    _synth.NoteOn(0);
                }
                else
                {
                    _synth.NoteOff(0);
                }

                if (currentState.Buttons.B == ButtonState.Pressed &&
                    previousGamePadState.Buttons.B == ButtonState.Released)
                {
                    NextOscillatorType();
                }

                if (currentState.DPad.Up == ButtonState.Pressed)
                {
                    _synth.FadeInDuration = MathHelper.Clamp(_synth.FadeInDuration + 10, 0, 44100);
                }

                if (currentState.DPad.Down == ButtonState.Pressed)
                {
                    _synth.FadeInDuration = MathHelper.Clamp(_synth.FadeInDuration - 10, 0, 44100);
                }

                if (currentState.DPad.Left == ButtonState.Pressed)
                {
                    _synth.FadeOutDuration = MathHelper.Clamp(_synth.FadeOutDuration + 10, 0, 44100);
                }

                if (currentState.DPad.Right == ButtonState.Pressed)
                {
                    _synth.FadeOutDuration = MathHelper.Clamp(_synth.FadeOutDuration - 10, 0, 44100);
                }

                _synth.PitchBend = MathHelper.Clamp(currentState.ThumbSticks.Left.Y, -1.0f, 1.0f);

                // Update previous gamepad state.
                previousGamePadState = currentState;
            }
            
            _synth.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(_spriteFont, "Active Voices: " + _synth.ActiveVoicesCount, Vector2.Zero, Color.White);
            spriteBatch.DrawString(_spriteFont, "Free Voices: " + _synth.FreeVoicesCount, new Vector2(0, 20), Color.White);
            spriteBatch.DrawString(_spriteFont, "Registered Notes: " + _synth.KeyRegistryCount, new Vector2(0, 40), Color.White);
            spriteBatch.DrawString(_spriteFont, "Oscillator: " + _oscillatorType, new Vector2(600, 0), Color.White);
            spriteBatch.DrawString(_spriteFont, "Attack/FadeIn: " + (_synth.FadeInDuration == 0 ? "Disabled" : (((int)(1000 * _synth.FadeInDuration / 44100.0f)).ToString() + "ms")), new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(_spriteFont, "Release/FadeOut: " + (_synth.FadeOutDuration == 0 ? "Disabled" : (((int)(1000 * _synth.FadeOutDuration / 44100.0f)).ToString() + "ms")), new Vector2(300, 20), Color.White);
            spriteBatch.DrawString(_spriteFont, "Creating a Basic Synth in XNA 4.0 - Part III (Sample)", new Vector2(110, 101), Color.Yellow, 0f, Vector2.Zero, new Vector2(1.3f), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(_spriteFont, "Space: Change Oscillator  |  Insert and Delete: Attack  |  PageUp and PageDown: Release", new Vector2(0, 550), Color.White);
            spriteBatch.DrawString(_spriteFont, "Source at: http://www.david-gouveia.com", new Vector2(0, 580), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ApplyOscillator()
        {
            if (_oscillatorType == OscillatorTypes.Sine)
                _synth.Oscillator = Oscillator.Sine;
            else if (_oscillatorType == OscillatorTypes.Triangle)
                _synth.Oscillator = Oscillator.Triangle;
            else if (_oscillatorType == OscillatorTypes.Square)
                _synth.Oscillator = Oscillator.Square;
            else if (_oscillatorType == OscillatorTypes.Sawtooth)
                _synth.Oscillator = Oscillator.Sawtooth;
            else
                _synth.Oscillator = Oscillator.Moag;
        }

        public void NextOscillatorType()
        {
            _oscillatorType = (OscillatorTypes)(((int)_oscillatorType + 1) % 5);
            ApplyOscillator();
        }
    }
}
