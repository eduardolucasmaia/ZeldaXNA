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

namespace TutorialColisaoRetangulo
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Vari�vel para armazenar a imagem
        Texture2D img = null;
        // Vari�vel para armazenar a posi��o da sprite (ou posi��o do personagem)
        // Inicializa a vari�vel na posi��o (0,0)
        Vector2 pos1 = Vector2.Zero;
        // Constante de velocidade (5 pixels)
        const float velocidade = 5;
        // Posi��o do fantasma parado
        Vector2 pos2 = new Vector2(350, 170);
        // Flag para quando detectar uma colis�o
        Boolean colidiu = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Carrega a imagem do fantasma para vari�vel "img"
            img = Content.Load<Texture2D>(@"fantasma");
        }

        protected override void Update(GameTime gameTime)
        {
            // Recebe o estado atual do teclado (estado deste exato momento)
            KeyboardState teclado = Keyboard.GetState();
            // Se a seta esquerda estiver pressionada
            if (teclado.IsKeyDown(Keys.Left))
            {
                // Decrementa a posi��o X em 10 pixels 
                pos1.X -= velocidade; 
            }
            // Se a seta direita estiver pressionada
            if (teclado.IsKeyDown(Keys.Right))
            {
                // Incrementa a posi��o X em 10 pixels
                pos1.X += velocidade;
            }
            // Se a seta cima estiver pressionada
            if (teclado.IsKeyDown(Keys.Up))
            {
                // Decrementa a posi��o Y em 10 pixels 
                pos1.Y -= velocidade;
            }
            // Se a seta baixo estiver pressionada
            if (teclado.IsKeyDown(Keys.Down))
            {
                // Incrementa a posi��o Y em 10 pixels
                pos1.Y += velocidade;
            }

            // Testa o limite direito da tela
            if (pos1.X + img.Width > graphics.PreferredBackBufferWidth)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posi��o do fantamas no m�ximo da janela em X
                pos1.X = graphics.PreferredBackBufferWidth - img.Width;
            }
            // Testa o limite inferior da tela
            if (pos1.Y + img.Height > graphics.PreferredBackBufferHeight)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posi��o do fantamas no m�ximo da janela em Y
                pos1.Y = graphics.PreferredBackBufferHeight - img.Height;
            }
            // Testa o limite esquerdo da tela
            if (pos1.X < 0)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posi��o do fantamas no in�cio da janela em X
                pos1.X = 0;
            }
            // Testa o limite superior da tela
            if (pos1.Y < 0)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posi��o do fantamas no in�cio da janela em Y
                pos1.Y = 0;
            }

            // Ret�ngulo do personagem que se movimenta
            // � necess�rio fazer um convers�o (cast) para int, pois Vector2 � do tipo float
            Rectangle rect1 = new Rectangle((int)pos1.X, (int)pos1.Y, (int)img.Width, (int)img.Height);
            // Ret�ngulo do personagem que fica parado
            // � necess�rio fazer um convers�o (cast) para int, pois Vector2 � do tipo float
            Rectangle rect2 = new Rectangle((int)pos2.X, (int)pos2.Y, (int)img.Width, (int)img.Height);
            // Verifica se o ret�ngulo 1 colidiu com o segundo ret�ngulo
            if (rect1.Intersects(rect2))
            {
                // se aconteceu a colis�o mudamos para verdadeiro 
                colidiu = true;
            }
            else
            {
                // se n�o aconteceu a colis�o mudamos para verdadeiro 
                colidiu = false;
            }
            // chama o m�todo da superclasse passando o par�mentro de tempo do jogo
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (colidiu)
            {
                // Se aconteceu uma colis�o, pintamos a tela de salmon
                GraphicsDevice.Clear(Color.Salmon);
            }
            else
            {
                // Caso n�o aconte�a uma colis�o pintamos com a cor padr�o
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            // Inicia o bloco de desenho
            spriteBatch.Begin();
            // Desenho o fantasma que fica parado na tela
            spriteBatch.Draw(img, pos2, Color.White);
            // Desenho o fantasma que se movimenta com o teclado
            spriteBatch.Draw(img, pos1, Color.White);
            // Finaliza o bloco de desenho
            spriteBatch.End();
            // chama o m�todo da superclasse passando o par�mentro de tempo do jogo
            base.Draw(gameTime);
        }
    }
}
