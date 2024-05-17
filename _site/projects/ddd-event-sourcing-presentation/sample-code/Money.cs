public struct Money
{
  public readonly decimal Value;
  public readonly Currency Currency;

  // Obvious constructor omitted

  public Money Add(Money other)
  {
    if (this.Currency != this.Currency)
    {
      throw new InvalidOperationException("Please convert between currencies first");
    }

    return new Money(this.Value + other.Value, this.Currency);
  }
}
