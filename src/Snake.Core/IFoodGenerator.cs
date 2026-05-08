namespace Snake.Core;

public interface IFoodGenerator
{
    Cell? PlaceFood(GameBoard board, IReadOnlySet<Cell> occupiedCells);
}
