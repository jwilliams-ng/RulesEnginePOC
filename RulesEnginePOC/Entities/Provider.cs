using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RulesEnginePOC.Entities;

[Table("Provider", Schema = "rule")]
public class Provider
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    
    public virtual ICollection<ProviderRule> ProviderRules { get; set; }
}