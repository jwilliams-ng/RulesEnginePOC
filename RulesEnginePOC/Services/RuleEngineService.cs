using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RulesEngine.Models;
using RulesEnginePOC.Entities;
using RulesEnginePOC.Services.Interfaces;
using DBWorkflow = RulesEnginePOC.Entities.Workflow;
using REWorkflow = RulesEngine.Models.Workflow;
using RERule = RulesEngine.Models.Rule;

namespace RulesEnginePOC.Services;

public class RuleEngineService : IRuleEngineService
{
    private RulesEngine.RulesEngine _rulesEngine;
    private RuleEngineContext _ruleEngineContext;
    private const string DOMAIN = "Discount";

    public RuleEngineService(RuleEngineContext context)
    {
        _ruleEngineContext = context;
        _ruleEngineContext.Database.EnsureCreated();

        DBWorkflow[] dbWorkflowRules = _ruleEngineContext.Workflows.Where(wf => wf.WorkflowName == DOMAIN).Include(wf => wf.Rules).ToArray();
        if (dbWorkflowRules.Length < 1)
        {
            throw new Exception("No workflow found");
        }
        
        REWorkflow[] workflowRules = dbWorkflowRules.Select(wf => new REWorkflow()
        {
            WorkflowName = wf.WorkflowName,
            Rules = wf.Rules.Select(r => new RERule()
            {
                Expression = r.Expression,
                RuleName = r.RuleName,
            }).ToArray()
        }).ToArray();
        
        _rulesEngine = new RulesEngine.RulesEngine(workflowRules);
    }

    public async Task<List<RuleResultTree>> GetRuleResults(RuleParameter[] rules)
    {
        var resultList  = await _rulesEngine.ExecuteAllRulesAsync(DOMAIN, rules);
        
        foreach(var result in resultList){
            Console.WriteLine($"Rule - {result.Rule.RuleName}, IsSuccess - {result.IsSuccess}");
        }
        
        return resultList;
    }
}