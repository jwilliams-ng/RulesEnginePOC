using System.Linq.Expressions;
using RulesEnginePOC.Entities;

namespace RulesEnginePOC.Services.Interfaces;

public interface IConvertDataToExpressionService
{
    public string ConvertProviderRefundExpression(ProviderRule pr);
    public string ConvertProviderRefundFormula(ProviderRule pr);
}