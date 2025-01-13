# Acrux.Json

Provides components for the **Newtonsoft.Json** namespace for better and more flexible
serialization.

## EnumConverter

Serializes `Enum` by it's value name and assembly-qualified type name.<br/>
This approach makes the serializer able to recognize types of dynamically loaded assemblies.

### Usage

Creating an instance of the `EnumConverter` and attaching it to a **Newtonsoft.Json** `JsonSerializerSettings` is necessary:
```cs
JsonSerializerSettings jsonSettings = new JsonSerializerSettings
{
    Converters = [new Acrux.Json.EnumConverter()],
};
```

> [!Caution]
> The **Newtonsoft.Json** has a problem serializing data whose datatype is `System.Enum` rather than a specific `enum` type.
> Is necessary to attach a `JsonConverter` attribute.
> ```cs
> [JsonConverter(typeof(Acrux.Json.EnumConverter))]
> public Enum? MyProperty { get; set; }
> ```