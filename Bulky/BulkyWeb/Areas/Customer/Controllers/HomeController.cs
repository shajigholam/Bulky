using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers;
[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
        return View(productList);
    }
    // use the same para name as asp-route-productId
    public IActionResult Details(int productId)
    {
        //Product product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category");
        ShoppingCart cart = new()
        {
            Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
            Count = 1,
            ProductId = productId
        };
        return View(cart);
    }
    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        // access the id of the logged in user
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;

        // add shopping cart
        _unitOfWork.ShoppingCart.Add(shoppingCart);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

