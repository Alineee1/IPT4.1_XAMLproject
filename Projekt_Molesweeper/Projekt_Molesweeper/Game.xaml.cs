using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekt_Molesweeper {

    public partial class StartWindow : Window {

        private int sizeY;
        private int sizeX;
        private List<List<Field>> fields;
        public StartWindow(Difficulty difficulty) {
            InitializeComponent();
            ///Sets Window to Startwindow

            ///defines number of field
            switch (difficulty) {
                case Difficulty.HARD:
                    sizeY = 20;
                    sizeX = 20;

                    break;
                case Difficulty.INTERMEDIATE:
                    sizeY = 15;
                    sizeX = 15;

                    break;
                case Difficulty.EASY:
                    sizeY = 10;
                    sizeX = 10;

                    break;
                default:
                    throw new Exception("Wrong value for difficulty");
            }


            fields = new();


            for (int i = 0; i < sizeY; i++) {
                fields.Add(new List<Field>());
                this.TheGrid.RowDefinitions.Add(new RowDefinition());
                this.TheGrid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int j = 0; j < sizeX; j++) {
                    fields[i].Add(new Field(false));
                    Button button = new Button();
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    button.Click += new RoutedEventHandler(this.FieldClick);

                    TheGrid.Children.Add(button);
                }
            }

            //Adjusts the amount of mines
            switch (difficulty) {
                case Difficulty.HARD:
                    Set_Mines(50);
                    break;
                case Difficulty.INTERMEDIATE:
                    Set_Mines(30);
                    break;
                case Difficulty.EASY:
                    Set_Mines(10);
                    break;
            }

            DataContext = fields;
        }

        public void FieldClick(object sender, RoutedEventArgs e) {
            // Stichwort Type-casting
            Button btn = (Button)sender;
            int x = Grid.GetColumn(btn);
            int y = Grid.GetRow(btn);

            Field field = fields[x][y];
            field.OnClick();

        //    private void btn_MouseUpClick(object sender, MouseButtonEventArgs e);
        //    if (e.ChangedButton == MouseButton.Right) {
        //    }
        //    e.Handled = true;

        //}



        btn.Content = field.Sign;
        }



        

    private void On_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", "http://127.0.0.1:5500/CODE/main.html");
        }

        ///Mark mines
        private void Set_Mines(int count) {
            Random mole = new Random();


            for (int i = 0; i < count; i++) {
                int coord_x = mole.Next(sizeX);
                int coord_y = mole.Next(sizeY);

                // 
                //Field f = new Field("x", false);
                //f.Mine = true;

                fields[coord_x][coord_y].Mine = true;
            }
        }
    }

    public class Field {
        public string Sign {
            get {
                if (!Clicked){
                    return "";
                }

                if (Clicked && Mine) {
                    return "x";
                }

                if (Clicked && !Mine) {
                    return CountMines().ToString();
                }

                return "";
            }
        }
        public bool Mine { get; set; }
        public bool Clicked { get; set; } = false;

        public Field(bool mine) {
            this.Mine = mine;

        }

        public int CountMines(){
            return 1;
        }

        public void OnClick() {
            Clicked = true;
        }
    }




}
