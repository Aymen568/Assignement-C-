namespace Assignement;

public class MachineNotFoundException : Exception
{
    public MachineNotFoundException(Guid machineId)
        : base($"Machine '{machineId}' was not found.")
    {
        MachineId = machineId;
    }

    public Guid MachineId { get; }
}
