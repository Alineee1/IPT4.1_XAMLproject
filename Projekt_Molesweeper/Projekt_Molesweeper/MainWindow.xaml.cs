using System;
using System.Collections.Generic;
using System.Data;
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

namespace Projekt_Molesweeper {
    /// <summary>
    /// 2D Array to create fields
    /// </summary>
    public partial class MainWindow : Window {
        bool mole = false;
        public MainWindow() {
            InitializeComponent();
            List<List<Field>> fields = new();

            //Defines the size of field
            int sizeX = 20;
            int sizeY = 20;

            Tiles Matrix;
            TimeSpan time;
            DispatcherTimer timer;

            int height;
            int width;
            int mines;

            bool status1;
            bool status2;

            for (int i = 0; i < sizeY; i++) {
                fields.Add(new List<Field>());

                for (int j = 0; j < sizeX; j++) {
                    fields[i].Add(new Field());
                }
            }

            DataContext = fields;
        }
        /// <summary>
        /// Defines whether cell is free or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            Field field = button.DataContext as Field;

            if (this.mole) {
                field.Click("Game over");
            }
            else {
                field.Click("");
            }

            this.mole = !this.mole;
            button.Content = field.Sign;
        }

    }

    public class Field {
        public string Sign { get; set; }


        public void Click(string sign) {
            this.Sign = sign;
        }
    }


    //New code from Mathu for fields?
    struct Tile {
        public int X;
        public int Y;
        public int label;

        public string Tile_neighbor;

        public bool mine;

        public Tile(int X, int Y, bool status, int selection, string neighbours) {
            this.X = X;
            this.Y = Y;
            mine = status;
            label = selection;
            Tile_neighbor = neighbours;
        }
    }
    class Tiles {
        public Tiles[,] tiles;

        public Tiles(int Width, int Height) {
            tiles = new Tiles[Width, Height];
        }
        public void Add_point(Tile point) {
            tiles[point.X, point.Y] = point;
        }





        //Check if a mine is there & retrun number
        private int Check_mine(int x, int y) {
            int count = 0;

            for (int x_ = -1; x_ <= 1; x_++) {
                for (int y_ = -1, y_ <= 1; y_++) {
                    if (((x + x_ >=)))
            }
            }
        }
    }



}


