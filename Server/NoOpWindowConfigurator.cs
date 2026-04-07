using gameSnake.Interfaces;

namespace gameSnake.Server
{
    /// <summary>
    /// Пустая реализация конфигуратора окна для серверной среды.
    /// На сервере нет окна — размеры игрового поля хранятся в GameState.
    /// </summary>
    public class NoOpWindowConfigurator : IWindowConfigurator
    {
        public void Configure(int fieldWidth, int fieldHeight) { }
    }
}
