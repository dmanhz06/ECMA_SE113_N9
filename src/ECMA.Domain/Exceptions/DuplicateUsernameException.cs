namespace ECMA.Domain.Exceptions;

public sealed class DuplicateUsernameException : Exception
{
    public DuplicateUsernameException(Exception innerException) : base("Username already exists.", innerException) { }
}
