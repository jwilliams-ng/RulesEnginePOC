using Microsoft.EntityFrameworkCore;

namespace RulesEnginePOC.Entities;

public class RuleEngineContext : DbContext
{
    public RuleEngineContext(DbContextOptions<RuleEngineContext> options) : base(options) { }

    public DbSet<Workflow> Workflows { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<ProviderRefundLogic> ProviderRefundLogics { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<ProviderRule> ProviderRules { get; set; }
    public DbSet<RuleExpression> RuleExpressions { get; set; }
    public DbSet<RuleOutput> RuleOutputs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workflow>().HasMany(wf => wf.Rules).WithOne(r => r.Workflow).HasForeignKey(r => r.WorkflowId);
        modelBuilder.Entity<ProviderRule>().HasOne(pr => pr.Provider).WithMany(p => p.ProviderRules).HasForeignKey(pr => pr.ProviderId);
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
        modelBuilder.Entity<Provider>().HasData(
            new Provider()
            {
                Id = 1234,
                Name = "Test Provider 1"
            },
            new Provider()
            {
                Id = 5678,
                Name = "Test Provider 2"
            });
        modelBuilder.Entity<RuleExpression>().HasData(
            new RuleExpression()
            {
                Id = 1,
                Formula = "cancelledContract.ElapsedDays > {0} && cancelledContract.ElapsedDays <= {1}"
            },
            new RuleExpression()
            {
                Id = 2,
                Formula = "cancelledContract.ElapsedDays >= {0}"
            });
        modelBuilder.Entity<RuleOutput>().HasData(
            new RuleOutput()
            {
                Id = 1,
                Formula = "cancelledContract.Amount"
            },
            new RuleOutput()
            {
                Id = 2,
                Formula =
                    "cancelledContract.Amount * (1 - (cancelledContract.ElapsedDays / cancelledContract.ContractLength))"
            },
            new RuleOutput()
            {
                Id = 3,
                Formula = "0m"
            },
            new RuleOutput()
            {
                Id = 4,
                Formula = "cancelledContract.Amount * {0}"
            });
        modelBuilder.Entity<ProviderRule>().HasData(
            new ProviderRule()
            {
                Id = 1,
                Name = "Full Refund",
                ProviderId = 1234,
                RuleExpressionId = 1,
                RuleOutputId = 1,
                StartDate = DateTime.MinValue,
                RefundPercentage = 1,
                StartOffsetDays = 0,
                EndOffsetDays = 90,
            },
            new ProviderRule()
            {
                Id = 2,
                Name = "Prorated",
                ProviderId = 1234,
                RuleExpressionId = 1,
                RuleOutputId = 2,
                StartDate = DateTime.MinValue,
                StartOffsetDays = 91,
                EndOffsetDays = 365,
                Prorated = true
            },
            new ProviderRule()
            {
                Id = 3,
                Name = "No Refund",
                ProviderId = 1234,
                RuleExpressionId = 2,
                RuleOutputId = 3,
                StartDate = DateTime.MinValue,
                RefundPercentage = 0,
                StartOffsetDays = 366,
            },
            new ProviderRule()
            {
                Id = 4,
                Name = "Full Refund",
                ProviderId = 5678,
                RuleExpressionId = 1,
                RuleOutputId = 1,
                StartDate = DateTime.MinValue,
                RefundPercentage = 1,
                StartOffsetDays = 0,
                EndOffsetDays = 90,
            },
            new ProviderRule()
            {
                Id = 5,
                Name = "No Refund",
                ProviderId = 5678,
                RuleExpressionId = 2,
                RuleOutputId = 3,
                StartDate = DateTime.MinValue,
                RefundPercentage = 0,
                StartOffsetDays = 91,
            });
    }
    
}