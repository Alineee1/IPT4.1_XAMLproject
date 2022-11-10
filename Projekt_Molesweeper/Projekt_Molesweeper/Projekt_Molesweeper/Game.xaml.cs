using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Projekt_Molesweeper {
    ///Change start window to mainwindow 
    public partial class Start : Window {
        public Start(Difficulty difficulty) {

            InitializeComponent();
            ///Sets Window to Startwindow

            int sizeY;
            int sizeX;

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
            }


            List<List<Field>> fields = new();



            for (int i = 0; i < sizeY; i++) {
                fields.Add(new List<Field>());

                for (int j = 0; j < sizeX; j++) {
                    fields[i].Add(new Field());
                }
            }

            DataContext = fields;
        }
    }

        public class Field {
            public string Sign { get; set; }


            public void Click(string sign) {
                this.Sign = sign;
            }
        }


        ///Set mines
        private void Set_Mines(int count, Difficulty difficulty){
            count = 10;
        sizeY = height;
        sizeX = width;
        Random mole = new Random();

        for (int i = 0; i < count; i++) {
                int coord_x = mole.Next(sizeY);
                int coord_y = mole.Next(sizeX);

                while(Matrix.tiles[coord_x, coord_y].mine){
                    coord_x = mole.Next(sizeY); 
                    coord_y = mole.Next(sizeX);
                }

                Matrix.tiles[coord_x, coord_y].mole = true;
        }
     
    }
}
