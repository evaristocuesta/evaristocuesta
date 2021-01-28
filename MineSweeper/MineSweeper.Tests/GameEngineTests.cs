using Xunit;
using Moq;
using SerializerLib;
using System.Linq;
using MineSweeperEngine;
using System.IO.Abstractions;
using System;

namespace MineSweeper.Tests
{
    public class GameEngineTests
    {
        private readonly Mock<ISerializer> _mockSerializer;
        private readonly Mock<IFileSystem> _mockFileSystem;

        public GameEngineTests()
        {
            _mockSerializer = new Mock<ISerializer>();
            _mockFileSystem = new Mock<IFileSystem>();
        }

        [Fact]
        public void NewGameShouldCreateCorrectGameBoard()
        {
            // Arrange

            // Act
            GameEngine game = new GameEngine(_mockSerializer.Object, 
                                             _mockFileSystem.Object);
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

        [Theory]
        [InlineData(3, 5)]
        [InlineData(1, 8)]
        [InlineData(5, 2)]
        [InlineData(9, 4)]
        public void FlagCellShouldWorkCorrecly(int x, int y)
        {
            // Arrange

            // Act
            GameEngine game = new GameEngine(_mockSerializer.Object,
                                             _mockFileSystem.Object);
            game.NewGame();
            bool cellFlaggedBefore = game.SearchCell(x, y)!.IsFlagged;
            game.FlagCell(x, y);
            bool cellFlagedAfter = game.SearchCell(x, y)!.IsFlagged;

            // Assert
            Assert.False(cellFlaggedBefore);
            Assert.True(cellFlagedAfter);
        }

        [Theory]
        [InlineData(-3, 5)]
        [InlineData(11, 8)]
        [InlineData(5, -2)]
        [InlineData(9, 40)]
        public void FlagCellShouldThrowIndexOutOfRangeException(int x, int y)
        {
            // Arrange

            // Act
            GameEngine game = new GameEngine(_mockSerializer.Object,
                                             _mockFileSystem.Object);
            game.NewGame();

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => game.FlagCell(x, y));
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
