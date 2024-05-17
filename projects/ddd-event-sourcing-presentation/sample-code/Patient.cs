public class Patient : AggregateRoot
{
  public Id BedId { get; private set; }

  public void AssignToBed(Id bedId)
  {
    if (this.BedId != null) { throw new Exception("Already assigned to bed"); }

    this.ApplyChange(new PatientAssignedToBed(this.Id, this.BedId));
  }

  public void Apply(PatientAssignedToBed evt)
  {
    this.BedId = evt.Id;
  }
}
