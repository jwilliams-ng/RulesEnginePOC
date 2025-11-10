using Microsoft.EntityFrameworkCore;
using RulesEngine.Models;
using RulesEnginePOC.Entities;
using RulesEnginePOC.Models;
using RulesEnginePOC.Services.Interfaces;
using RERule = RulesEngine.Models.Rule;
using REWorkflow = RulesEngine.Models.Workflow;

namespace RulesEnginePOC.Services;

public class ProviderRefundService : IProviderRefundService
{
    private readonly RuleEngineContext _context;
    private readonly IConvertDataToExpressionService _convertDataToExpressionService;
    
    public ProviderRefundService(RuleEngineContext context, IConvertDataToExpressionService convertDataToExpressionService)
    {
        _convertDataToExpressionService = convertDataToExpressionService;
        _context = context;
        _context.Database.EnsureCreated();
    }
    
    public async Task<ProviderRefundReturn> GetRuleResults(int providerId, RuleParameter[] rules)
    {
        var provider = await _context.Providers.FirstOrDefaultAsync(p => p.Id == providerId);
        if (provider == null)
        {
            throw new Exception($"Provider {providerId} not found");
        }

        var providerRules = await _context.ProviderRules.Where(p => p.ProviderId == providerId)
            .Include(pr => pr.Provider)
            .Include(pr => pr.RuleExpression)
            .Include(pr => pr.RuleOutput).ToListAsync();
        
        
        var reRules = providerRules.Select(pr => new RERule
        {
            RuleName = pr.Provider?.Name + "-" + pr.Name,
            Expression = _convertDataToExpressionService.ConvertProviderRefundExpression(pr),
            Actions = new RuleActions
            {
                OnSuccess =
                    new ActionInfo
                    {
                        Name = "OutputExpression",
                        Context = new Dictionary<string, object> { {"Expression", _convertDataToExpressionService.ConvertProviderRefundFormula(pr) } }
                    }
            }
        }).ToList();

        var reWorkflowRules = new REWorkflow[]
        {
            new()
            {
                WorkflowName = provider.Name,
                Rules = reRules
            }
        };
        
        var rulesEngine = new RulesEngine.RulesEngine(reWorkflowRules);
        
        var resultList  = await rulesEngine.ExecuteAllRulesAsync(provider.Name, rules);
        if (resultList == null)
        {
            throw new Exception($"Rules engine returned null for {provider.Name}");
        }
        
        resultList = resultList.Where(rl => rl.IsSuccess).ToList();

        if (resultList.Count == 0)
        {
            return new ProviderRefundReturn(0, []);
        }
        
        foreach(var result in resultList){
            Console.WriteLine($"Rule - {result.Rule.RuleName}, IsSuccess - {result.IsSuccess}, Output - {result.ActionResult.Output}");
        }

        var resultOutputs = resultList.Select(rl => rl.ActionResult.Output).ToList();
        var resultAmounts = resultOutputs.Select(ro => Math.Round((decimal)ro, 2)).ToList();
        var resultAmount = resultAmounts.Min();
        
        return new ProviderRefundReturn(resultAmount, resultList);
    }
}