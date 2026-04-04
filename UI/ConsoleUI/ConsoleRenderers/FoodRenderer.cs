using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Отвечает за отрисовку еды на игровом поле.
    /// Еда отображается только если она успешно создана и находится внутри поля.
    /// </summary>
    public static class FoodRenderer
    {
        /// <summary>
        /// Отрисовывает еду на игровом поле.
        /// Пропускает отрисовку, если еда не создана или находится за пределами поля.
        /// </summary>
        /// <param name="food">Еда с позицией</param>
        /// <param name="field">Игровое поле для проверки границ</param>
        /// <param name="headerHeight">Высота заголовка для смещения по вертикали</param>
        public static void Draw(Food food, PlayingField field, int headerHeight)
        {
            if (!food.IsSuccess || food.Position == null) return;
            Point pos = food.Position;
            if (!field.IsInside(pos)) return;
            Console.SetCursorPosition(pos.X, pos.Y + headerHeight);
            Console.Write(RenderConstants.FoodSymbol);
        }
    }
}
