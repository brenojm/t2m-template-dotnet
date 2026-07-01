namespace T2MTemplate.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
