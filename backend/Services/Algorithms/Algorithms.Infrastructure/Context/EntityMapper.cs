using Algorithms.Domain.Core.Interfaces;
using Algorithms.Domain.Core;
using Algorithms.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Infrastructure.Context;

public class EntityMapper<T> where T : Entity
{
    private readonly Func<string, string> Namer;
    private readonly string Schema;
    private readonly EntityTypeBuilder<T> Builder;

    public EntityMapper(EntityTypeBuilder<T> builder, Dialect dialect)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        Builder = builder;
        Namer = GetDialectNamingConventionConverter(dialect);
        Schema = Namer(GetSchemaFromStack());

        MapUuid();

        if (!IsDerivedType())
        {
            builder.HasKey(x => x.Id);
            MapSoftDeleteQueryFilter();
        }

        builder.Ignore(b => b.DomainEvents);
    }

    public EntityTypeBuilder<T> GetBuilder() => Builder;

    private void MapSoftDeleteQueryFilter()
    {
        if (typeof(T).GetInterfaces().Contains(typeof(IDeleteable)))
        {
            Builder.HasQueryFilter(x => EF.Property<bool>(x, nameof(IDeleteable.IsActive)));
        }
    }

    public void Navigation(string propertyName)
    {
        var navigation = Builder.Metadata.FindNavigation(propertyName);

        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    public void OptionalParent(string propertyName)
    {
        var fieldName = Namer(propertyName + "Id");

        Builder
            .HasOne(propertyName)
            .WithMany()
            .HasForeignKey(fieldName)
            .IsRequired(false);
    }

    /// <summary>
    /// Seadista veeru nimi automaatselt välja nimest.
    /// </summary>
    /// <typeparam name="TColumnType"></typeparam>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public PropertyBuilder ToColumn<TColumnType>(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentException("Argument is null or empty", nameof(fieldName));

        return ToColumn<TColumnType>(fieldName, Namer(CleanName(fieldName)));
    }

    public PropertyBuilder ToColumn<TColumnType>(string fieldName, string columnName)
    {
        return Builder
            .Property<TColumnType>(fieldName)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName(columnName);
    }

    /// <summary>
    /// Seadista tabeli nimi automaatselt tüübi nimest.
    /// </summary>
    public void ToTable()
    {
        ToTable(Namer(typeof(T).Name));
    }

    /// <summary>
    /// Seadista tabeli nimi ette antud nimele.
    /// </summary>
    public void ToTable(string tableName)
    {
        Builder.ToTable(tableName, Schema);
    }

    private void MapUuid()
    {
        if (typeof(T).GetProperties(BindingFlags.Instance).Any(x => x.Name == "Uuid"))
        {
            ToColumn<Guid>("Uuid", "uuid");
        }
    }

    private static string GetSchemaFromStack()
    {
        var ns = new StackTrace().GetFrame(2).GetMethod().ReflectedType.Namespace.Split('.');

        return ns[ns.Length - 1];
    }

    private static string CleanName(string name)
    {
        return name.Trim('_');
    }

    private static string GetFieldName(string name)
    {
        return "_" + char.ToLower(name[0]) + name.Substring(1) + "Id";
    }

    private bool IsDerivedType()
    {
        return
            typeof(T).BaseType != typeof(Entity) &&
            typeof(T).BaseType != typeof(EntityWithUuid) &&
            typeof(T).BaseType != typeof(TrackedEntity) &&
            typeof(T).BaseType != typeof(AggregateRoot) &&
            typeof(T).BaseType != typeof(TrackedAggregateRoot);
    }

    /// <summary>
    /// Tagastab nimetamismeetoodi vastavalt dialektile. 
    /// </summary>
    /// <param name="dialect"></param>
    /// <returns></returns>
    private static Func<string, string> GetDialectNamingConventionConverter(Dialect dialect)
    {
        switch (dialect)
        {
            case Dialect.Postgres:
                return PostgresNamingConvention.ConvertName;
            default:
                throw new NotImplementedException($"No converter for dialect: {dialect}");
        }
    }

}