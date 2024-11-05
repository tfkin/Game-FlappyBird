using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using G7FlappyBird.Scenes;
using Microsoft.Xna.Framework.Content;

namespace G7FlappyBird.Scenes
{
    public class AboutScene : IScene
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        private Texture2D _backgroundTexture;
        private Texture2D _baseTexture;
        private Texture2D _logoTexture;
        private Texture2D _birdTexture;
        private Texture2D _creatorsTexture;
        private Texture2D _kinTexture;
        private Texture2D _mateoTexture;
        private Texture2D _menuButtonTexture;
        private Texture2D _kinPicture;
        private Texture2D _mateoPicture;
        private float baseScale;
        private MouseState _previousMouseState;

        private Vector2 _logoPosition;
        private Vector2 _birdPosition;
        private Vector2 _creatorsPosition;
        private Vector2 _kinPosition;
        private Vector2 _mateoPosition;
        private Vector2 _menuButtonPosition;
        private Vector2 _basePosition;

        private Rectangle _kinHitbox;
        private Rectangle _mateoHitbox;
        private Rectangle _menuButtonHitbox;

        private bool _showKinPicture = false;
        private bool _showMateoPicture = false;

        private float _buttonSpacing = 10f;

        private SceneManager _sceneManager;

        public AboutScene(GraphicsDeviceManager graphics, ContentManager content, SceneManager sceneManager)
        {
            _graphics = graphics;
            _content = content;
            _sceneManager = sceneManager;
        }

        public void Initialize()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
        }

        public void LoadContent()
        {
            _backgroundTexture = _content.Load<Texture2D>("background-day");
            _baseTexture = _content.Load<Texture2D>("base");
            _logoTexture = _content.Load<Texture2D>("flappybird-logo");
            _birdTexture = _content.Load<Texture2D>("yellowbird-midflap");
            _creatorsTexture = _content.Load<Texture2D>("creators");
            _kinTexture = _content.Load<Texture2D>("creatorKin");
            _mateoTexture = _content.Load<Texture2D>("creatorMateo");
            _menuButtonTexture = _content.Load<Texture2D>("btnMenu");
            _kinPicture = _content.Load<Texture2D>("picKin");
            _mateoPicture = _content.Load<Texture2D>("picMateo");

            float logoCenter = (_graphics.PreferredBackBufferWidth - _logoTexture.Width) / 2;
            _logoPosition = new Vector2(logoCenter, 20);
            _birdPosition = new Vector2(logoCenter + _logoTexture.Width + 10, _logoPosition.Y + _logoTexture.Height / 2 - _birdTexture.Height / 2);

            float buttonCenterX = (_graphics.PreferredBackBufferWidth - _creatorsTexture.Width) / 2;
            float kinAndMateoStartX = buttonCenterX + 15;
            _creatorsPosition = new Vector2(buttonCenterX, _graphics.PreferredBackBufferHeight / 2 - 80);
            _kinPosition = new Vector2(kinAndMateoStartX, _creatorsPosition.Y + _creatorsTexture.Height + _buttonSpacing);
            _mateoPosition = new Vector2(kinAndMateoStartX, _kinPosition.Y + _kinTexture.Height + _buttonSpacing);
            _menuButtonPosition = new Vector2(buttonCenterX, _mateoPosition.Y + _mateoTexture.Height + _buttonSpacing);

            // Define hitboxes for interactive elements
            _kinHitbox = new Rectangle(_kinPosition.ToPoint(), new Point(_kinTexture.Width, _kinTexture.Height));
            _mateoHitbox = new Rectangle(_mateoPosition.ToPoint(), new Point(_mateoTexture.Width, _mateoTexture.Height));
            _menuButtonHitbox = new Rectangle(_menuButtonPosition.ToPoint(), new Point(_menuButtonTexture.Width, _menuButtonTexture.Height));

            _basePosition = new Vector2(0, _graphics.PreferredBackBufferHeight - 112);
            baseScale = (_graphics.PreferredBackBufferHeight * 0.125f) / _baseTexture.Height;
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            bool isMouseClicked = currentMouseState.LeftButton == ButtonState.Pressed &&
                                  _previousMouseState.LeftButton == ButtonState.Released;

            if (_showKinPicture || _showMateoPicture)
            {
                // Allow clicking on the sides to return to the About scene
                if (isMouseClicked)
                {
                    _showKinPicture = false;
                    _showMateoPicture = false;
                }
            }
            else
            {
                // Handle button clicks here
                if (isMouseClicked)
                {
                    if (_kinHitbox.Contains(currentMouseState.Position))
                    {
                        _showKinPicture = true;
                    }
                    else if (_mateoHitbox.Contains(currentMouseState.Position))
                    {
                        _showMateoPicture = true;
                    }
                    else if (_menuButtonHitbox.Contains(currentMouseState.Position))
                    {
                        _sceneManager.ChangeScene("MenuScene");
                    }
                }
            }
            _previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.Draw(_baseTexture, new Rectangle(0, _graphics.PreferredBackBufferHeight - (int)(_baseTexture.Height * baseScale), _graphics.PreferredBackBufferWidth, (int)(_baseTexture.Height * baseScale)), Color.White);
            spriteBatch.Draw(_logoTexture, _logoPosition, Color.White);
            spriteBatch.Draw(_birdTexture, _birdPosition, Color.White);

            if (_showKinPicture)
            {
                // Center the picture
                Vector2 kinPicturePosition = new Vector2((_graphics.PreferredBackBufferWidth - _kinPicture.Width) / 2, (_graphics.PreferredBackBufferHeight - _kinPicture.Height) / 2 + 50);
                spriteBatch.Draw(_kinPicture, kinPicturePosition, Color.White);
            }
            else if (_showMateoPicture)
            {
                // Center the picture
                Vector2 mateoPicturePosition = new Vector2((_graphics.PreferredBackBufferWidth - _mateoPicture.Width) / 2, (_graphics.PreferredBackBufferHeight - _mateoPicture.Height) / 2 + 50);
                spriteBatch.Draw(_mateoPicture, mateoPicturePosition, Color.White);
            }
            else
            {
                // Draw the About scene elements
                spriteBatch.Draw(_creatorsTexture, _creatorsPosition, Color.White);
                spriteBatch.Draw(_kinTexture, _kinPosition, Color.White);
                spriteBatch.Draw(_mateoTexture, _mateoPosition, Color.White);
                spriteBatch.Draw(_menuButtonTexture, _menuButtonPosition, Color.White);
            }

            spriteBatch.End();
        }
    }
}
