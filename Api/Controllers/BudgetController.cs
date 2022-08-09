using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly FeesCollection _fees;
    public BudgetController(FeesCollection fees) => _fees = fees;

    [HttpGet("Reverse/{total}")]
    public IActionResult Get(double total) => Ok(Math.Round(_fees.Reverse(total),2));
}