using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RulesEngine.Models;
using RulesEnginePOC.Entities;
using RulesEnginePOC.Models;
using RulesEnginePOC.Services.Interfaces;
using Rule = RulesEngine.Models.Rule;
using Workflow = RulesEngine.Models.Workflow;

namespace RulesEnginePOC.Services;

public class ProviderRefundRulesService : IProviderRefundRulesService
{
    private RuleEngineContext _context;
    
    public ProviderRefundRulesService(RuleEngineContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }
    
    public async Task<List<string>> GetRulesForProvider(int providerId)
    {
        var dbRefundLogic = await GetProviderRefundLogicsAsync(providerId);
        var convertedRules = dbRefundLogic.Select(ConvertRowToRule).ToArray();

        return convertedRules.Select(rule => JsonSerializer.Serialize(rule)).ToList();
    }

    public async Task<ProviderRefundReturn> GetRuleResults(int providerId, RuleParameter[] rules)
    {
        string domain = "provider-" + providerId;
        var dbRefundLogic = await GetProviderRefundLogicsAsync(providerId);
        
        var convertedRules = dbRefundLogic.Select(ConvertRowToRule).ToArray();
        var workflowRules = new Workflow[]
        {
            new()
            {
                WorkflowName = domain,
                Rules = convertedRules,
            }
        };

        var rulesEngine = new RulesEngine.RulesEngine(workflowRules);
        
        var resultList  = await rulesEngine.ExecuteAllRulesAsync(domain, rules);
        resultList = resultList.Where(rl => rl.IsSuccess).ToList();
        
        foreach(var result in resultList){
            Console.WriteLine($"Rule - {result.Rule.RuleName}, IsSuccess - {result.IsSuccess}, Output - {result.ActionResult.Output}");
        }

        List<object> resultOutputs = resultList.Select(rl => rl.ActionResult.Output).ToList();
        List<decimal> resultAmounts = resultOutputs.Select(ro => Math.Round((decimal)ro, 2)).ToList();
        decimal resultAmount = resultAmounts.Min();
        
        return new ProviderRefundReturn(resultAmount, resultList);
    }

    private async Task<List<ProviderRefundLogic>> GetProviderRefundLogicsAsync(int providerId)
    {
        return await _context.ProviderRefundLogics.Where(prl => prl.ProviderId == providerId).ToListAsync();
    }

    private Rule ConvertRowToRule(ProviderRefundLogic row)
    {
        if (row == null)
        {
            throw new NullReferenceException("ProviderRefundLogic row passed to ConvertRowToRule is null");
        }
        
        var ruleName = row.RefundPercentage == 1 ? "Full Refund" :
            row.Prorated ? "Prorated" :
            row.RefundPercentage == 0 ? "No Refund" :
            row.RefundPercentage * 100 + "% refund";
        var expression = row.EndOffsetDays != null
            ? $"cancelledContract.ElapsedDays < {row.EndOffsetDays} && cancelledContract.ElapsedDays >= {row.StartOffsetDays}" :
            $"cancelledContract.ElapsedDays >= {row.StartOffsetDays}";
        var formula = row.RefundPercentage == 1 ? "cancelledContract.Amount" :
            row.Prorated ? $"cancelledContract.Amount * (1 - (cancelledContract.ElapsedDays / cancelledContract.ContractLength))" :
            row.RefundPercentage == 0 ? "0m" :
            $"cancelledContract.Amount * {row.RefundPercentage}";
        var context = new Dictionary<string, object>
        {
            {"Expression", formula}
        };

        return new Rule
        {
            RuleName = ruleName,
            Expression = expression,
            Actions =
            new RuleActions
            {
                OnSuccess =
                new ActionInfo
                {
                    Name = "OutputExpression",
                    Context = context
                }
            }
        };
    }
}