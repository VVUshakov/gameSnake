namespace gameSnake.Models
{
    /// <summary>
    /// Представляет точку на игровом поле с координатами X и Y.
    /// Является типом-значением с семантикой сравнения по значению.
    /// </summary>
    public record struct Point(int X, int Y);
}
