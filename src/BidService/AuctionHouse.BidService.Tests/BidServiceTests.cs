using AuctionHouse.BidService.Domain.Entities;
using AuctionHouse.BidService.Rabbit;
using AuctionHouse.BidService.Service.Repositories;
using AuctionHouse.BidService.Service.Services;
using Moq;

namespace AuctionHouse.BidService.Tests
{
    [TestClass]
    public class BidServiceTests
    {
        private Mock<IAvailableAuctionsRepository> _availableAuctionsRepositoryMock;
        private Mock<IBidRepository> _bidRepositoryMock;
        private Mock<IBidPublisherService> _publisherServiceMock;
        private IBidService _bidService;

        [TestInitialize]
        public void Setup()
        {
            _availableAuctionsRepositoryMock = new Mock<IAvailableAuctionsRepository>();
            _bidRepositoryMock = new Mock<IBidRepository>();
            _publisherServiceMock = new Mock<IBidPublisherService>();

            _bidService = new Service.Services.BidService(
                _availableAuctionsRepositoryMock.Object,
                _bidRepositoryMock.Object,
                _publisherServiceMock.Object
            );
        }

        [TestMethod]
        public async Task PlaceBid_ReturnsGuidAndPublishes_WhenAuctionIsStarted()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var amount = 100m;
            var userId = 42;

            _availableAuctionsRepositoryMock
                .Setup(repo => repo.GetAuctionState(auctionId))
                .ReturnsAsync(AuctionType.AuctionStarted);

            // Act
            var result = await _bidService.PlaceBid(auctionId, amount, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.Value);
            _bidRepositoryMock.Verify(r => r.CreateBid(It.Is<Bid>(b =>
                b.AuctionId == auctionId &&
                b.BidAmount == amount &&
                b.UserId == userId &&
                b.BidId == result.Value
            )), Times.Once);
        }

        [TestMethod]
        public async Task PlaceBid_ReturnsNull_WhenAuctionIsNotStarted()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var amount = 50m;
            var userId = 10;

            // Auction is not started
            _availableAuctionsRepositoryMock
                .Setup(repo => repo.GetAuctionState(auctionId))
                .ReturnsAsync(AuctionType.AuctionFinished);

            // Act
            var result = await _bidService.PlaceBid(auctionId, amount, userId);

            // Assert
            Assert.IsNull(result);
            _bidRepositoryMock.Verify(r => r.CreateBid(It.IsAny<Bid>()), Times.Never);
            _publisherServiceMock.Verify(p => p.PublishMessage(It.IsAny<string>()), Times.Never);
        }
    }
}
