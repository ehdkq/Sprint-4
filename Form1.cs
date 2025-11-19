using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sprint_3
{
    public partial class Form1 : Form
    {
        private BaseGame game;
        private Button[,] gridButtons;

        private IPlayer _bluePlayer;
        private IPlayer _redPlayer;
        private Timer _computerTimer;
        public Form1()
        {
            InitializeComponent();

            pnlBoard.Enabled = false;

            _computerTimer = new Timer();
            _computerTimer.Interval = 1000;
            _computerTimer.Tick += ComputerTimer_Tick;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnNewGame_Click(object sender, EventArgs e) //what happens when you click the new game button
        {
            _computerTimer.Stop();

            int boardSize = (int)numBoardSize.Value; //declares the boardSize the user inputs from the field

            var mode = simpleGameButton.Checked ? GameMode.Simple : GameMode.General; //checks which game mode is selected

            if (mode == GameMode.Simple)
            {
                game = new SimpleGame(boardSize);
            }
            else
            {
                game = new GeneralGame(boardSize);
            }

            _bluePlayer = radioButton1.Checked ? (IPlayer)new ComputerPlayer() : new HumanPlayer();
            _redPlayer = radioButton2.Checked ? (IPlayer)new ComputerPlayer() : new HumanPlayer();
            
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            
            pnlBoard.Enabled = true;
            CreateBoardGrid(boardSize); //creates the grid based on the boardSize

            UpdateTurnLabel(); //updates the turn (red or blue)
            UpdateScores();

            CheckComputerTurn();
        }

        private void CheckComputerTurn()
        {
            if (game.State != GameState.InProgress)
            {
                return;
            }

            IPlayer currentPlayer = (game.CurrentTurn == Player.Blue) ? _bluePlayer : _redPlayer;

            if (currentPlayer is ComputerPlayer)
            {
                pnlBoard.Enabled = false;
                _computerTimer.Start();
            }
            else
            {
                pnlBoard.Enabled = true;
            }
        }

        private void ComputerTimer_Tick(object sender, EventArgs e)
        {
            _computerTimer.Stop();

            if (game.State != GameState.InProgress)
            {
                return;
            }

            IPlayer currentPlayer = (game.CurrentTurn == Player.Blue) ? _bluePlayer : _redPlayer;

            currentPlayer.MakeMove(game);

            UpdateBoardFromGame();
            UpdateScores();
            UpdateTurnLabel();
            CheckForGameOver();

            CheckComputerTurn();
        }

        private void CreateBoardGrid(int size)
        {
            pnlBoard.Controls.Clear(); //clears board
            gridButtons = new Button[size, size]; //sets the buttons on the grid
            int buttonSize = pnlBoard.Width / size; //sets the size for the buttons

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    var button = new Button
                    {
                        //sets all the button parameters
                        Width = buttonSize,
                        Height = buttonSize,
                        Left = col * buttonSize,
                        Top = row * buttonSize,
                        Tag = new Point(row, col)
                    };

                    button.Click += GridButton_Click;
                    pnlBoard.Controls.Add(button);
                    gridButtons[row, col] = button;
                }
            }
        }

        private void GridButton_Click(object sender, EventArgs e)
        {
            IPlayer currentPlayer = (game.CurrentTurn == Player.Blue) ? _bluePlayer : _redPlayer;
            if (currentPlayer is ComputerPlayer)
            {
                return;
            }

            Button clickedButton = sender as Button;

            if (clickedButton == null || game == null || game.State != GameState.InProgress)
            {
                return;
            }

            Point position = (Point)clickedButton.Tag;

            Cell move; 

            if (game.CurrentTurn == Player.Blue) //if the turn is the blue player, put the s or o down
            {
                move = blueSButton.Checked ? Cell.S : Cell.O;
            }
            else
            {
                move = redSButton.Checked ? Cell.S : Cell.O; //if not, put it for the red player
            }
            game.MakeMove(position.X, position.Y, move);
            UpdateBoardFromGame();
            UpdateScores();
            CheckForGameOver();
            UpdateTurnLabel();

            CheckComputerTurn();
        }

        private void UpdateTurnLabel() //updates turn label from red to blue and vice versa
        {
            if (game == null)
            {
                lblTurn.Text = "Current Turn: ";
                return;
            }

            lblTurn.Text = $"Current Turn: {game.CurrentTurn}";

            if (game.State != GameState.InProgress) 
            {
                bluePlayer.Enabled = true;
                redPlayer.Enabled = true;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                return;
            }

            bool isBlueHuman = (_bluePlayer is HumanPlayer);
            bool isRedHuman = (_redPlayer is HumanPlayer);

            if (game.CurrentTurn == Player.Blue && isBlueHuman) 
            {
                bluePlayer.Enabled = true;
                redPlayer.Enabled = false;
            }
            else if (game.CurrentTurn == Player.Red && isRedHuman)
            {
                bluePlayer.Enabled = false;
                redPlayer.Enabled = true;
            }
            else
            {
                bluePlayer.Enabled = false;
                redPlayer.Enabled = false;
            }
            
        }
        private void numBoardSize_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblTurn_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void redOButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void UpdateBoardFromGame()
        {
            if (game == null)
            {
                return;
            }
            for (int r = 0; r < game.BoardSize; r++)
            {
                for (int c = 0; c < game.BoardSize; c++)
                {
                    Cell cellState = game.GameBoard[r, c];
                    if (cellState != Cell.Empty)
                    {
                        gridButtons[r, c].Text = cellState.ToString();
                        gridButtons[r, c].Enabled = false;
                    }
                }
            }
        }

        private void UpdateScores()
        {
            if (game == null)
            {
                lblBlueScore.Text = "Blue: 0";
                lblRedScore.Text = "Red: 0";
            }
            else
            {
                lblBlueScore.Text = $"Blue: {game.BlueScore}";
                lblRedScore.Text = $"Red: {game.RedScore}";
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void CheckForGameOver()
        {
            if (game.State != GameState.InProgress)
            {
                _computerTimer.Stop();
                pnlBoard.Enabled = false;
                string message = "";
                if (game.State == GameState.Draw)
                {
                    message = "The game is a draw!";
                }
                else if (game.State == GameState.BlueWin)
                {
                    message = "Blue player wins!";
                }
                else
                {
                    message = "Red player wins!";
                }

                MessageBox.Show(message, "Game Over!");
            }
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }
}
