using RulesEngine.Models;

namespace RulesEnginePOC.Models;

public record ProviderRefundReturn(decimal returnAmount, List<RuleResultTree> resultList);