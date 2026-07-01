namespace T2MTemplate.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
}
