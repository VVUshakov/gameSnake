namespace Snake
{
    public interface IGameRenderer
    {
        void Clear();
        void Render(GameState state);
    }
}