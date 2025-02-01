using Microsoft.AspNetCore.Mvc;
using Travel_Planner.Data;
using Travel_Planner.Models;
using System.Linq;

public class DestinationController : Controller
{
    private readonly ApplicationDbContext _context;

    public DestinationController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var destinations = _context.Destinations.ToList();
        return View(destinations); //returns to destination view
    }
}
