using System;
using System.Linq;
using InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TearableCloth
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Cloth _cloth;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new[] {Color.White});
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _cloth = new Cloth();
        }

        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();

            if (KeyboardHandler.IsKeyPressed(Keys.Escape))
            {
                Exit();
            }

            _cloth.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var constraint in _cloth.Points.SelectMany(point => point.Constraints))
            {
                DrawLine(_spriteBatch, constraint.Point1.PreviousPosition, constraint.Point2.PreviousPosition);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            // Calculate angle to rotate line.
            var edge = Vector2.Subtract(end, start);
            var angle = (float) Math.Atan2(edge.Y, edge.X);
            var rectangle = new Rectangle((int) start.X, (int) start.Y,
                                          (int) edge.Length(), 1);

            sb.Draw(_texture,
                    rectangle,
                    null,
                    Color.Red,
                    angle,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    0);
        }
    }
}