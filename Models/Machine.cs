namespace Assignement;

public enum MachineStatus { Offline, Online }

public class Machine
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public MachineStatus Status { get; set; } = MachineStatus.Offline;

    public DateTimeOffset LastHeartbeat { get; set; }

    public string CurrentJob { get; set; } = string.Empty;

    public Metrics Metrics { get; set; } = new();

    public Dictionary<string, string> Metadata { get; set; } = new();
}
