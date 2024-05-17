public interface IRepository
{
  Task<T> LoadAsync<T>(AggregateId id) where T : AggregateRoot;

  Task<T> FindAsync<T>(AggregateId id) where T : AggregateRoot;

  Task SaveAsync<T>(T aggregate) where T : AggregateRoot;
}
