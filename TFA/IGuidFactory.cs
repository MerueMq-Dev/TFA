namespace TFA.Domain;

public interface IGuidFactory
{
    public Guid Create();
}

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}