using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace CollisionTexture
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Initialize some variables
        Vector2 towerPos;
        Texture2D tower;
        Texture2D path;
        Texture2D map;
        RenderTarget2D colTex;

        int TileX;
        int TileY;
        int tileWidth = 30;
        int tileHeight = 30;
        bool towerActive = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //lock screen size to 800x600
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //load the textures and set the initialize the render target
            spriteBatch = new SpriteBatch(GraphicsDevice);
            path = Content.Load<Texture2D>("path");
            tower = Content.Load<Texture2D>("tower");
            map = Content.Load<Texture2D>("map");

            colTex = new RenderTarget2D(GraphicsDevice, tileWidth, tileHeight, true, SurfaceFormat.Color,  DepthFormat.Depth24);
        }

        protected override void UnloadContent()
        {
        }
        
        /*
         * Function to render a tower sized portion of the path to the render target that we can use to check if
         * the tower is on the path. Uses the path as the seed, not the map!
        */
        private Texture2D CreateCollisionTexture(float X, float Y)
        {
            GraphicsDevice.SetRenderTarget(colTex);

            GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);

            spriteBatch.Begin();

            spriteBatch.Draw(path, new Rectangle(0, 0, tileWidth, tileHeight), new Rectangle((int)(X - tileWidth / 2), (int)(Y - tileHeight / 2), tileWidth - 1, tileHeight - 1), Color.White);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            return colTex;
        }

        /*
         * Checks from the current tile position if it's a valid placement. Does this by looping through each pixel of 
         * the collision texture (called Path) to see if any pixel is white.
        */
        public void checkPlacement()
        {
            TileX = (int)Math.Floor((float)(Mouse.GetState().X / tileWidth));
            TileY = (int)Math.Floor((float)(Mouse.GetState().Y / tileHeight));

            float aXPosition = ((TileX * tileWidth) + (tileWidth / 2));
            float aYPosition = ((TileY * tileHeight) + (tileHeight / 2));

            Texture2D CollisionCheck = CreateCollisionTexture(aXPosition, aYPosition);

            int pixels = tileWidth * tileHeight;

            Color[] myColors = new Color[pixels];

            CollisionCheck.GetData<Color>(0, new Rectangle(0,0, tileWidth, tileHeight), myColors, 0, pixels);

            foreach (Color aColor in myColors)
            {
                if (aColor == Color.White)
                {
                    towerActive = false;
                    break;
                }
            }                                         
        }

        //Moves the tower on the grid
        private void moveTower()
        {  
            TileX = (int)Math.Floor((float)(Mouse.GetState().X / tileWidth));
            TileY = (int)Math.Floor((float)(Mouse.GetState().Y / tileHeight));

            towerPos.X = ((TileX * tileWidth) + (tileWidth / 2));
            towerPos.Y = ((TileY * tileHeight) + (tileHeight / 2));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            towerActive = true;
            checkPlacement();
            moveTower();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(path, Vector2.Zero, Color.White);
            spriteBatch.Draw(map, Vector2.Zero, Color.White);

            //Render the tower as grey if the move is not allowed
            if (!towerActive)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("inactive"), towerPos, null, Color.White, 0f, new Vector2(tower.Width / 2, tower.Height / 2), 1.0f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(tower, towerPos, null, Color.White, 0f, new Vector2(tower.Width / 2, tower.Height / 2), 1.0f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
