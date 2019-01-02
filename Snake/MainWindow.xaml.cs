using System;
using System.Collections.Generic;
using System.IO;
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
using System.Reflection;

using System.Runtime.Serialization.Formatters.Binary;

// Defined within System.Xml.dll.
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double speedMultiplier;
        private int UP = 0;
        private int RIGHT = 1;
        private int DOWN = 2;
        private int LEFT = 3;

        private TheSnake snake;
        private DispatcherTimer timer;
        private Boolean paused;
        private Boolean currentlyPlaying;
        private List<Point> foodOnBoard;
        private Random random = new Random();
        private int currentScore;
        private int highScore;

        public MainWindow()
        {
            InitializeComponent();
            Boolean paused = false;
            Boolean currentlyPlaying = false;
     

            this.highScore = (int) LoadFromBinaryFile(System.IO.Path.GetFullPath("Highscore.dat"));
            speedMultiplier = 0.5;
            foodOnBoard = new List<Point>();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += Timer_Tick;
            KeyDown += MainWindow_KeyDown;
            snake = new TheSnake();

            animateSnake(snake.CurrentPosition); //after this moment, the total number of objects on the canvas is 1
            for (int i = 0; i < 10; i++)
            {
                generateFood(i); //after this moment, the total number of objects on the canvas is 11
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!currentlyPlaying)
            {
                currentlyPlaying = true;
                currentScoreDisplay.Text = "Current Score: " + currentScore;
                highScoreDisplay.Text = "High Score: " + highScore;
                timer.Start();
            }
        }

        private void animateSnake(Point currentPosition)
        {
            Ellipse e = new Ellipse();
            e.Fill = Brushes.Green;
            e.Width = snake.SnakeThickness;
            e.Height = snake.SnakeThickness;

            Canvas.SetTop(e, snake.Y);
            Canvas.SetLeft(e, snake.X);

            int count = gameBoard.Children.Count;
            gameBoard.Children.Add(e);

            snake.snakeBody.Add(currentPosition);

            if(count > snake.CurrentLength)
            {
                gameBoard.Children.RemoveAt(count - snake.CurrentLength + 9);
                snake.snakeBody.RemoveAt(count - snake.CurrentLength);
            }
        }

        private void generateFood(int index)
        {
            Rectangle r = new Rectangle(); //r will represent the food on the gameBoard (canvas object)
            if (index % 3 == 0)
                r.Fill = Brushes.OrangeRed;
            else if (index % 2 == 0)
                r.Fill = Brushes.Violet;
            else
                r.Fill = Brushes.DeepSkyBlue;

            r.Width = snake.SnakeThickness*(3/2);
            r.Height = snake.SnakeThickness*(3/2);

            Point food = new Point(random.Next(snake.SnakeThickness*(3/2), (int)gameBoard.Width - snake.SnakeThickness*(3/2)), 
                random.Next(snake.SnakeThickness*(3/2), (int)gameBoard.Height - snake.SnakeThickness*(3/2)));
            Canvas.SetTop(r, food.Y);
            Canvas.SetLeft(r, food.X);

            gameBoard.Children.Insert(index, r);
            foodOnBoard.Insert(index, food);

            speedMultiplier += 0.05;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.W || e.Key == Key.Up)
            {
                if(snake.PreviousDirection != DOWN)
                    snake.CurrentDirection = UP;
            }
            else if(e.Key == Key.D || e.Key == Key.Right)
            {
                if(snake.PreviousDirection != LEFT)
                    snake.CurrentDirection = RIGHT;
            }
            else if(e.Key == Key.S || e.Key == Key.Down)
            {
                if(snake.PreviousDirection != UP)
                    snake.CurrentDirection = DOWN;
            }
            else if(e.Key == Key.A || e.Key == Key.Left)
            {
                if(snake.PreviousDirection != RIGHT)
                snake.CurrentDirection = LEFT;
            }
            snake.PreviousDirection = snake.CurrentDirection;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(snake.CurrentDirection == UP)
            {
                snake.Y -= speedMultiplier;
            }
            else if(snake.CurrentDirection == RIGHT)
            {
                snake.X += speedMultiplier;
            }
            else if(snake.CurrentDirection == DOWN)
            {
                snake.Y += speedMultiplier;
            }
            else
            {
                snake.X -= speedMultiplier;
            }
            animateSnake(snake.CurrentPosition);

            if ((snake.Y <= 0 || snake.Y + snake.SnakeThickness >= gameBoard.Height) 
                || (snake.X <= 0 || snake.X + snake.SnakeThickness >= gameBoard.Width))
            {
                GameOver();
            }

            int i = 0;
            foreach (Point point in foodOnBoard)
            {
                if ((Math.Abs(point.X - snake.X) < snake.SnakeThickness*(3/2) &&
                    (Math.Abs(point.Y - snake.Y) < snake.SnakeThickness*(3/2))))
                {
                    this.currentScore += 10;
                    currentScoreDisplay.Text = "Current Score: " + this.currentScore;
                    snake.CurrentLength += 10;

                    //delete & replace food that was eaten
                    //b/c of collision with head
                    foodOnBoard.RemoveAt(i);
                    gameBoard.Children.RemoveAt(i);
                    generateFood(i);
                    break;
                }
                i++;
            }

            for (int j = 1; j < snake.PointCount - snake.SnakeThickness*2; j++)
            {
                Point point = new Point(snake[j].X, snake[j].Y);
                if((Math.Abs(point.X - snake.X) < snake.SnakeThickness) &&
                  (Math.Abs(point.Y - snake.Y) < snake.SnakeThickness)) 
                {
                    GameOver();
                    break;
                }
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (!paused && currentlyPlaying)
            {
                timer.Stop();
                this.btnPause.Content = "Resume";
                this.paused = true;
            }
            else if (currentlyPlaying && paused)
            {
                this.paused = false;
                this.btnPause.Content = "Pause";
                timer.Start();
            }
        }

        private void GameOver()
        {
            if (currentlyPlaying)
            {
                currentlyPlaying = false;
                timer.Stop();
                if (currentScore > highScore)
                {
                    highScore = currentScore;
                    SaveAsBinaryFormat(highScore, System.IO.Path.GetFullPath("Highscore.dat"));
                    MessageBox.Show("Congratulations! You have achieved a new high score: " + highScore);
                    highScoreDisplay.Text = "High Score: " + highScore;
                }
                else
                {
                    MessageBox.Show("Game Over! You scored " + currentScore);
                }
                resetVariables();
            }
        }

        private void resetVariables() //Sets up a new Snake, Canvas and populates with head of snake and food
        {
            snake = new TheSnake();
            gameBoard.Children.Clear();
            animateSnake(snake.CurrentPosition); 
            foodOnBoard = new List<Point>();
            currentScore = 0;
            speedMultiplier = 0.5;

            for (int i = 0; i < 10; i++)
            {
                generateFood(i);
            }
        }

        private void SaveGame()
        {
            SaveGameBoard(this.gameBoard);
            SaveAsBinaryFormat(this.currentScore, System.IO.Path.GetFullPath("CurrentScoreData.dat"));
            SaveAsBinaryFormat(snake, System.IO.Path.GetFullPath("SnakeData.dat"));
            SaveAsBinaryFormat(foodOnBoard, System.IO.Path.GetFullPath("FoodData.dat"));
            SaveAsBinaryFormat(this.speedMultiplier, System.IO.Path.GetFullPath("SpeedData.dat"));
        }

        private void LoadGame() //Replaces current main game objects with the objects that have been saved previously
        {
            if (LoadGameBoard())
            {
                this.snake = (TheSnake)LoadFromBinaryFile(System.IO.Path.GetFullPath("SnakeData.dat"));
                this.foodOnBoard = (List<Point>)LoadFromBinaryFile(System.IO.Path.GetFullPath("FoodData.dat"));
                this.currentScore = (int)LoadFromBinaryFile(System.IO.Path.GetFullPath("CurrentScoreData.dat"));
                this.currentScoreDisplay.Text = "Current Score: " + this.currentScore;
                this.speedMultiplier = (double)LoadFromBinaryFile(System.IO.Path.GetFullPath("SpeedData.dat"));
            }
        }

        static void SaveAsBinaryFormat(object obj, string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream fStream = new FileStream(fileName,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                bf.Serialize(fStream, obj);
            }
        }

        static void SaveGameBoard(Canvas canvas)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "CanvasData"; // Default file name
            dlg.DefaultExt = ".xaml"; // Default file extension
            dlg.Filter = "Xaml File (.xaml)|*.xaml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                SerializeToXAML(canvas, filename);
            }
        }

        public bool LoadGameBoard()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xaml"; // Default file extension
            dlg.Filter = "Xaml File (.xaml)|*.xaml"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                this.gameBoard.Children.Clear();
                string filename = dlg.FileName;
                Canvas canvas = DeSerializeXAML(filename) as Canvas;

                // Add all child elements (lines, rectangles etc) to canvas
                while (canvas.Children.Count > 0)
                {
                    UIElement obj = canvas.Children[0]; // Get next child
                    canvas.Children.Remove(obj); // Have to disconnect it from result before we can add it
                    this.gameBoard.Children.Add(obj); // Add to canvas
                }
                return true;
            }
            return false;
        }

        public static void SerializeToXAML(UIElement element, string filename)
        {// Serializes any UIElement object to XAML using a given filename.
            // Use XamlWriter object to serialize element
            string strXAML = System.Windows.Markup.XamlWriter.Save(element);

            // Write XAML to file. Use 'using' so objects are disposed of properly.
            using (System.IO.FileStream fs = System.IO.File.Create(filename))
            {
                using (System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(fs))
                {
                    streamwriter.Write(strXAML);
                }
            }
        }

        public static UIElement DeSerializeXAML(string filename)
        {// De-Serialize XML to UIElement using a given filename.
            // Load XAML from file. Use 'using' so objects are disposed of properly.
            using (System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                return System.Windows.Markup.XamlReader.Load(fs) as UIElement;
            }
        }

        static object LoadFromBinaryFile(string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter();

            // Read the object from the binary file.
            using (Stream fStream = File.OpenRead(fileName))
            {
                return binFormat.Deserialize(fStream);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if(!currentlyPlaying || (currentlyPlaying && paused)) //must not be currently playing in order to load a saved game
            {
                LoadGame();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(paused && currentlyPlaying)
            {
                SaveGame();
            }
            else if(!paused) {
                MessageBox.Show("Sorry, the game must be paused in order to save the current state.");
            }
            else if(!paused && !currentlyPlaying)
            {
                MessageBox.Show("Sorry, there must be an active game occurring in order to save the game.");
            }
        }
    }
}
