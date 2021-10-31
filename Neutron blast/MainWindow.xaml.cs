using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Neutron_blast
{
    
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        Rect playerHitBox;
        Rect groundHitBox;
        Rect obstacleHitBox;

        bool jumping;
        int force = 20;
        int speed = 5;
        Random rand = new Random();
        bool gameover = false;
        double spriteInt = 0;
        ImageBrush playerSprite = new ImageBrush();
        ImageBrush backgroundSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();
        int[] obstaclePosition = { 320, 310, 300, 305, 315 };
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            myCanvas.Focus();
            gameTimer.Tick += gameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.gif.png"));
            background.Fill = backgroundSprite;
            background2.Fill = backgroundSprite;
            StartGame();

        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && gameover)
            {
               
                StartGame();
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && !jumping && Canvas.GetTop(player) > 260)
            {
               
                jumping = true;
                
                force = 15;
                
                speed = -12;
                
                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif.png"));
            }
        }

        private void StartGame()
        {
            

            Canvas.SetLeft(background, 0); 
            Canvas.SetLeft(background2, 1262); 

            
            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 140);

            
            Canvas.SetLeft(obstacle, 950);
            Canvas.SetTop(obstacle, 310);
            
            runSprite(1);

            
            obstacleSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/obstacle.png.png"));


            obstacle.Fill = obstacleSprite; 

            
            jumping = false;
            
            gameover = false;
            
            score = 0;
            
            scoreText.Content = "Score: " + score;

            
            gameTimer.Start();
        }

        private void runSprite(double i)
        {
             switch (i)
            {

                case 1:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_01.gif.png"));
                    break;
                case 2:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif.png"));
                    break;





            }
            
            player.Fill = playerSprite;
        }

        private void gameEngine(object sender, EventArgs e)
        {
            Canvas.SetTop(player, Canvas.GetTop(player) + speed);


           


            Canvas.SetLeft(background, Canvas.GetLeft(background) - 3);
            Canvas.SetLeft(background2, Canvas.GetLeft(background2) - 3);
            
            Canvas.SetLeft(obstacle, Canvas.GetLeft(obstacle) - 12);
            
            scoreText.Content = "Score: " + score;

            
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            groundHitBox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width, ground.Height);
            obstacleHitBox = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);

            
            if (playerHitBox.IntersectsWith(groundHitBox))
            {
                speed = 0;
                
                Canvas.SetTop(player, Canvas.GetTop(ground) - player.Height);
                
                jumping = false;
                
                spriteInt += .5;
                
                if (spriteInt > 8)
                {
                    
                    spriteInt = 1;
                }
                
                runSprite(spriteInt);
            }

           
            if (playerHitBox.IntersectsWith(obstacleHitBox))
            {
                
                gameover = true;
                
                gameTimer.Stop();

            }

            
            if (jumping)
            {
                
                speed = -9;
                
                force--;
            }
            else
            {
                
                speed = 12;
            }

            
            if (force < 0)
            {
                jumping = false;
            }

            
            if (Canvas.GetLeft(background) < -1262)
            {
                
                Canvas.SetLeft(background, Canvas.GetLeft(background2) + background2.Width);
            }
            
            if (Canvas.GetLeft(background2) < -1262)
            {
                Canvas.SetLeft(background2, Canvas.GetLeft(background) + background.Width);
            }

            
            if (Canvas.GetLeft(obstacle) < -50)
            {
                
                Canvas.SetLeft(obstacle, 950);
                Canvas.SetTop(obstacle, obstaclePosition[rand.Next(0, obstaclePosition.Length)]);
                
                score += 1;
            }

            
            if (gameover)
            {
                
                obstacle.Stroke = Brushes.Black;
                obstacle.StrokeThickness = 1;

                
                player.Stroke = Brushes.Red;
                player.StrokeThickness = 1;
                
                scoreText.Content += "  Enter to try again ";
            }
            else
            {
                
                player.StrokeThickness = 0;
                obstacle.StrokeThickness = 0;
            }
        }

    }
}
