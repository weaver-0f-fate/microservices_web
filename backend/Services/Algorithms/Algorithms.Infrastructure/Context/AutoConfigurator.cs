using Algorithms.Infrastructure.Configuration;
using Algorithms.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Algorithms.Infrastructure.Context;

public static class AutoConfigurator
{
    public static void MapSchemas(ModelBuilder modelBuilder, Dialect dialect, Type callingAssemblyType)
    {
        var confTypes = GetEntityTypeConfigurationImplementationTypes(callingAssemblyType);

        foreach (var type in confTypes)
        {
            var constructor = GetConstructor(type);
            ValidateConstructor(constructor, type);

            dynamic configurationInstance = constructor.Invoke(new object[] { dialect });
            modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }

    private static List<Type> GetEntityTypeConfigurationImplementationTypes(Type callingAssemblyType)
    {
        var infraAssembly = System.Reflection.Assembly.GetAssembly(callingAssemblyType);
        var configurationType = typeof(IEntityTypeConfiguration<>);

        var implementations = infraAssembly
            .GetTypes()
            .Where(x =>
                x.GetInterfaces()
                    .Any(y =>
                        y.IsGenericType
                        && y.GetGenericTypeDefinition() == configurationType))
            .ToList();

        return implementations;
    }

    private static ConstructorInfo GetConstructor(Type type)
    {
        var constructors = type.GetConstructors();

        if (constructors.Length != 1)
            throw new EntityConfigurationException(type, "has more than one CTOR");

        return constructors[0];
    }

    private static void ValidateConstructor(ConstructorInfo constructorsInfo, Type type)
    {
        var parameters = constructorsInfo.GetParameters();
        var constructorParamType = typeof(Dialect);

        if (parameters.Length != 1)
            throw new EntityConfigurationException(type, $"CTOR must contain single param of type {constructorParamType.FullName}");

        if (parameters[0].ParameterType != constructorParamType)
            throw new EntityConfigurationException(type, $"CTOR param must be of type {constructorParamType.FullName}");
    }
}