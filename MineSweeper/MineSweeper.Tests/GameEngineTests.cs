using Xunit;
using Moq;
using SerializerLib;
using System.Linq;
using MineSweeperEngine;

namespace MineSweeper.Tests
{
    public class GameEngineTests
    {
        private readonly Mock<ISerializer> _mockSerializer;

        public GameEngineTests()
        {
            _mockSerializer = new Mock<ISerializer>();
        }

        [Fact]
        public void NewGameShouldCreateCorrectGameBoard()
        {
            // Arrange

            // Act
            GameEngine game = new GameEngine(_mockSerializer.Object);
            game.NewGame();

            // Assert
            Assert.Equal(GameStatus.InProgress, game.GameBoard.Status);
            Assert.Equal(GameEngine.NUM_COLUMNS * GameEngine.NUM_ROWS, game.GameBoard.Cells.Count);
            Assert.Equal(GameEngine.NUM_MINES, game.GameBoard.Cells.Count(c => c.IsMine));
            Assert.Equal(0, game.GameBoard.Cells.Count(c => c.IsRevealed));
            Assert.Equal(0, game.GameBoard.Cells.Count(c => c.IsFlagged));
            var mines = game.GameBoard.Cells.Where(m => m.IsMine);
            var cells = game.GameBoard.Cells.Where(c => !c.IsMine);
            foreach (Cell cell in cells)
            {
                if (!cell.IsMine)
                {
                    Assert.True(CheckAdjacentMines(game, cell));
                }
            }
        }

        private bool CheckAdjacentMines(GameEngine game, Cell cell)
        {
            int count = 0;
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX - 1 
                                                     && c.PosY == cell.PosY - 1
                                                     && c.IsMine);
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX
                                                     && c.PosY == cell.PosY - 1
                                                     && c.IsMine);
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX + 1
                                                     && c.PosY == cell.PosY - 1
                                                     && c.IsMine);
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX - 1
                                                     && c.PosY == cell.PosY
                                                     && c.IsMine);
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX + 1
                                                     && c.PosY == cell.PosY
                                                     && c.IsMine);
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX - 1
                                                     && c.PosY == cell.PosY + 1
                                                     && c.IsMine);
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX
                                                     && c.PosY == cell.PosY + 1
                                                     && c.IsMine);
            count += game.GameBoard.Cells.Count(c => c.PosX == cell.PosX + 1
                                                     && c.PosY == cell.PosY + 1
                                                     && c.IsMine);

            return count == cell.AdjacentMines;
        }
    }
}
