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
        private Difficulty difficulty;

        public MainWindow() {
            InitializeComponent();

        }

        private void Button_Click_Difficulty(object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            switch(button.Name){
                case "btn_hard":
                    difficulty = Difficulty.HARD;
                    break;
                case "btn_intermediate":
                    difficulty = Difficulty.INTERMEDIATE;
                    break;
                case "btn_EASY":
                    difficulty = Difficulty.EASY;
                    break;
            }
        }

        ///
        private void Button_Click_Start(object sender, RoutedEventArgs e) {
            var start = new Start(difficulty);
            start.Show();
        }
    }


    public enum Difficulty { HARD, EASY, INTERMEDIATE } ;

}