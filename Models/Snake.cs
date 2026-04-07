namespace gameSnake.Models
{
    /// <summary>
    /// Представляет тело змейки как упорядоченный список точек.
    /// </summary>
    public class Snake
    {
        /// <summary>
        /// Список точек тела. Body[0] — хвост, Body[Body.Count - 1] — голова.
        /// </summary>
        public List<Point> Body { get; set; }

        /// <summary>
        /// Позиция головы (последний элемент тела)
        /// </summary>
        public Point Head => Body[Body.Count - 1];

        /// <summary>
        /// Позиция хвоста (первый элемент тела)
        /// </summary>
        public Point Tail => Body[0];

        /// <summary>
        /// Создаёт змейку из готового списка сегментов.
        /// </summary>
        /// <param name="body">Упорядоченный список точек тела</param>
        /// <exception cref="ArgumentException">Если тело пустое</exception>
        public Snake(IEnumerable<Point> body)
        {
            Body = new List<Point>(body);
            if (Body.Count == 0) throw new ArgumentException("Snake body cannot be empty");
        }

        /// <summary>
        /// Проверяет, содержит ли змейка указанную точку.
        /// </summary>
        public bool Contains(Point point)
        {
            foreach (Point segment in Body)
                if (segment.X == point.X && segment.Y == point.Y) return true;
            return false;
        }
    }
}
