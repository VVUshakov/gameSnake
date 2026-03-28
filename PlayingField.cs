namespace Snake
{
    public class PlayingField
    {
        public int Width { get; }
        public int Height { get; }

        public PlayingField(int width = 20, int height = 10)
        {
            Width = width;
            Height = height;
        }
    }
}