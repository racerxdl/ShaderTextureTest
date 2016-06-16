using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShaderTextureTest {
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Effect myEff;
    Texture2D bgTest;
    private RenderTarget2D ShaderRenderTarget;
    private SpriteBatch secondarySpriteBatch;
    private Texture2D dummyTexture;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      // TODO: Add your initialization logic here
     
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);
      myEff = Content.Load<Effect>("ShaderTest");
      bgTest = Content.Load<Texture2D>("bgtest");
      Vector2 buffSize = new Vector2(320, 240);
      ShaderRenderTarget = createRenderTarget(GraphicsDevice, (int)buffSize.X, (int)buffSize.Y);
      secondarySpriteBatch = new SpriteBatch(GraphicsDevice);
      dummyTexture = new Texture2D(GraphicsDevice, (int)buffSize.X, (int)buffSize.Y);
      Color[] colors = new Color[(int)(buffSize.X * buffSize.Y)];
      for (int i = 0; i < colors.Length; i++) {
        colors[i] = Color.Wheat;
      }
      dummyTexture.SetData(colors);

      myEff.Parameters["resolution"].SetValue(buffSize);

      // TODO: use this.Content to load your game content here
    }

    public static RenderTarget2D createRenderTarget(GraphicsDevice device, int width, int height) {
      return new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
      // bool mipMap, SurfaceFormat format, DepthFormat depthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, SurfaceType surfaceType);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent() {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();
      //drawFrameBuffer(gameTime);  //  This does not work
      base.Update(gameTime);
    }

    private void drawFrameBuffer(GameTime gameTime) {
      GraphicsDevice.SetRenderTarget(ShaderRenderTarget);
      GraphicsDevice.Clear(Color.CornflowerBlue);

      myEff.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
      secondarySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
      myEff.CurrentTechnique.Passes[0].Apply();
      secondarySpriteBatch.Draw(dummyTexture, new Rectangle(0, 0, ShaderRenderTarget.Width, ShaderRenderTarget.Height), Color.White);
      secondarySpriteBatch.End();

      GraphicsDevice.SetRenderTarget(null);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      // TODO: Add your drawing code here
      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
      //drawFrameBuffer(gameTime);  //  This does work
      spriteBatch.Draw(bgTest, new Rectangle(0, 0, bgTest.Width, bgTest.Height), Color.White);
      spriteBatch.Draw(ShaderRenderTarget, new Rectangle(0, 0, ShaderRenderTarget.Width, ShaderRenderTarget.Height), Color.White);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
