using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentQnA.Users.Api.Controllers;
using StudentQnA.Users.Api.Data;
using StudentQnA.Users.Api.Models;
using Xunit;

namespace StudentQnA.Users.Api.Tests
{
    public class NamesControllerTests
    {
        private readonly AppDbContext _context;
        private readonly NamesController _controller;

        public NamesControllerTests()
        {
            // Use an InMemory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);

            // Empty database for every test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _controller = new NamesController(_context);
        }

        [Fact]
        public async Task GetNames_ReturnsAllNames()
        {
            // Arrange
            _context.Names.AddRange(
                new NameEntity { Id = 1, Value = "Matthijs", CreatedAt = DateTime.UtcNow },
                new NameEntity { Id = 2, Value = "Jan", CreatedAt = DateTime.UtcNow }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetNames();

            // Assert
            var actionResult = Xunit.Assert.IsType<ActionResult<IEnumerable<NameEntity>>>(result);
            var names = Xunit.Assert.IsAssignableFrom<IEnumerable<NameEntity>>(actionResult.Value);
            Xunit.Assert.Equal(2, names.Count());
        }

        [Fact]
        public async Task PostName_AddsNameAndReturnsCreatedAtAction()
        {
            // Arrange
            var newName = new NameEntity
            {
                Value = "Linda",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _controller.PostName(newName);

            // Assert
            var actionResult = Xunit.Assert.IsType<ActionResult<NameEntity>>(result);
            var createdResult = Xunit.Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Xunit.Assert.IsType<NameEntity>(createdResult.Value);

            Xunit.Assert.Equal("Linda", returnValue.Value);
            Xunit.Assert.Equal(1, _context.Names.Count());
        }

        [Fact]
        public async Task PostName_SavesToDatabase()
        {
            // Arrange
            var entity = new NameEntity
            {
                Value = "Pieter",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _controller.PostName(entity);

            // Assert
            var saved = await _context.Names.FirstOrDefaultAsync(n => n.Value == "Pieter");
            Xunit.Assert.NotNull(saved);
            Xunit.Assert.Equal("Pieter", saved.Value);
        }
    }
}
