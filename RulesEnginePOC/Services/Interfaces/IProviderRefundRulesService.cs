using RulesEngine.Models;
using RulesEnginePOC.Models;

namespace RulesEnginePOC.Services.Interfaces;

public interface IProviderRefundRulesService
{
    public Task<List<string>> GetRulesForProvider(int providerId);
    public Task<ProviderRefundReturn> GetRuleResults(int providerId, RuleParameter[] rules);
}