using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MineSweeper
{
    class MineSweeperC
    {
        private int rows;                    //no need to explain
        private int columns;                 //no nead to explain this either
        private int bombs;                   //number of bombs (should be less than the total ammount of grid by 9)
        private bool isFirst;                //true until the player picks a grid for the 1st time
        private int[,] gameBoard;            //contain the number of neighboring grids that contains a mine
        private Boolean[,] containsMine;     //true if the grid contains a mine
        private Boolean[,] revealed;         //true if the grid was already clicked by the player
        private Boolean[,] isFlagged;        //true if the grid was flagged
        public int remainingMines;          //numbers of mines - number of flags
        private Boolean playerLost;          //true if the player lost


        public MineSweeperC(int rows, int columns, int bombs)
        {
            isFirst = true;
            this.rows = rows;
            this.columns = columns;
            this.bombs = bombs;
            playerLost = false;
            remainingMines = bombs;

            gameBoard = new int[rows, columns];
            revealed = new Boolean[rows, columns];
            isFlagged = new Boolean[rows, columns];
            containsMine = new Boolean[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    containsMine[i, j] = false;
                    revealed[i, j] = false;
                    isFlagged[i, j] = false;
                }
            }
        }


        /*
        player actions
        */
        //return true If the player clicked on a mine, false otherwise.
        public bool playerAtion_chooseGrid(int x, int y, bool isPlayer)
        {
            if (x == -1 || x == rows || y == -1 || y == columns)
                return false;
            if (isFlagged[x, y] && isPlayer)
            {
                return false;
            }
            if (isFirst)
            {
                isFirst = false;
                setValueOfNeighborTo(x, y, -1);
                placeMines();
                setValueOfNeighborTo(x, y, 0);
                setNumbers();
                playerAtion_chooseGrid(x, y, false);
                return false;
            }
            if (revealed[x, y])
            {
                return false;
            }
            if (containsMine[x, y])
            {
                playerLost = true;
                return true;
            }
            else
            {
                revealed[x, y] = true;
                if (gameBoard[x, y] == 0)
                {
                    playerAtion_chooseGrid(x-1, y-1, false);
                    playerAtion_chooseGrid(x-1, y+1, false);
                    playerAtion_chooseGrid(x-1, y, false);
                    playerAtion_chooseGrid(x, y-1, false);
                    playerAtion_chooseGrid(x, y+1, false);
                    playerAtion_chooseGrid(x+1, y-1, false);
                    playerAtion_chooseGrid(x+1, y+1, false);
                    playerAtion_chooseGrid(x+1, y, false);
                }
                return false;
            }
            
           

           
        }

        //returns 0 if: flag wasn't placed. 1 if: the flag was placed. 2 if: the flag was removed
        public int playerAtion_addOrRemoveFlag(int x, int y)
        {
            if (revealed[x, y])
                return 0;
            if (isFlagged[x, y])
            {
                isFlagged[x, y] = false;
                remainingMines++;
                return 2;
            }
            isFlagged[x, y] = true;
            remainingMines--;
            return 1;
        }




        /*
        Setters
        */
        public void setNumbers()
        {
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    if (containsMine[i, j])
                    {
                        addValueToNeighbor(i, j, 1);
                    }
                }
            }
        }

        public void placeMines()
        {
            for (int i = 0; i < bombs; i++)
            {
                Random rand = new Random();
                while (true)
                {
                    int x = rand.Next(0, rows);
                    int y = rand.Next(0, columns);
                    if (!containsMine[x,y] && gameBoard[x,y]!=-1)
                    {
                        containsMine[x,y] = true;
                        break;
                    }
                }
            }
        }



        /*
        getters 
        */
        public Boolean Lost()
        {
            return playerLost;
        }
        public int[,] getNumbers()
        {
            return gameBoard;
        }
        public Boolean[,] getRevealed()
        {
            return revealed;
        }
        public Boolean[,] getMineLocation()
        {
            return containsMine;
        }
        public Boolean[,] getIsFlagged()
        {
            return isFlagged;
        }

        public Boolean Won()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (!containsMine[i, j] && !revealed[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        /*
        Utility functions
        */
        public void setGridValueTo(int x, int y, int value)
        {
            try
            {
                gameBoard[x, y] = value;
            } catch (IndexOutOfRangeException)
            {
                return;
            }
            
        }
        public void addValueAtGrid(int x, int y, int value)
        {
            try
            {
                gameBoard[x, y] += value;
            } catch(IndexOutOfRangeException)
            {
                return;
            }
        }

        public void setValueOfNeighborTo(int x, int y, int value)
        {
            setGridValueTo(x, y, value);
            setGridValueTo(x, y + 1, value);
            setGridValueTo(x, y - 1, value);

            setGridValueTo(x - 1, y, value);
            setGridValueTo(x - 1, y + 1, value);
            setGridValueTo(x - 1, y - 1, value);

            setGridValueTo(x + 1, y, value);
            setGridValueTo(x + 1, y - 1, value);
            setGridValueTo(x + 1, y + 1, value);
        }
        public void addValueToNeighbor(int x, int y, int value)
        {
            addValueAtGrid(x, y, value);
            addValueAtGrid(x, y + 1, value);
            addValueAtGrid(x, y - 1, value);

            addValueAtGrid(x - 1, y, value);
            addValueAtGrid(x - 1, y + 1, value);
            addValueAtGrid(x - 1, y - 1, value);

            addValueAtGrid(x + 1, y, value);
            addValueAtGrid(x + 1, y - 1, value);
            addValueAtGrid(x + 1, y + 1, value);
        }


    }
}
