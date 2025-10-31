namespace RulesEnginePOC.Models;

public class CancelledContract
{
    public DateTime BilledDate { get; set; }
    public DateTime CancelledDate { get; set; }
    public decimal ElapsedDays { get; set; }
    public decimal Amount { get; set; }
    public decimal ContractLength { get; set; }
}