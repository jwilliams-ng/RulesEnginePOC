using Microsoft.AspNetCore.Mvc;
using RulesEngine.Models;
using RulesEnginePOC.Models;
using RulesEnginePOC.Services.Interfaces;

namespace RulesEnginePOC.Controllers;

[Route("[controller]")]
[ApiController]
public class RulesEngineController(IRuleEngineService generalRuleService, IProviderRefundRulesService providerRefundRulesService, IProviderRefundService providerRefundService) : ControllerBase
{
    
    [HttpGet("process/{value1}/{value2}/{value3}")]
    public async Task<IActionResult> Index(int value1, int value2, int value3)
    {
        
        var rp1 = new RuleParameter("storeInfo", value1);
        var rp2 = new RuleParameter("totalOrders", value2);
        var rp3 = new RuleParameter("noOfVisitsPerMonth",value3);

        var result = await generalRuleService.GetRuleResults([rp1, rp2, rp3]);
        return Ok(result);
    }

    [HttpPost("provider-refund/{providerId}")]
    public async Task<IActionResult> ProviderRefund(int providerId, [FromBody] CancelledContract cancelledContract)
    {
        var rp1 = new RuleParameter("cancelledContract", cancelledContract);
        
        var result = await providerRefundRulesService.GetRuleResults(providerId, [rp1]);
        return Ok(result);
    }
    
    [HttpGet("provider-refund-rules/{providerId}")]
    public async Task<IActionResult> ProviderRefund(int providerId)
    {
        var result = await providerRefundRulesService.GetRulesForProvider(providerId);
        return Ok(result);
    }
    
    
    [HttpPost("generated-provider-refund/{providerId}")]
    public async Task<IActionResult> GeneratedProviderRefund(int providerId, [FromBody] CancelledContract cancelledContract)
    {
        var rp1 = new RuleParameter("cancelledContract", cancelledContract);
        
        var result = await providerRefundService.GetRuleResults(providerId, [rp1]);
        return Ok(result);
    }

}