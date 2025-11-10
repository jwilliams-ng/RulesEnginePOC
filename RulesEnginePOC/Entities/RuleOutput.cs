using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEnginePOC.Entities;

[Table("RuleOutput", Schema = "rule")]
public class RuleOutput
{
    [Key]
    public int Id { get; set; }
    public string Formula { get; set; }
    
    public virtual ICollection<ProviderRule> ProviderRules { get; set; }
}