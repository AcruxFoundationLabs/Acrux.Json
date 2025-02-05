using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
namespace Acrux.Json;

/// <summary>
/// Serializes an <see cref="Enum"/> by it's value name and assembly-qualified type name.
/// </summary>
public class EnumConverter : StringEnumConverter
{
	public override bool CanConvert(Type objectType)
	{
		Type type = Nullable.GetUnderlyingType(objectType) ?? objectType;
		return type.IsEnum;
	}

	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
			return null;

		if (reader.TokenType != JsonToken.String)
			throw new JsonSerializationException($"Unexpected token parsing enum. Expected String, got {reader.TokenType}.");

		string serialized = reader.Value?.ToString() ?? throw new JsonSerializationException("Enum value is null or invalid.");

		string[] parts = serialized.Split('@');
		if (parts.Length != 2)
			throw new JsonSerializationException("Enum serialization format is invalid.");

		string partType = parts[0];
		string partValue = parts[1];

		Type enumType = Type.GetType(partType) ?? throw new JsonSerializationException($"The type '{partType}' for the deserializing enum was not found.");
		return Enum.Parse(enumType, partValue);
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		if (value == null)
		{
			writer.WriteNull();
			return;
		}

		if (value is Enum enumValue)
		{
			writer.WriteValue(GetSerializedValue(enumValue));
		}
		else
		{
			throw new JsonSerializationException("Expected an enum value.");
		}
	}

	public string GetSerializedValue(Enum value)
	{
		Type enumType = value.GetType();
		string partType = enumType.AssemblyQualifiedName ?? throw new JsonSerializationException("Enum type's assembly-qualified name is null.");
		string partValue = value.ToString();
		return $"{partType}@{partValue}";
	}
}
