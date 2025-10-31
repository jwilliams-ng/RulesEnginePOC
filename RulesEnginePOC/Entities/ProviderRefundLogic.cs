using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEnginePOC.Entities;

[Table("ProviderRefundLogic", Schema = "rule")]
public class ProviderRefundLogic
{
    [Key]
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public int? StartOffsetDays { get; set; }
    public int? EndOffsetDays { get; set; }
    public decimal? RefundPercentage { get; set; }
    public decimal? CancelFee { get; set; }
    public bool Prorated { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}