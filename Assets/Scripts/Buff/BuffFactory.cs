using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class BuffFactory
{
    static readonly Dictionary<string, Type> typeMap;

    static BuffFactory()
    {
        typeMap = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && typeof(BuffBase).IsAssignableFrom(t))
            .Select(t => new {
                Type = t,
                Attr = t.GetCustomAttribute<BuffAttribute>()
            })
            .Where(x => x.Attr != null)
            .ToDictionary(x => x.Attr.Id, x => x.Type);
    }

    public static BuffBase Create(string id, BuffData data, CharacterBase from, CharacterBase target)
    {
        if (!typeMap.TryGetValue(id, out var type))
            throw new ArgumentException($"Unknown buff id: {id}");

        return (BuffBase)Activator.CreateInstance(type, data, from, target);
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class BuffAttribute : Attribute
{
    public string Id { get; }
    public BuffAttribute(string id) => Id = id;
}
