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
using WindowsGame1;

namespace ZeldaPC
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {


        #region Variaveis
        GraphicsDeviceManager graphics;
        Mapa mapa = new Mapa(@"Mapas\TextFile1.txt");
        SpriteBatch spriteBatch;
        Color chao, parede;
        bool Iniciou;
        Double counterTime = 0;
        Vector2 pos;
        Vector2 posSenario = new Vector2(0, 0);

        #region Velocidades
        int velocidadeInicial = 2;
        int VelocidadeXplayer;
        int VelocidadeYplayer;
        #endregion

        Texture2D quad;
        Texture2D senario, mapa00000;
        Texture2D senarioSuperior, mapa00001;
        int tamanhoMapaX = 960;
        int tamanhoMapaY = 512;
        SpriteFont font;
        Rectangle ret1player;
        enum estado { rodando, pause, gameover, menu };
        estado estadoAtual;

        #region Player
        enum posicao { cima, cimaandando, baixo, baixoandando, esquerda, esquerdaandando, direita, direitaandando };
        posicao posicaoAtual;
        Texture2D playerCima, AndandoCima00, AndandoCima01, playerBaixo, AndandoBaixo00, AndandoBaixo01;
        #endregion

        #region Quadrado
        int contador = 0;
        int posRelandoX;
        int posRelandoY;
        bool[] posEstado;
        Vector2[] posd;
        Vector2[] posu;
        Vector2[] posp;
        Vector2[] posl;
        Vector2[] posr;
        int limiteLeitura = 1000;
        bool podeAndarX = true;
        bool podeAndarY = true;
        bool relandoBaixo;
        bool relandoCima;
        bool relandoEsquerda;
        bool relandoDireita;
        #endregion

        #region Troca de Mapa
        bool[] TemTeleportEsquerda;
        bool[] TemTeleportDireita;
        bool[] TemTeleportCima;
        bool[] TemTeleportBaixo;
        Vector2[] teleportMapaBaixo;
        Vector2[] teleportMapaEsquerda;
        Vector2[] teleportMapaDireita;
        Vector2[] teleportMapaCima;
        #endregion


        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        //############################################################



        protected override void Initialize()
        {
            VelocidadeXplayer = velocidadeInicial;
            VelocidadeYplayer = velocidadeInicial;
            graphics.PreferredBackBufferHeight = tamanhoMapaY;
            graphics.PreferredBackBufferWidth = tamanhoMapaX;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Iniciou = false;
            estadoAtual = estado.menu;
            posicaoAtual = posicao.cima;
            base.Initialize();
        }


        //############################################################


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            parede = Color.Blue;
            chao = Color.White;
            quad = Content.Load<Texture2D>("quad");
            contador = 0;

            #region Variaveis
            posEstado = new bool[limiteLeitura];
            posd = new Vector2[limiteLeitura];
            posu = new Vector2[limiteLeitura];
            posp = new Vector2[limiteLeitura];
            posl = new Vector2[limiteLeitura];
            posr = new Vector2[limiteLeitura];

            teleportMapaEsquerda = new Vector2[limiteLeitura];
            TemTeleportEsquerda = new bool[limiteLeitura];
            teleportMapaDireita = new Vector2[limiteLeitura];
            TemTeleportDireita = new bool[limiteLeitura];
            teleportMapaCima = new Vector2[limiteLeitura];
            TemTeleportCima = new bool[limiteLeitura];
            teleportMapaBaixo = new Vector2[limiteLeitura];
            TemTeleportBaixo = new bool[limiteLeitura];

            #endregion

            #region Carregando Mapas
            mapa00000 = Content.Load<Texture2D>("senario");
            mapa00001 = Content.Load<Texture2D>("senarioSuperior");
            font = Content.Load<SpriteFont>("SpriteFont1");
            senario = mapa00000;
            senarioSuperior = mapa00001;
            #endregion

            #region Animação do Player
            playerCima = Content.Load<Texture2D>(@"player\ParadoCima");
            playerBaixo = Content.Load<Texture2D>(@"player\ParadoBaixo");
            AndandoCima00 = Content.Load<Texture2D>(@"player\AndandoCima00");
            AndandoCima01 = Content.Load<Texture2D>(@"player\AndandoCima01");
            AndandoBaixo00 = Content.Load<Texture2D>(@"player\AndandoBaixo00");
            AndandoBaixo01 = Content.Load<Texture2D>(@"player\AndandoBaixo01");
            #endregion

            #region Leitura da localização dos Caracteres
            for (int y = 0; y < mapa.Tamanho_Y(); y++)
            {
                for (int x = 0; x < mapa.Tamanho_X(); x++)
                {
                    if (mapa.array[x, y] == '1' && posEstado[contador] == false) { posEstado[contador] = true; posp[contador] = new Vector2(x * quad.Width, y * quad.Height); }
                    if (Iniciou == false && mapa.array[x, y] == '@') { pos = new Vector2(x * quad.Width, y * quad.Height); }
                    if (mapa.array[x, y] == 'h' && TemTeleportEsquerda[contador] == false) { TemTeleportEsquerda[contador] = true;  teleportMapaEsquerda[contador] = new Vector2(x * quad.Width, y * quad.Height); }
                    if (mapa.array[x, y] == 'k' && TemTeleportDireita[contador] == false) { TemTeleportDireita[contador] = true; teleportMapaDireita[contador] = new Vector2(x * quad.Width, y * quad.Height); }
                    if (mapa.array[x, y] == 'u' && TemTeleportCima[contador] == false) { TemTeleportCima[contador] = true; teleportMapaCima[contador] = new Vector2(x * quad.Width, y * quad.Height); }
                    if (mapa.array[x, y] == 'j' && TemTeleportBaixo[contador] == false) { TemTeleportBaixo[contador] = true; teleportMapaBaixo[contador] = new Vector2(x * quad.Width, y * quad.Height); }
                    contador++;
                }
            }
            #endregion


        }


        //############################################################


        protected override void UnloadContent()
        {
        }


        //############################################################


        protected override void Update(GameTime gameTime)
        {

            Iniciou = true;

            counterTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            //if (estadoAtual == estado.rodando) { 


            #region Leitura do Teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) { pos.X -= VelocidadeXplayer; posicaoAtual = posicao.esquerdaandando; }

            else if (Keyboard.GetState().IsKeyDown(Keys.Right)) { pos.X += VelocidadeXplayer; posicaoAtual = posicao.direitaandando; }

            else if (Keyboard.GetState().IsKeyDown(Keys.Up)) { pos.Y -= VelocidadeYplayer; posicaoAtual = posicao.cimaandando; } //Esse else eh para a imagem ficar com ele parado para cima

            else if (Keyboard.GetState().IsKeyDown(Keys.Down)) { pos.Y += VelocidadeYplayer; posicaoAtual = posicao.baixoandando; }

            else{

                if (posicaoAtual == posicao.cimaandando)
                    posicaoAtual = posicao.cima;

                if (posicaoAtual == posicao.baixoandando)
                    posicaoAtual = posicao.baixo;

                if (posicaoAtual == posicao.esquerdaandando)
                    posicaoAtual = posicao.esquerda;

                if (posicaoAtual == posicao.direitaandando)
                    posicaoAtual = posicao.direita;

                };

            #endregion

            ColisaoParedes();

            limiteSenario();

            trocarMapa();

            base.Update(gameTime);

        }


        //############################################################


        public void ColisaoParedes()
        {

            #region Vriaveis
            Rectangle[] rectr = new Rectangle[limiteLeitura];
            Rectangle[] rectl = new Rectangle[limiteLeitura];
            Rectangle[] rectu = new Rectangle[limiteLeitura];
            Rectangle[] rectd = new Rectangle[limiteLeitura];
            #endregion

            int coordenadaX = ((int)pos.X / quad.Width), coordenadaY = ((int)pos.Y / quad.Height);

            ret1player = new Rectangle((int)pos.X, (int)pos.Y, quad.Width, quad.Height);

            for (contador = 0, posRelandoX = 0, posRelandoY = 0; contador < limiteLeitura; contador++)
            {

                #region Colisao com os Quadrado
                if (posEstado[contador] == true)
                {
                    #region Retangulos
                    rectl[contador] = new Rectangle(((int)posp[contador].X - 2), ((int)posp[contador].Y), 0, (int)quad.Height);
                    rectr[contador] = new Rectangle(((int)posp[contador].X + quad.Width + 2), ((int)posp[contador].Y), 0, (int)quad.Height);
                    rectd[contador] = new Rectangle(((int)posp[contador].X), ((int)posp[contador].Y + quad.Height + 2), (int)quad.Width, 0);
                    rectu[contador] = new Rectangle(((int)posp[contador].X), ((int)posp[contador].Y - 2), (int)quad.Width, 0);
                    #endregion

                    #region Esquerda dos Retangulos
                    if (ret1player.Intersects(rectl[contador]))
                    {
                        relandoEsquerda = true;
                        podeAndarX = false;
                        posRelandoX = 0;
                        VelocidadeXplayer = 0;
                        if (Keyboard.GetState().IsKeyDown(Keys.Left) && relandoDireita == false) { VelocidadeXplayer = velocidadeInicial; }


                    }
                    #endregion

                    #region Direita dos Retangulos
                    else if (ret1player.Intersects(rectr[contador]))
                    {
                        relandoDireita = true;
                        podeAndarX = false;
                        posRelandoX = 0;
                        VelocidadeXplayer = 0;
                        if (Keyboard.GetState().IsKeyDown(Keys.Right) && relandoEsquerda == false) { VelocidadeXplayer = velocidadeInicial; }
                    }
                    else
                    {
                        relandoEsquerda = false;
                        relandoDireita = false;
                        posRelandoX++;
                    }
                    #endregion

                    #region Cima dos Retangulos
                    if (ret1player.Intersects(rectu[contador]))
                    {
                        relandoCima = true;
                        podeAndarY = false;
                        posRelandoY = 0;
                        VelocidadeYplayer = 0;
                        if (Keyboard.GetState().IsKeyDown(Keys.Up) && relandoBaixo == false) { VelocidadeYplayer = velocidadeInicial; }
                    }
                    #endregion

                    #region Baixo dos Retangulos
                    else if (ret1player.Intersects(rectd[contador]))
                    {
                        relandoBaixo = true;
                        podeAndarY = false;
                        posRelandoY = 0;
                        VelocidadeYplayer = 0;
                        if (Keyboard.GetState().IsKeyDown(Keys.Down) && relandoCima == false) { VelocidadeYplayer = velocidadeInicial; }
                    }
                    else
                    {
                        relandoCima = false;
                        relandoBaixo = false;
                        posRelandoY++;
                    }
                    #endregion

                }

                #region Libera o Player para Andar
                if (posRelandoX >= contador)
                {
                    podeAndarX = true;
                }

                if (posRelandoY >= contador)
                {
                    podeAndarY = true;
                }
            }

            if (podeAndarX == true)
                VelocidadeXplayer = velocidadeInicial;

            if (podeAndarY == true)
                VelocidadeYplayer = velocidadeInicial;

                #endregion
                #endregion


        }


        //######################################################################


        void limiteSenario()
        {
            #region Limite Senario
            #region Limite Direita
            if (pos.X + quad.Width > graphics.PreferredBackBufferWidth)
            {
                pos.X = graphics.PreferredBackBufferWidth - quad.Width;
            }
            #endregion
            #region Limite Esquerda
            if (pos.X < 0) { pos.X = 0; }
            #endregion
            #region Limite Baixo
            if (pos.Y + quad.Height > graphics.PreferredBackBufferHeight)
            {
                pos.Y = graphics.PreferredBackBufferHeight - quad.Height;
            }
            #endregion
            #region Limite Superior
            if (pos.Y < 0)
            {
                pos.Y = 0;
            }
            #endregion
            #endregion
        }


        //######################################################################

        void trocarMapa()
        {

                Rectangle[] teleportEsquerda = new Rectangle[limiteLeitura];
                Rectangle[] teleportDireita = new Rectangle[limiteLeitura];
                Rectangle[] teleportCima = new Rectangle[limiteLeitura];
                Rectangle[] teleportBaixo = new Rectangle[limiteLeitura];

            int coordenadaX = ((int)pos.X / quad.Width), coordenadaY = ((int)pos.Y / quad.Height);

            ret1player = new Rectangle((int)pos.X, (int)pos.Y, quad.Width, quad.Height);

            #region Teleport Esquerda
            for (contador = 0; contador < limiteLeitura; contador++)
            {
                if (TemTeleportEsquerda[contador] == true)
                {
                    teleportEsquerda[contador] = new Rectangle((int)teleportMapaEsquerda[contador].X, (int)teleportMapaEsquerda[contador].Y, quad.Width - 31, quad.Height);

                    if (ret1player.Intersects(teleportEsquerda[contador]))
                    {

                        mapa = new Mapa(@"Mapas\TextFile2.txt");

                        pos.X = tamanhoMapaX - 1;

                        LoadContent();

                        ColisaoParedes();
                    }

                }

                }
                #endregion

            #region Teleport Direita
                for (contador = 0; contador < limiteLeitura; contador++)
                {

                    if (TemTeleportDireita[contador] == true)
                    {
                        teleportDireita[contador] = new Rectangle((int)teleportMapaDireita[contador].X + 31, (int)teleportMapaDireita[contador].Y, quad.Width, quad.Height);

                        if (ret1player.Intersects(teleportDireita[contador]))
                        {

                            mapa = new Mapa(@"Mapas\TextFile2.txt");

                            pos.X = 2;

                            LoadContent();

                            ColisaoParedes();
                        }

                    }
                }
                #endregion

            #region Teleport Cima
                for (contador = 0; contador < limiteLeitura; contador++)
                {
                    if (TemTeleportCima[contador] == true)
                    {
                        teleportCima[contador] = new Rectangle((int)teleportMapaCima[contador].X, (int)teleportMapaCima[contador].Y, quad.Width, quad.Height - 31);

                        if (ret1player.Intersects(teleportCima[contador]))
                        {

                            mapa = new Mapa(@"Mapas\TextFile1.txt");

                            pos.Y = tamanhoMapaY - 1;

                            LoadContent();

                            ColisaoParedes();
                        }

                    }

                }
                #endregion

            #region Teleport Baixo
                for (contador = 0; contador < limiteLeitura; contador++)
                {
                    if (TemTeleportBaixo[contador] == true)
                    {
                        teleportBaixo[contador] = new Rectangle((int)teleportMapaBaixo[contador].X, (int)teleportMapaBaixo[contador].Y + 31, quad.Width, quad.Height);

                        if (ret1player.Intersects(teleportBaixo[contador]))
                        {

                            mapa = new Mapa(@"Mapas\TextFile1.txt");

                            pos.Y = 2;

                            LoadContent();

                            ColisaoParedes();
                        }

                    }

                }
                #endregion

        }

        //######################################################################


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(senario, new Rectangle(0, 0, senario.Width, senario.Height), Color.White);

            #region Testas onde tem Colisao (excluir Depois)
            for (int y = 0; y < mapa.Tamanho_Y(); y++)
            {
                for (int x = 0; x < mapa.Tamanho_X(); x++)
                {

                    if (mapa.array[x, y] == '1') { spriteBatch.Draw(quad, new Vector2(x * quad.Width, y * quad.Height), parede); }
                    if (mapa.array[x, y] == 'h') { spriteBatch.Draw(quad, new Vector2(x * quad.Width, y * quad.Height), chao); }
                    if (mapa.array[x, y] == 'k') { spriteBatch.Draw(quad, new Vector2(x * quad.Width, y * quad.Height), chao); }
                    if (mapa.array[x, y] == 'u') { spriteBatch.Draw(quad, new Vector2(x * quad.Width, y * quad.Height), chao); }
                    if (mapa.array[x, y] == 'j') { spriteBatch.Draw(quad, new Vector2(x * quad.Width, y * quad.Height), chao); }
                    
                }
            }
            #endregion


            #region Imprimir a localização X e Y
            string debug = "[" + ((int)pos.X / quad.Width).ToString() + "," + ((int)pos.Y / quad.Height).ToString() + "] - " + pos.X.ToString() + "/" + pos.Y.ToString();
            Vector2 dpos = font.MeasureString(debug);

            spriteBatch.DrawString(font, debug, new Vector2(20, tamanhoMapaY - dpos.Y - 20), Color.Black);
            #endregion


            #region Animação do Player andando para Cima
            if (posicaoAtual == posicao.cima)
            {
                spriteBatch.Draw(playerCima, pos, Color.White);
                counterTime = 0;
            }

            if (posicaoAtual == posicao.cimaandando)
            {

                if (counterTime >= 0 && counterTime <= 175)
                    spriteBatch.Draw(AndandoCima00, pos, Color.White);

                if (counterTime > 175 && counterTime <= 350)
                    spriteBatch.Draw(playerCima, pos, Color.White);

                if (counterTime >= 350 && counterTime <= 525)
                    spriteBatch.Draw(AndandoCima01, pos, Color.White);

                if (counterTime > 525 && counterTime <= 700)
                    spriteBatch.Draw(playerCima, pos, Color.White);

                if (counterTime > 700)
                {
                    counterTime = 0;
                    spriteBatch.Draw(AndandoCima00, pos, Color.White);
                }
            }
            #endregion

            #region Animação do Player andando para Baixo
            if (posicaoAtual == posicao.baixo)
            {
                spriteBatch.Draw(playerBaixo, pos, Color.White);
                counterTime = 0;
            }

            if (posicaoAtual == posicao.baixoandando)
            {

                if (counterTime >= 0 && counterTime <= 175)
                    spriteBatch.Draw(AndandoBaixo00, pos, Color.White);

                if (counterTime > 175 && counterTime <= 350)
                    spriteBatch.Draw(playerBaixo, pos, Color.White);

                if (counterTime >= 350 && counterTime <= 525)
                    spriteBatch.Draw(AndandoBaixo01, pos, Color.White);

                if (counterTime > 525 && counterTime <= 700)
                    spriteBatch.Draw(playerBaixo, pos, Color.White);

                if (counterTime > 700)
                {
                    counterTime = 0;
                    spriteBatch.Draw(playerBaixo, pos, Color.White);
                }
            }
            #endregion



            else  //Impirir o Qaudrado Branco (Player)
            {
                spriteBatch.Draw(quad, pos, Color.White);
            }

            //spriteBatch.Draw(senarioSuperior, new Rectangle(0, 0, senarioSuperior.Width, senarioSuperior.Height), Color.White);

            spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}