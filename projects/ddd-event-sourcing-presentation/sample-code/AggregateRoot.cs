public class AggregateRoot
{
  private readonly List<DomainEvent> events = new List<DomainEvent>();

  protected void ApplyChange(DomainEvent evt)
  {
    this.events.Add(evt);

    // Reflection magic
    this.Apply(evt);
  }

  public void LoadFromHistory(IEnumerable<DomainEvent> events)
  {
    events.ForEach(x => this.ApplyChange(x));
  }
}
