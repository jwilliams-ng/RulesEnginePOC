using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RulesEnginePOC.Entities;

[Table("Workflow", Schema = "rule")]
public partial class Workflow
{
    [Key]
    public int Id { get; set; }
    
    public string WorkflowName { get; set; }
    
    public virtual ICollection<Rule>? Rules { get; set; } = new List<Rule>();
}