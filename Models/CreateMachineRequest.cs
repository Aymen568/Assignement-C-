using System.ComponentModel.DataAnnotations;

namespace Assignement;

public record CreateMachineRequest(
    [Required]
    [MinLength(1)]
    string Name,
    Dictionary<string, string>? Metadata);
