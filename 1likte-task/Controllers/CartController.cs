using _1likte_task.Entities;
using _1likte_task.Services;
using Microsoft.AspNetCore.Mvc;

namespace _1likte_task.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;


    public CartController(ICartService cartService, ILogger<CartController> logger)
    {
        _cartService = cartService;
        _logger = logger;
    }


    // GET
    [HttpGet("/{userId:int}")]
    public async Task<ActionResult<Cart>> GetCart(int userId)
    {
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null)
            return NotFound();

        return cart;
    }

    // POST
    [HttpPost("/add")]
    public async Task<IActionResult> AddItem(int userId, [FromBody] CartItem item)
    {
        try
        {
            await _cartService.AddItemAsync(userId, item);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //DELETE
    [HttpDelete("/remove/{userId:int}/{productId:int}/{productStoreId:guid}")]
    public async Task<IActionResult> RemoveItem(int userId, int productId, Guid productStoreId)
    {
        try
        {
            await _cartService.RemoveItemAsync(userId, productId, productStoreId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("/clear/{userId:int}")]
    public async Task<IActionResult> ClearCart(int userId)
    {
        try
        {
            await _cartService.ClearCartAsync(userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}