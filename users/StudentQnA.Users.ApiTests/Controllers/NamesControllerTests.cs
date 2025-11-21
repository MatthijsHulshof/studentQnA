using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentQnA.Users.Api.Controllers;
using StudentQnA.Users.Api.Data;
using StudentQnA.Users.Api.Models;

namespace StudentQnA.Users.Api.Tests
{
    [TestClass]
    public class NamesControllerTests
    {
        private AppDbContext _context;
        private NamesController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _controller = new NamesController(_context);
        }

        [TestMethod]
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
            var actionResult = result as ActionResult<IEnumerable<NameEntity>>;
            Assert.IsNotNull(actionResult);

            var names = actionResult.Value;
            Assert.IsNotNull(names);

            Assert.AreEqual(2, names.Count());
        }

        [TestMethod]
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
            var actionResult = result as ActionResult<NameEntity>;
            Assert.IsNotNull(actionResult);

            var createdResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);

            var returnValue = createdResult.Value as NameEntity;
            Assert.IsNotNull(returnValue);

            Assert.AreEqual("Linda", returnValue.Value);
            Assert.AreEqual(1, _context.Names.Count());
        }

        [TestMethod]
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
            Assert.IsNotNull(saved);
            Assert.AreEqual("Pieter", saved.Value);
        }
    }
}
