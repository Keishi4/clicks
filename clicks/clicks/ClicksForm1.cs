﻿/*  Interface changes
 *  03.08.2016: Sizing grip is disabled on main window (fixed bug #(?) )
 *
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using libdraw;

namespace ClicksGame
{
    public partial class ClicksForm1 : Form
    {
        public ClicksForm1()
        {
            InitializeComponent();
            
            // ClicksShape = DrawRShape;
            this.Text = "Clicks " + ver.Major + "." + ver.Minor;
            try
            {
                boardBox.BackgroundImage = Image.FromFile(@"back.jpg");
            }
            catch(Exception)
            {
            //   MessageBox.Show("Background image not found", "File not found", MessageBoxButtons.OK,
            //                    MessageBoxIcon.Warning);
            }
        }
        private void TurnClick(int xPointClicked, int yPointClicked)
        {
            infoPanel.Text = "";
            if (game.ComputeTurn(xPointClicked, yPointClicked))
            {
                undoItem.Enabled = true;
                btnUndoTurn.Enabled = true;
                btnUndoTurn.Visible = true;
            }
            // boardBox_MouseDown(this, new MouseEventArgs(MouseButtons.Right, 1, 300, 300, 0));
            boardBox.Invalidate();
            if(game.NoTurnCheck())
            {
                infoPanel.Text = "Ходов больше нет";
                var res = MessageBox.Show("End game?", "Game Over", MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);
                if(res == DialogResult.No)
                    undoItem_Click(undoItem, new EventArgs());
                else
                    exitItem_Click(exitItem, new EventArgs());
            }
            boardBox.Focus();
        }
        private void boardBox_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                if((e.X < boardBox.Width) & (e.Y < boardBox.Height))
                    TurnClick(e.X, e.Y);
        }
        private void boardBox_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
                boardBox.Invalidate();
        }
        private void boardBox_Paint(object sender, PaintEventArgs e)
        {
            drw.GameBrd = game.Bd;
            drw.DisplayBoard(e);
        }
        private void newGameItem_Click(object sender, EventArgs e)
        {
            if (easySkill.Checked)
            {
                skill = 4;
                skillPanel.Text = "Easy";
            }
            if (normalSkill.Checked)
            {
                skill = 5;
                skillPanel.Text = "Normal";
            }
            if (hardSkill.Checked)
            {
                skill = 6;
                skillPanel.Text = "Hard";
            }
            try
            {
                InitGame();
            }
            catch (System.IO.FileNotFoundException)
            {
                gameStarted = false;
                MessageBox.Show("Game drawing library libdrw.dll is not found", "Game loading error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            if(gameStarted)
            {
                grpStart.Visible = false;
                grpStart.Enabled = false;
                grpGame.Visible = true;
                grpGame.Enabled = true;
                game.Infinite = checkInfinite.Checked;
                // kPanel.Text = InfoKilled(board);
            }
            // Next function is used for update board after starting new game from context menu
            // resolves bug #(?) **** 03.08.2016
            // boardBox_MouseDown(this, new MouseEventArgs(MouseButtons.Right, 1, 300, 300, 0));
            // replaceed with Invalidate() 16.10.2017
            boardBox.Invalidate();
            boardBox.Focus();
        }
        private void quitGame_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void undoItem_Click(object sender, EventArgs e)
        {
            boardBox.Invalidate();
            game.UndoTurn();
            if ((bool)game.NoUndo)
            {
                undoItem.Enabled = false;
                btnUndoTurn.Visible = false;
            }
            infoPanel.Text = "Ход отменен";
            // ?
            // boardBox_MouseDown(this, new MouseEventArgs(MouseButtons.Right, 1, 300, 300, 0));
        }
        private void helpItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(helpMsg);
        }
        private void btnUndoTurn_Click(object sender, EventArgs e)
        {
            undoItem_Click(undoItem, new EventArgs());
            boardBox.Focus();
        }
        private void exitItem_Click(object sender, EventArgs e)
        {
            gameStarted = false;
            grpStart.Visible = true;
            grpStart.Enabled = true;
            grpGame.Visible = false;
            grpGame.Enabled = false;
            btnUndoTurn.Visible = false;
            skillPanel.Text = "";
            infoPanel.Text = "";
            undoItem.Enabled = false;
            exitItem.Enabled = false;
            btnUndoTurn.Enabled = false;
        }
        private void exitProgramItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void InitGame()
        {
            game = new ClicksComp(skill, boardRows, boardCols);
            drw = new ClicksDraw(boardRows, boardCols);
            infoPanel.Text = "";
            undoItem.Enabled = false;
            btnUndoTurn.Enabled = false;
            btnUndoTurn.Visible = false;
            // boardBox.Visible = true;
            // boardBox.Enabled = true;
            exitItem.Enabled = true;
            gameStarted = true;

        }
        private void startGame_Click(object sender, EventArgs e)
        {
            newGameItem_Click(newGameItem, new EventArgs());
        }
    }
}