using System.Linq.Expressions;
using RulesEnginePOC.Entities;
using RulesEnginePOC.Models;
using RulesEnginePOC.Services.Interfaces;

namespace RulesEnginePOC.Services;

public class ConvertDataToExpressionService(RuleEngineContext context) : IConvertDataToExpressionService
{
    public string ConvertProviderRefundExpression(ProviderRule pr)
    {
        if (pr.RuleExpression == null)
        {
            throw new Exception($"Rule Expression is null for Provider: {pr.ProviderId} on ProviderRule: {pr.Id}");
        }
        return String.Format(pr.RuleExpression.Formula, pr.StartOffsetDays, pr.EndOffsetDays);
    }

    public string ConvertProviderRefundFormula(ProviderRule pr)
    {
        if (pr.RuleOutput == null)
        {
            throw new Exception($"Rule Output is null for Provider: {pr.ProviderId} on ProviderRule: {pr.Id}");
        }
        return String.Format(pr.RuleOutput.Formula, pr.RefundPercentage);
    }
}