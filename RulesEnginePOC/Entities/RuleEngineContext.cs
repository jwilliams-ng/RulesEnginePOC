using Microsoft.EntityFrameworkCore;

namespace RulesEnginePOC.Entities;

public class RuleEngineContext : DbContext
{
    public RuleEngineContext(DbContextOptions<RuleEngineContext> options) : base(options) { }

    public DbSet<Workflow> Workflows { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<ProviderRefundLogic> ProviderRefundLogics { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workflow>().HasMany(wf => wf.Rules).WithOne(r => r.Workflow).HasForeignKey(r => r.WorkflowId);
        modelBuilder.Entity<Workflow>().HasData(
            new Workflow
            {
                Id = 1,
                WorkflowName = "Discount"
            });
        modelBuilder.Entity<Rule>().HasData(
            new Rule()
            {
                Id = 1,
                WorkflowId = 1,
                RuleName = "GiveDiscount10",
                Expression = "storeInfo <= 2 AND totalOrders > 2 AND noOfVisitsPerMonth > 2"
            },
            new Rule()
            {
                Id = 2,
                WorkflowId = 1,
                RuleName = "GiveDiscount20",
                Expression = "storeInfo == 3 AND totalOrders > 2 AND noOfVisitsPerMonth > 2"
            });
        modelBuilder.Entity<ProviderRefundLogic>().HasData(
            new ProviderRefundLogic()
            {
                Id = 1,
                ProviderId = 1234,
                StartDate = DateTime.MinValue,
                RefundPercentage = 1,
                StartOffsetDays = 0,
                EndOffsetDays = 90,
            },new ProviderRefundLogic()
            {
                Id = 2,
                ProviderId = 1234,
                StartDate = DateTime.MinValue,
                StartOffsetDays = 91,
                EndOffsetDays = 365,
                Prorated = true
            },new ProviderRefundLogic()
            {
                Id = 3,
                ProviderId = 1234,
                StartDate = DateTime.MinValue,
                RefundPercentage = 0,
                StartOffsetDays = 366,
            });
    }
    
}