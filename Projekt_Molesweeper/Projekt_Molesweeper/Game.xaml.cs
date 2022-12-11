using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace Projekt_Molesweeper {

    public partial class StartWindow : Window {
        
        /// <summary>
        /// Defines how tall and wide the field is
        /// </summary>
        private int sizeY; 
        private int sizeX;

        private List<List<Field>> Fields { get; set; } = new();

        DispatcherTimer _timer;
        TimeSpan _time;

        private Difficulty Difficulty { get; set; }

        public static ImageBrush Mine { get; private set; }

        public static ImageBrush Flag { get; private set; }
        

        public StartWindow() {
        }

        public StartWindow(Difficulty difficulty) {
            InitializeComponent();

            //import images from image folder
            var mineBitmapImage = new BitmapImage(new Uri("Images/mole.png", UriKind.Relative));
            Mine = new ImageBrush();
            Mine.ImageSource = mineBitmapImage;

            var flagBitmapImage = new BitmapImage(new Uri("Images/shovel.png", UriKind.Relative));
            Flag = new ImageBrush();
            Flag.ImageSource = flagBitmapImage;


            Difficulty = difficulty;

            //pass over parameter seconds from switch case to timer function to not have duplicated code
            int seconds = 0;
            
            //Timer adapted to difficulty
            switch (difficulty) {
                case Difficulty.HARD:
                    seconds = 150;
                    break;
                case Difficulty.INTERMEDIATE:
                    seconds = 350;
                    break;
                case Difficulty.EASY:
                    seconds = 600;
                    break;
            }

            _time = TimeSpan.FromSeconds(seconds);
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate {
                tbTime.Text = _time.ToString("c");
                if (_time == TimeSpan.Zero) {
                    _timer.Stop();
                    ShowGameOverDialog("Time's out!");
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();


            //defines size of field
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

       
            ///After every x add a new column, that consists of a list of fields.
            ///Is called as often as defined size of field from sizeX & sizeY
            ///TheGrid is the name of our grid in Game.xaml
            /// x and y are the position of the fields
            for (int x = 0; x < sizeX; x++) {
                Fields.Add(new List<Field>());
                this.TheGrid.RowDefinitions.Add(new RowDefinition());
                this.TheGrid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int y = 0; y < sizeY; y++) {
                    Position pos = new Position(x, y); 
                    Button button = new Button();
                    Field field = new Field(pos, button, Fields);
                    Fields[x].Add(field);
                    Grid.SetRow(button, y);
                    Grid.SetColumn(button, x);
                    button.Click += new RoutedEventHandler(this.FieldClick); //if button clicked open FieldClick method
                    button.MouseRightButtonUp += new MouseButtonEventHandler(this.FieldRightClick);

                    TheGrid.Children.Add(button);
                }
            }

            /// Adjusts the amount of mines
            Set_Mines((int)difficulty);

            /// Call InitMinesCount on EVERY field.
            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeY; y++) {
                    Fields[x][y].InitMinesCount(); //access the x and y stored in the array Fields
                }
            }
        }

        //Checks when all fields are clicked or flagged you win
        private void CheckForWin()
        {
            int clickedCount = 0;
            int flaggedCount = 0;
            for(int x = 0; x < sizeX; x++)
            {
                for(int y = 0; y < sizeY; y++)
                {
                    if (Fields[x][y].Clicked)
                    {
                        clickedCount++;
                    }
                    if (Fields[x][y].Flagged)
                    {
                        flaggedCount++;
                    }
                }
            }  ///Checks if we have uncovered as many fields as there are fields without mines and flagged as many fields as there are mines in the right difficulty.
               /// Because there are Names for Difficulty instead of numbers, we have to write int infront of it.
            if (clickedCount == (sizeX * sizeY) - (int)Difficulty && flaggedCount == (int)Difficulty)
            {
                ShowWinDialog();
            }
        }

        private void ShowWinDialog()
        {
            ShowGameOverDialog("Congrats! °˖✧◝(⁰▿⁰)◜✧˖° You Won! Restart?");
        }

        //Starts a new game, when New Game Button is clicked
        public void OnClickNewGame(object sender, RoutedEventArgs e){
            new StartWindow(Difficulty).Show();
            this.Close();
        }


        public void FieldClick(object sender, RoutedEventArgs e) {
            // Implicit casting
            Button btn = (Button)sender;

            Field field = ButtonToField(btn);
            field.OnClick();
            if (field.Mine)
            {
                ShowLostDialog();
            }
            CheckForWin();
        }

        private void ShowLostDialog()
        {
            ShowGameOverDialog("You Lost!  (╥﹏╥)  Restart?");
        }

        private void ShowGameOverDialog(string text)
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Exclamation;
            MessageBoxResult result = MessageBox.Show(text, "Game Over", button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                new StartWindow(Difficulty).Show();
                Window.GetWindow(this).Close(); // closes current window
            }
        }

        /// <summary>
        /// Used to flag a mine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldRightClick(object sender, MouseButtonEventArgs e) {
            // Implicit casting
            Button btn = (Button)sender;

            Field field = ButtonToField(btn);
            field.OnRightClick();
        }


        /// <summary>
        /// Turns all the fields from the 2D array to buttons
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        private Field ButtonToField(Button btn) {

            int x = Grid.GetColumn(btn);
            int y = Grid.GetRow(btn);

            return Fields[x][y];
        }

        /// <summary>
        /// Sets Mines on random positions of the grid
        /// </summary>
        /// <param name="count">Set_Mines increases the count until it has reached its limit, which is defined in the public enum difficulty</param>
        private void Set_Mines(int count) {
            Random r = new Random();

            for (int i = 0; i < count; i++) {
                int coord_x = r.Next(sizeX);
                int coord_y = r.Next(sizeY);

                Fields[coord_x][coord_y].Mine = true;
            }
        }

        /// <summary>
        /// Links help button from Game.xaml to website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", "http://127.0.0.1:5500/CODE/main.html");
        }
    }
    public enum Sign
    {
        DEFAULT,
        FLAGGED,
        MINE,
        COUNT,
        EMPTY
    }

    public class Field {
        public Sign Sign { get; private set; }
        public bool Mine { get; set; }
        public bool Clicked { get; set; } = false;
        public bool Flagged { get; set; } = false;
        public Position Position { get; set; }

        private List<List<Field>> Fields { get; set; }

        private Button Button { get; set; }

        public int MinesCount { get; set; }

        public event EventHandler SignChanged;


        public Field(Position position, Button button, List<List<Field>> fields) {
            Fields = fields;
            Button = button;
            Position = position;
            UpdateSign();
        }

        //Updates the sign of a button if it has been changed
        private void UpdateSign()
        {
            Sign newSign = Sign;
            if (!Clicked && !Flagged)
            {
                newSign = Sign.DEFAULT;
            }
            if (!Clicked && Flagged)
            {
                newSign = Sign.FLAGGED;
            }
            if (Clicked && Mine)
            {
                newSign = Sign.MINE;
            }
            if (Clicked && !Mine)
            {
                if (MinesCount > 0)
                {
                    newSign = Sign.COUNT;
                }
                else
                {
                    newSign = Sign.EMPTY;
                }
            }

            if(newSign != Sign)
            {
                Sign = newSign;
                UpdateButton();
            }
        }

        //Update sign and background of a button
        private void UpdateButton()
        {
            Button.Content = "";
            switch (Sign)
            {
                case Sign.DEFAULT:
                    Button.Background = Brushes.Gray; break;
                case Sign.FLAGGED:
                    Button.Background = StartWindow.Flag; break;
                case Sign.MINE:
                    Button.Background = StartWindow.Mine; break;
                case Sign.COUNT:
                    Button.Background = Brushes.White;
                    Button.Content = MinesCount; break;
                case Sign.EMPTY:
                    Button.Background = Brushes.White; break;
            }

        }


        /// <summary>
        /// Position.x and Position.y are the coordinates of a field from which we want to know the amount of mines around it.
        /// With a 3x3 grid we can calculate mines around the Position.x and Position.y field.
        /// With the double loop we calculate the positions of the new 3x3 grid, that are stord in dx/dy.
        /// If we add to each dx/dy the position of our field Position.x/Position.y, the 3x3 grid is not where Position.x/Position.y is in the middle but where it is in the top left of the grid.
        /// So that Position.x and Position.y are in the middle, we have to subtract -1 from each coordinate in the 3x3 grid.
        /// </summary>

        public void InitMinesCount() {
            int count = 0;
            
            for(int dx = 0; dx < 3; dx++){
                for(int dy = 0; dy < 3; dy++){
                    int x = Position.x - 1 + dx;              
                    int y = Position.y - 1 + dy;              
                    if(x == Position.x && y == Position.y){ 
                        continue;
                    }
                    if(x >= 0 && x < Fields.Count && y >= 0 && y < Fields[0].Count){
                        if(Fields[x][y].Mine){ //if coordinates were right we access the field with a mine and increase count
                            count++;
                        }
                    }
                }
            }

            MinesCount = count;
        }

        private Field GetFieldByPos(Position position){
            return Fields[position.x][position.y];
        }

        /// <summary>
        /// When button is clicked and it is not a mine, begin with OpenArea method
        /// </summary>
        public void OnClick() {
            Clicked = true;
            UpdateSign();
            if (!Mine){
                OpenArea(Position);
            }   
        }

        /// <summary>
        /// Calculates the adjacent fields of a grid to call the method OpenArea on all of them, which then calls the method again on all of their neighbors. 
        /// So that it does repeat it endlessly, we have inserted a return condition in the first to ifs.
        /// 1. return if the current field is already clicked or a mine, don't uncover it.
        /// 2. return if the current field has a number in it, don't uncover it
        /// </summary>
        /// <param name="position">Position of the filed that the player has clicked</param>
        private void OpenArea(Position position) {
            Field field = GetFieldByPos(position);
            int x = position.x;
            int y = position.y;
            if(field != this){
                if (field.Clicked || field.Mine){
                    return;
                }
                field.Clicked = true;
                field.UpdateSign();
            }

            if(field.MinesCount > 0){ return; }

            for (int dx = 0; dx < 3; dx++) {
                for (int dy = 0; dy < 3; dy++) {
                    int nextX = field.Position.x + dx - 1;
                    int nextY = field.Position.y + dy - 1;
                    if (nextX == field.Position.x && nextY == field.Position.y) {
                        continue;
                    }
                    if (nextX >= 0 && nextX < Fields.Count && nextY >= 0 && nextY < Fields[0].Count) {
                        OpenArea(new Position(nextX, nextY));
                    }
                }
            }
        }

        public void OnRightClick() {
            if(Clicked){
                return;
            }
            Flagged = !Flagged;

            UpdateSign();
        }
    }

    public struct Position{
        public int x;
        public int y;

        public Position(int x, int y){
            this.x = x;
            this.y = y;
        }
    }
}

