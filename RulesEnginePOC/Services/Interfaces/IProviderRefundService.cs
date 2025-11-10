using RulesEngine.Models;
using RulesEnginePOC.Models;

namespace RulesEnginePOC.Services.Interfaces;

public interface IProviderRefundService
{
    public Task<ProviderRefundReturn> GetRuleResults(int providerId, RuleParameter[] rules);
}