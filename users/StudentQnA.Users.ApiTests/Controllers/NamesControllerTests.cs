using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentQnA.Users.Api.Controllers;
using StudentQnA.Users.Api.Data;
using StudentQnA.Users.Api.Models;
using StudentQnA.Users.Api.Service;

namespace StudentQnA.Users.Api.Tests
{
    [TestClass]
    public class NamesControllerTests
    {
        private AppDbContext _context;
        private INameService _service;
        private NamesController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            _service = new NameService(_context);
            _controller = new NamesController(_service);
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
            var result = await _controller.GetNames(CancellationToken.None);

            // Assert
            var ok = result.Result as OkObjectResult;
            Assert.IsNotNull(ok);

            var names = ok.Value as IEnumerable<NameEntity>;
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
            var result = await _controller.PostName(newName, CancellationToken.None);

            // Assert
            var createdAt = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAt);

            Assert.AreEqual(nameof(NamesController.GetNames), createdAt.ActionName);

            var returnValue = createdAt.Value as NameEntity;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("Linda", returnValue.Value);

            Assert.AreEqual(1, await _context.Names.CountAsync());
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
            await _controller.PostName(entity, CancellationToken.None);

            // Assert
            var saved = await _context.Names.FirstOrDefaultAsync(n => n.Value == "Pieter");
            Assert.IsNotNull(saved);
            Assert.AreEqual("Pieter", saved.Value);
        }
    }
}
