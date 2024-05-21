using System;
using System.Collections.Generic;

using Application.Pocos;
using Application.Repositories.Interfaces;
using Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Tests.Services
{
    [TestClass]
    public class BorrowHistoryServiceTests
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper = null!;
        private Mock<IBorrowHistoryRepository> _mockBorrowHistoryRepository = null!;
        private BorrowHistoryService _borrowHistoryService = null!;

        [TestInitialize]
        public void SetUp()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockBorrowHistoryRepository = new Mock<IBorrowHistoryRepository>();
            _mockRepositoryWrapper.Setup(rw => rw.BorrowHistoryRepository)
                                  .Returns(_mockBorrowHistoryRepository.Object);

            _borrowHistoryService = new BorrowHistoryService(_mockRepositoryWrapper.Object);
        }

        [TestMethod]
        public void GetHistoryForUser_ReturnsHistoryForUser()
        {
            var userId = Guid.NewGuid();
            var expectedHistory = new List<HistoryPoco>
            {
                new HistoryPoco
                {
                    BookIsbn = "12345",
                    BookTitle = "Test Book",
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    BorrowDate = DateTime.Now,
                },
            };
            _mockBorrowHistoryRepository.Setup(repo => repo.GetHistoryForUser(userId))
                                        .Returns(expectedHistory);

            var result = _borrowHistoryService.GetHistoryForUser(userId);

            Assert.AreEqual(expectedHistory, result);
            _mockBorrowHistoryRepository.Verify(repo => repo.GetHistoryForUser(userId), Times.Once);
        }

        [TestMethod]
        public void BorrowBook_CallsRepositoryToBorrowBook()
        {
            var borrowHistory = new BorrowHistoryPoco
            {
                UserId = Guid.NewGuid(),
                BookIsbn = "12345",
                LibraryId = Guid.NewGuid(),
                BorrowDate = DateTime.Now,
            };

            _borrowHistoryService.BorrowBook(borrowHistory);

            _mockBorrowHistoryRepository.Verify(repo => repo.BorrowBook(borrowHistory), Times.Once);
        }

        [TestMethod]
        public void GetHistoryForUser_ReturnsEmptyList_WhenNoHistoryFound()
        {
            var userId = Guid.NewGuid();
            var expectedHistory = new List<HistoryPoco>();

            _mockBorrowHistoryRepository.Setup(repo => repo.GetHistoryForUser(userId))
                                        .Returns(expectedHistory);

            var result = _borrowHistoryService.GetHistoryForUser(userId);

            Assert.AreEqual(expectedHistory, result);

            // Assert.AreEqual(0, result.Count);
            _mockBorrowHistoryRepository.Verify(repo => repo.GetHistoryForUser(userId), Times.Once);
        }

        [TestMethod]
        public void BorrowBook_WithFutureBorrowDate_CallsRepositoryToBorrowBook()
        {
            var borrowHistory = new BorrowHistoryPoco
            {
                UserId = Guid.NewGuid(),
                BookIsbn = "12345",
                LibraryId = Guid.NewGuid(),
                BorrowDate = DateTime.Now.AddDays(1), // Future borrow date
            };

            _borrowHistoryService.BorrowBook(borrowHistory);

            _mockBorrowHistoryRepository.Verify(repo => repo.BorrowBook(borrowHistory), Times.Once);
        }
    }
}
