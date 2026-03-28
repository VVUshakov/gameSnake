namespace Snake
{
    public class Snake
    {
        // Список всех сегментов змейки: Body[0] - хвост, [Body.Count - 1] - голова
        public List<Point> Body { get; set; }

        // Для удобства - вычисляемые свойства
        public Point Head => Body[Body.Count - 1];  // координаты головы
        public Point Tail => Body[0];               // координаты хвоста

        public Snake(
            Point headPosition,     // позиция головы
            Direction direction,    // начальное направление движения 
            int snakeLength = 3     // длина змейки
        )
        {
            // Защита: длина змейки должна быть хотя бы 1
            if(snakeLength < 1) snakeLength = 1;

            // Создаём список для тела змейки
            Body = new List<Point>();

            // Строим змейку от хвоста к голове
            // i = snakeLength-1 (хвост) ... 0 (голова)
            for(int i = snakeLength - 1; i >= 0; i--)
            {
                switch(direction)
                {
                    case Direction.Right:
                        // При движении вправо: хвост слева, голова справа
                        Body.Add(new Point(x: headPosition.X - i, y: headPosition.Y));
                        break;

                    case Direction.Left:
                        // При движении влево: хвост справа, голова слева
                        Body.Add(new Point(x: headPosition.X + i, y: headPosition.Y));
                        break;

                    case Direction.Up:
                        // При движении вверх: хвост снизу, голова сверху
                        Body.Add(new Point(x: headPosition.X, y: headPosition.Y + i));
                        break;

                    case Direction.Down:
                        // При движении вниз: хвост сверху, голова снизу
                        Body.Add(new Point(x: headPosition.X, y: headPosition.Y - i));
                        break;

                    default:
                        throw new ArgumentException($"Неизвестное направление: {direction}");
                }
            }
        }

        /// <summary>
        /// Проверяет, содержит ли змейка указанную точку
        /// </summary>
        public bool Contains(Point point)
        {
            foreach(Point segment in Body)
            {
                if(segment.X == point.X && segment.Y == point.Y)
                    return true;
            }
            return false;
        }
    }
}