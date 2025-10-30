
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RulesEnginePOC.Entities;

[Table("Rule", Schema = "rule")]
[Index("WorkflowId", Name = "fk_rule_Rule_Workflow")]
public partial class Rule
{
    [Key]
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public string RuleName { get; set; }

    public string? Operator { get; set; }
    public string? Expression { get; set; }

    public string? ErrorMessage { get; set; }
    
    [ForeignKey("WorkflowId")]
    public virtual Workflow Workflow { get; set; }
}