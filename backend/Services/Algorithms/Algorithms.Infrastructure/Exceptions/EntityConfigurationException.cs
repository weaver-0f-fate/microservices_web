using System.Runtime.Serialization;

namespace Algorithms.Infrastructure.Exceptions;

[Serializable]
public class EntityConfigurationException : Exception
{
    public EntityConfigurationException(Type type, string message) : base(AddTypeToMessage(type, message))
    {
    }

    private static string AddTypeToMessage(Type type, string message)
    {
        return $"Type: {type.Name} " + message;
    }

    public EntityConfigurationException()
    {
    }

    public EntityConfigurationException(string message) : base(message)
    {
    }

    public EntityConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected EntityConfigurationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}