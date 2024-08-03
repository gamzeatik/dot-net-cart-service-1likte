using _1likte_task.Entities;
using _1likte_task.Repositories;
using _1likte_task.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Test;

public class UnitTest1
{
    private readonly CartService _service;
    private readonly Mock<ICartRepository> _mockRepo;
    private readonly Context _context;

    public UnitTest1()
    {
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new Context(options);
        _mockRepo = new Mock<ICartRepository>();
        _service = new CartService(_mockRepo.Object, _context);
    }

    [Fact]
    public async Task GetCartShouldReturnsCartWhenCartExists()
    {
        // Given
        const int userId = 1;
        var cart = new Cart { UserId = userId };
        _mockRepo.Setup(m => m.GetCart(userId)).ReturnsAsync(cart);

        // When
        var result = await _service.GetCartAsync(userId);

        // Then
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        _mockRepo.Verify(m => m.GetCart(userId));
    }

    [Fact]
    public async Task RemoveItemWhenItemExists()
    {
        // Arrange
        const int userId = 1;
        const int productId = 2;
        var productStoreId = Guid.NewGuid();
        var cartItem = new CartItem { ProductId = productId, Quantity = 1, Price = 250 , ProductStoreId = productStoreId};
        var cart = new Cart { UserId = userId, Items = new List<CartItem> { cartItem } };
        _mockRepo.Setup(x => x.GetCart(userId)).ReturnsAsync(cart);
        _mockRepo.Setup(x => x.RemoveItem(userId, productId, productStoreId))
            .Callback<int, int, Guid>((uid, pid, psid) =>
                cart.Items.RemoveAll(i => i.ProductId == pid && i.ProductStoreId == psid));

        // Act
        await _service.RemoveItemAsync(userId, productId, productStoreId);

        // Assert
        _mockRepo.Verify(m => m.RemoveItem(userId, productId, productStoreId), Times.Once);
        Assert.DoesNotContain(cartItem, cart.Items);
    }

    [Fact]
    public async Task ClearCartWhenCartExists()
    {
        // Arrange
        const int userId = 1;
        var cart = new Cart
        {
            UserId = userId, Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 1, Price = 250 } }
        };
        _mockRepo.Setup(x => x.GetCart(userId)).ReturnsAsync(cart);
        _mockRepo.Setup(x => x.ClearCart(userId)).Verifiable();

        // Act
        await _service.ClearCartAsync(userId);

        // Assert
        _mockRepo.Verify(x => x.ClearCart(userId), Times.Once);
    }

    [Fact]
    public async Task AddItemShouldAddNewItemToEmptyCart()
    {
        // Arrange
        const int userId = 1;
        var newItem = new CartItem { ProductId = 2, Quantity = 1, Price = 100 };
        var cart = new Cart { UserId = userId, Items = new List<CartItem>() };

        _mockRepo.Setup(m => m.GetCart(userId)).ReturnsAsync(cart);
        _mockRepo.Setup(m => m.AddItem(userId, It.IsAny<CartItem>()))
            .Callback<int, CartItem>((id, item) => cart.Items.Add(item));


        // Act
        await _service.AddItemAsync(userId, newItem);

        // Assert
        Assert.Contains(newItem, cart.Items);
        _mockRepo.Verify(m => m.AddItem(userId, It.IsAny<CartItem>()));
    }

    [Fact]
    public async Task AddItemShouldUpdateQuantityWhenItemExists()
    {
        const int userId = 1;
        var existingItem = new CartItem { ProductId = 2, Quantity = 1, Price = 100 };
        var newItem = new CartItem { ProductId = 2, Quantity = 2, Price = 100 };
        var cart = new Cart { UserId = userId, Items = new List<CartItem> { existingItem } };

        _mockRepo.Setup(m => m.GetCart(userId)).ReturnsAsync(cart);
        _mockRepo.Setup(m => m.AddItem(userId, It.IsAny<CartItem>()))
            .Callback<int, CartItem>((id, item) =>
            {
                var foundItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (foundItem != null)
                    foundItem.Quantity += item.Quantity;
                else
                    cart.Items.Add(item);
            });

        // Act
        await _service.AddItemAsync(userId, newItem);

        // Assert
        Assert.Equal(3, existingItem.Quantity);
        _mockRepo.Verify(m => m.AddItem(userId, It.IsAny<CartItem>()));
    }

    [Fact]
    public async Task AddItemToCart()
    {
        // Arrange
        const int userId = 1;
        var existingItem = new CartItem { ProductId = 2, Quantity = 1, Price = 100 };
        var cart = new Cart { UserId = userId, Items = new List<CartItem> { existingItem } };

        var newItem = new CartItem { ProductId = 3, Quantity = 2, Price = 100 };

        _mockRepo.Setup(m => m.GetCart(userId)).ReturnsAsync(cart);
        _mockRepo.Setup(m => m.AddItem(userId, It.IsAny<CartItem>()))
            .Callback<int, CartItem>((id, item) =>
            {
                var foundItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (foundItem != null)
                    foundItem.Quantity += item.Quantity;
                else
                    cart.Items.Add(item);
            });
        // Act
        await _service.AddItemAsync(userId, newItem);

        // Assert
        Assert.Equal(2, cart.Items.Count);
        _mockRepo.Verify(m => m.AddItem(userId, It.IsAny<CartItem>()));
    }
}