using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong3U
{
    public partial class Form1 : Form
    {
        int paddle1X = 10;
        int paddle1Y = 170;
        int player1Score = 0;

        int paddle2X = 550;
        int paddle2Y = 170;
        int player2Score = 0;

        int paddleWidth = 40;
        int paddleHeight = 40;
        int paddleSpeed = 5;

        int ballX = 295;
        int ballY = 195;
        int ballXSpeed = 6;
        int ballYSpeed = 6;
        int ballWidth = 10;
        int ballHeight = 10;

        bool wDown = false;
        bool sDown = false;
        bool dDown = false;
        bool aDown = false;
        bool lArrowDown = false;
        bool rArrowDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        Random randGen = new Random();

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        Pen bluePen = new Pen(Color.DodgerBlue, 10);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font screenFont = new Font("Consolas", 12);
        SoundPlayer hit = new SoundPlayer(Properties.Resources.hitSound);
        SoundPlayer wallHit = new SoundPlayer(Properties.Resources.wallHit);
        SoundPlayer goal = new SoundPlayer(Properties.Resources.cheer);

        public Form1()
        {
            InitializeComponent();
        }

        public void InitializeValues()
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // on key down set the bool associated with the pressed key to true
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;

                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    lArrowDown = true;
                    break;
                case Keys.Right:
                    rArrowDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //on key up set the bool associated with the released key to false
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;

                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    lArrowDown = false;
                    break;
                case Keys.Right:
                    rArrowDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move ball
            ballX += ballXSpeed;
            ballY += ballYSpeed;

            //move player 1
            if (wDown == true && paddle1Y > 0)
            {
                paddle1Y -= paddleSpeed;
            }

            if (sDown == true && paddle1Y < this.Height - paddleHeight)
            {
                paddle1Y += paddleSpeed;
            }

            if (dDown == true && paddle1X < this.Width/2 - 20)
            {
                paddle1X += paddleSpeed;
            }

            if (aDown == true && paddle1X > 10)
            {
                paddle1X -= paddleSpeed;
            }

            //move player 2
            if (upArrowDown == true && paddle2Y > 0)
            {
                paddle2Y -= paddleSpeed;
            }

            if (downArrowDown == true && paddle2Y < this.Height - paddleHeight)
            {
                paddle2Y += paddleSpeed;
            }

            if (lArrowDown == true && paddle2X > this.Width / 2 - 20)
            {
                paddle2X -= paddleSpeed;
            }

            if (rArrowDown == true && paddle2X < this.Width - 50)
            {
                paddle2X += paddleSpeed;
            }

            //check if ball hit top or bottom wall and change direction if it does
            if (ballY < 0 || ballY > this.Height - ballHeight)
            {
                ballYSpeed *= -1;  // or: ballYSpeed = -ballYSpeed;
                wallHit.Play();
            }

            //create Rectangles of objects on screen to be used for collision detection
            Rectangle player1Rec = new Rectangle(paddle1X + paddleWidth/2, paddle1Y, paddleWidth/2, paddleHeight);
            Rectangle player2Rec = new Rectangle(paddle2X + paddleWidth/2, paddle2Y, paddleWidth/2, paddleHeight);
            Rectangle player2RecBack = new Rectangle(paddle2X, paddle2Y, paddleWidth / 2, paddleHeight);
            Rectangle player1RecBack = new Rectangle(paddle1X, paddle1Y, paddleWidth / 2, paddleHeight);
            Rectangle ballRec = new Rectangle(ballX, ballY, ballWidth, ballHeight);


           Rectangle LTopWall = new Rectangle( 0, 0, 10, 100);
           Rectangle LBottomWall = new Rectangle( 0, 300, 10, 100);

           Rectangle RTopWall = new Rectangle( 600, 0, 10, 100);
           Rectangle RBottomWall = new Rectangle( 600, 300, 10, 100);

            //check if ball hits either paddle. If it does change the direction
            //and place the ball in front of the paddle hit
            if (player1Rec.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                ballX = paddle1X + paddleWidth + 1;
                hit.Play();
            }
            else if (player1RecBack.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                ballX = paddle1X - ballWidth - 1;
                hit.Play();
            }
           
            else if (player2Rec.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                ballX = paddle2X - ballWidth - 1;
                hit.Play();
            }
            else if (player2RecBack.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                ballX = paddle2X - ballWidth + 1;
                hit.Play();
            }
            
            else if (ballRec.IntersectsWith(LBottomWall))
            { 
                ballXSpeed *= -1;
                ballX = ballX + 5;
                wallHit.Play();
            }
            else if (ballRec.IntersectsWith(LTopWall))
            {
                ballXSpeed *= -1;
                ballX = ballX + 5;
                wallHit.Play();
            }
            else if (ballRec.IntersectsWith(RBottomWall))
            {
                ballXSpeed *= -1;
                ballX = ballX - 5;
                wallHit.Play();
            }
            else if (ballRec.IntersectsWith(RTopWall))
            {
                ballXSpeed *= -1;
                ballX = ballX - 5;
                wallHit.Play();
            }
            //check if a player missed the ball and if true add 1 to score of other player 
            if (ballX < 0)
            {
                goal.Play();
                player2Score++;
                ballX = 295;
                ballY = 195;

                paddle1Y = 170;
                paddle1X = 10;
                paddle2Y = 170;
                paddle2X = 540;
            }
            else if (ballX > 600 )
            {
                goal.Play();
                player1Score++;

                ballX = 295;
                ballY = 195;

                paddle1Y = 170;
                paddle2Y = 170;
            }

            // check score and stop game if either player is at 3
            if (player1Score == 3 || player2Score == 3)
            {
                gameTimer.Enabled = false;
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen linePen = new Pen(Color.Gray, 3);
            
            e.Graphics.DrawEllipse(linePen, -100, 100, 200, 200);
            e.Graphics.DrawEllipse(linePen, this.Width -100, 100, 200, 200);

            e.Graphics.DrawLine(linePen, this.Width / 2, 0, this.Width / 2, this.Height);
            e.Graphics.DrawEllipse(linePen, this.Width/2 - 100, 100, 200, 200);

            e.Graphics.FillEllipse(blueBrush, paddle1X, paddle1Y, paddleWidth, paddleHeight);
            e.Graphics.FillEllipse(blueBrush, paddle2X, paddle2Y, paddleWidth, paddleHeight);

            e.Graphics.FillRectangle(whiteBrush, ballX, ballY, ballWidth, ballHeight);

            e.Graphics.DrawString($"{player1Score}", screenFont, whiteBrush, 275, 10);
            e.Graphics.DrawString($"{player2Score}", screenFont, whiteBrush, 310, 10);

            e.Graphics.DrawLine(bluePen, 0, 0, 0, 100);
            e.Graphics.DrawLine(bluePen, 0, 400, 0, 300);
            
            e.Graphics.DrawLine(bluePen, 600, 0, 600, 100);
            e.Graphics.DrawLine(bluePen, 600, 400, 600, 300);
        }
    }
}
