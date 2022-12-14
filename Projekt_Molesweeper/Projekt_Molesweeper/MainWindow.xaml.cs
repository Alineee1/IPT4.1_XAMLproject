using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
        private Difficulty difficulty;

        public MainWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// Change button color when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonColorChange(object sender, RoutedEventArgs e)
        {
            Button? button = e.Source as Button;
            button.Background = Brushes.LightSteelBlue;
        }
        
        /// <summary>
        /// Sets difficulty when button clicked, that will be used in Game.xaml.cs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Difficulty(object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            switch(button.Name){
                case "btn_hard":
                    difficulty = Difficulty.HARD;
                    break;
                case "btn_intermediate":
                    difficulty = Difficulty.INTERMEDIATE;
                    break;
                case "btn_easy":
                    difficulty = Difficulty.EASY;
                    break;
            }
        }

        /// <summary>
        /// Starts game on Game.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Start(object sender, RoutedEventArgs e) {
            var start = new StartWindow(difficulty);
            start.Show();
        }
    }


    public enum Difficulty { 
        EASY = 15,
        INTERMEDIATE = 30,
        HARD = 50
    } ;

}