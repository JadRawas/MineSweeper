using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int row = 10;
        int column = 10;
        int mine = 25;
        Button[,] buttonlist;
        MineSweeperC Game;
        public MainWindow()
        {
            InitializeComponent();
            newGame();
           
        }

        private void right_click(object sender, MouseButtonEventArgs e)
        {
            var source = e.Source as FrameworkElement;
            if (source != null)
            {
                string elementName = source.Name;
                int[] xy = getCoordinates(elementName);
                flag(xy[0], xy[1]);
            }
        }


        //Click Listeners
        private void Field_Click(object sender, RoutedEventArgs e)
        {
            
            var source = e.OriginalSource as FrameworkElement;
            if (source == null)
                return;
            int[] xy = getCoordinates(source.Name);

            if (Game.playerAtion_chooseGrid(xy[0], xy[1], true))
            {
                player_lost();
                MessageBox.Show("player lost");
                newGame();
                return;
            }
            Update_Field();
            if (Game.Won())
            {
                MessageBox.Show("Player Won!!");
                newGame();
            }
            
        }

        //Place or remove flag
        public void flag(int x, int y)
        {
            bool[,] isFlagged = Game.getIsFlagged();
            if (isFlagged[x, y])
            {
                isFlagged[x, y] = false;
                buttonlist[x, y].Content = "";
                buttonlist[x, y].Foreground = Brushes.Black;
                Game.remainingMines++;
            }
            else
            {
                isFlagged[x, y] = true;
                buttonlist[x, y].Content = "🚩";
                buttonlist[x, y].Foreground = Brushes.Red;
                Game.remainingMines--;
            }
            this.Title = "Mines left: " +Game.remainingMines;
        }


        //Update Interface
        private void Update_Field()
        {
            bool[,] revealed = Game.getRevealed();
            int[,] numbers = Game.getNumbers();
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    if (revealed[i, j])
                    {
                       buttonlist[i, j].IsEnabled = false;
                       buttonlist[i, j].Content = numbers[i, j];
                       buttonlist[i, j].Foreground = Brushes.Black;
                        if (numbers[i, j] == 0)
                            buttonlist[i, j].Content = "";
                    }
                }
            }
        }
        //reveal All
        public void player_lost()
        {
            bool[,] revealed = Game.getRevealed();
            int[,] numbers = Game.getNumbers();
            bool[,] mineLoc = Game.getMineLocation();
            bool[,] isFlagged = Game.getIsFlagged();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (mineLoc[i, j])
                    {
                        buttonlist[i, j].IsEnabled = false;
                        buttonlist[i, j].Content = "💣";
                        buttonlist[i,j].Foreground = Brushes.Black;
                        if (isFlagged[i, j])
                        {
                            buttonlist[i,j].Foreground = Brushes.Green;
                        }

                    }
                    else
                    {
                        buttonlist[i, j].IsEnabled = false;
                        buttonlist[i, j].Content = numbers[i, j];
                        buttonlist[i, j].Foreground = Brushes.Black;
                        if(numbers[i, j]==0)
                             buttonlist[i, j].Content = "";
                        if (isFlagged[i, j])
                        {
                            buttonlist[i, j].Foreground = Brushes.Red;
                            buttonlist[i, j].Content = numbers[i, j]+"🚩";
                        }
                    }
                }
            }
        }
        /*
        New Game
        */ 
        public void newGame()
        {
            Field.Children.Clear();
            Field.RowDefinitions.Clear();
            Field.ColumnDefinitions.Clear();
            buttonlist = new Button[row, column];

            //Setting Interface
            for (int i = 0; i < column; i++)
            {
                ColumnDefinition C = new ColumnDefinition();
                Field.ColumnDefinitions.Add(C);
            }
            for (int i = 0; i < row; i++)
            {
                RowDefinition R = new RowDefinition();
                Field.RowDefinitions.Add(R);
            }
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    Button B = new Button();
                    B.Name = "r" + i + "c" + j;
                    B.MouseDown += new MouseButtonEventHandler(right_click);
                    //B.Content = i + "," + j;
                    buttonlist[i, j] = B;
                    Grid.SetColumn(B, j);
                    Grid.SetRow(B, i);
                    Field.Children.Add(B);
                }
            }
            //Initializing game
            Game = new MineSweeperC(row, column, mine);
            this.Title = "Mines left: " + Game.remainingMines;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            row = 5;
            column = 5;
            mine = 6;
            newGame();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            row = 10;
            column = 10;
            mine = 25;
            newGame();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            row = 15;
            column = 15;
            mine = 60;
            newGame();
        }

        /*
        utility
        */

        //get's the name of the button and return it's coordinates
        private int[] getCoordinates(String s)
        {
            int[] xy = new int[2];
            String x = "";
            String y = "";

            int i;
            for(i = 1; s[i] != 'c'; i++)
            {
                x = x + s[i];
            }
            for(i = i+1; i < s.Length; i++)
            {
                y = y + s[i];
            }
            xy[0] = Int32.Parse(x);
            xy[1] = Int32.Parse(y);
            return xy;
        }
    }
}
