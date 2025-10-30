using RulesEngine.Models;

namespace RulesEnginePOC.Services.Interfaces;

public interface IRuleEngineService
{
    public Task<List<RuleResultTree>> GetRuleResults(RuleParameter[] rules);
}