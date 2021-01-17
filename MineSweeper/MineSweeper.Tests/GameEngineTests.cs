using Xunit;
using Moq;
using SerializerLib;
using System.Linq;

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
        public void NewGameShouldCreateGameBoard()
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
        }
    }
}
