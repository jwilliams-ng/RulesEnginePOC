using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEnginePOC.Entities;

[Table("ProviderRule", Schema = "rule")]
public class ProviderRule
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProviderId { get; set; }
    
    [ForeignKey("ProviderId")]
    public virtual Provider? Provider { get; set; }
    public int RuleExpressionId { get; set; }
    
    [ForeignKey("RuleExpressionId")]
    public virtual RuleExpression? RuleExpression { get; set; }
    public int RuleOutputId { get; set; }
    
    [ForeignKey("RuleOutputId")]
    public virtual RuleOutput? RuleOutput { get; set; }
    public int? StartOffsetDays { get; set; }
    public int? EndOffsetDays { get; set; }
    public decimal? RefundPercentage { get; set; }
    public decimal? CancelFee { get; set; }
    public bool Prorated { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}