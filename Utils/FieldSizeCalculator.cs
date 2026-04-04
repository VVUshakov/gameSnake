using gameSnake.Servises;

namespace gameSnake.Utils
{
    /// <summary>
    /// Вычисляет размеры игрового поля на основе габаритов сервисных сообщений.
    /// </summary>
    public static class FieldSizeCalculator
    {
        private const int BorderPadding = 4;

        /// <summary>
        /// Вычисляет размеры игрового поля.
        /// </summary>
        /// <returns>Размеры игрового поля (ширина, высота)</returns>
        public static (int width, int height) Calculate()
        {
            var messages = MessageRegistry.GetAll();
            var (fieldWidth, fieldHeight) = MessageSizer.GetMaxSize(messages);
            return (
                fieldWidth + BorderPadding,
                fieldHeight + BorderPadding
            );
        }
    }
}
