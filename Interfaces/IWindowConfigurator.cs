namespace gameSnake.Interfaces
{
    /// <summary>
    /// Абстракция настройки окна приложения.
    /// Позволяет заменить конфигурацию окна при смене платформы.
    /// </summary>
    public interface IWindowConfigurator
    {
        /// <summary>
        /// Настраивает окно приложения под размеры игрового поля.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля</param>
        /// <param name="fieldHeight">Высота игрового поля</param>
        void Configure(int fieldWidth, int fieldHeight);
    }
}
