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
        // Variável para armazenar a imagem
        Texture2D img = null;
        // Variável para armazenar a posição da sprite (ou posição do personagem)
        // Inicializa a variável na posição (0,0)
        Vector2 pos1 = Vector2.Zero;
        // Constante de velocidade (5 pixels)
        const float velocidade = 5;
        // Posição do fantasma parado
        Vector2 pos2 = new Vector2(350, 170);
        // Flag para quando detectar uma colisão
        Boolean colidiu = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Carrega a imagem do fantasma para variável "img"
            img = Content.Load<Texture2D>(@"fantasma");
        }

        protected override void Update(GameTime gameTime)
        {
            // Recebe o estado atual do teclado (estado deste exato momento)
            KeyboardState teclado = Keyboard.GetState();
            // Se a seta esquerda estiver pressionada
            if (teclado.IsKeyDown(Keys.Left))
            {
                // Decrementa a posição X em 10 pixels 
                pos1.X -= velocidade; 
            }
            // Se a seta direita estiver pressionada
            if (teclado.IsKeyDown(Keys.Right))
            {
                // Incrementa a posição X em 10 pixels
                pos1.X += velocidade;
            }
            // Se a seta cima estiver pressionada
            if (teclado.IsKeyDown(Keys.Up))
            {
                // Decrementa a posição Y em 10 pixels 
                pos1.Y -= velocidade;
            }
            // Se a seta baixo estiver pressionada
            if (teclado.IsKeyDown(Keys.Down))
            {
                // Incrementa a posição Y em 10 pixels
                pos1.Y += velocidade;
            }

            // Testa o limite direito da tela
            if (pos1.X + img.Width > graphics.PreferredBackBufferWidth)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posição do fantamas no máximo da janela em X
                pos1.X = graphics.PreferredBackBufferWidth - img.Width;
            }
            // Testa o limite inferior da tela
            if (pos1.Y + img.Height > graphics.PreferredBackBufferHeight)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posição do fantamas no máximo da janela em Y
                pos1.Y = graphics.PreferredBackBufferHeight - img.Height;
            }
            // Testa o limite esquerdo da tela
            if (pos1.X < 0)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posição do fantamas no início da janela em X
                pos1.X = 0;
            }
            // Testa o limite superior da tela
            if (pos1.Y < 0)
            {
                // Se colidiu faz alguma coisa:
                // fixa a posição do fantamas no início da janela em Y
                pos1.Y = 0;
            }

            // Retângulo do personagem que se movimenta
            // É necessário fazer um conversão (cast) para int, pois Vector2 é do tipo float
            Rectangle rect1 = new Rectangle((int)pos1.X, (int)pos1.Y, (int)img.Width, (int)img.Height);
            // Retângulo do personagem que fica parado
            // É necessário fazer um conversão (cast) para int, pois Vector2 é do tipo float
            Rectangle rect2 = new Rectangle((int)pos2.X, (int)pos2.Y, (int)img.Width, (int)img.Height);
            // Verifica se o retângulo 1 colidiu com o segundo retângulo
            if (rect1.Intersects(rect2))
            {
                // se aconteceu a colisão mudamos para verdadeiro 
                colidiu = true;
            }
            else
            {
                // se não aconteceu a colisão mudamos para verdadeiro 
                colidiu = false;
            }
            // chama o método da superclasse passando o parâmentro de tempo do jogo
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (colidiu)
            {
                // Se aconteceu uma colisão, pintamos a tela de salmon
                GraphicsDevice.Clear(Color.Salmon);
            }
            else
            {
                // Caso não aconteça uma colisão pintamos com a cor padrão
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
            // chama o método da superclasse passando o parâmentro de tempo do jogo
            base.Draw(gameTime);
        }
    }
}
