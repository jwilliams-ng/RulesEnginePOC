using Microsoft.AspNetCore.Mvc;
using RulesEngine.Models;
using RulesEnginePOC.Services.Interfaces;

namespace RulesEnginePOC.Controllers;

[Route("[controller]")]
[ApiController]
public class RulesEngineController(IRuleEngineService service) : ControllerBase
{
    
    [HttpGet("process/{value1}/{value2}/{value3}")]
    public async Task<IActionResult> Index(int value1, int value2, int value3)
    {
        
        var rp1 = new RuleParameter("storeInfo", value1);
        var rp2 = new RuleParameter("totalOrders", value2);
        var rp3 = new RuleParameter("noOfVisitsPerMonth",value3);

        var result = await service.GetRuleResults([rp1, rp2, rp3]);
        return Ok(result);
    }
}