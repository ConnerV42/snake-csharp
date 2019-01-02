using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Snake
{
    [Serializable]
    internal class TheSnake
    {
        private int snakeThickness = 15;
        private int currentDirection;
        private int previousDirection;
        private Point currentPosition;
        private int snakeLength;
        private int amountOfPoints;

        public List<Point> snakeBody;
        //THINGS TO ADD: POWERUPS, SCORE, HIGHSCORE THAT REMAINS AFTER CLOSE
        public TheSnake()
        {
            this.currentPosition = new Point(420, 500);
            this.currentDirection = 0; //when initially starting, previous direction
            this.previousDirection = 1; //must be set to opposite of current direction
            this.snakeBody = new List<Point>();
            this.snakeLength = 10;
        }

        //Properies (getters & setters) 
        public int CurrentLength { get => this.snakeLength; set => this.snakeLength = value; }

        public int PointCount { get => this.snakeBody.Count; }

        public double Y { get => this.currentPosition.Y; set => this.currentPosition.Y = value; }

        public double X { get => this.currentPosition.X; set => this.currentPosition.X = value; }

        public int CurrentDirection { get => this.currentDirection; set => this.currentDirection = value; }

        public int PreviousDirection { get => this.previousDirection; set => this.previousDirection = value; }

        public int SnakeThickness { get => this.snakeThickness; }

        public Point CurrentPosition { get => this.currentPosition; set => this.currentPosition = value; }
    
        public Point this[int index]
        {
            get
            {
                if (index >= this.CurrentLength || index < 0)
                    throw new IndexOutOfRangeException();
                return this.snakeBody[index];
            }

            set
            {
                this.snakeBody[this.CurrentLength - 1] = value;
            }
        }
    }
}