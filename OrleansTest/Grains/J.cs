using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Orleans.CodeGeneration;
using Orleans.Concurrency;
using Orleans.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OrleansTest.Grains
{
    [Immutable]
    public struct J<T>
    {
        public T Value { get; }

        public J(T value)
        {
            Value = value;
        }

        public static implicit operator T(J<T> value)
        {
            return value.Value;
        }

        public static implicit operator J<T>(T d)
        {
            return new J<T>(d);
        }

        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }

        public static Task<J<T>> AsTask(T value)
        {
            return Task.FromResult<J<T>>(value);
        }

        [CopierMethod]
        public static object Copy(object input, ICopyContext context)
        {
            return input;
        }

        [SerializerMethod]
        public static void Serialize(object input, ISerializationContext context, Type expected)
        {
            var jsonSerializer = GetSerializer(context);

            var stream = new StreamWriterWrapper(context.StreamWriter);

            using (var textWriter = new StreamWriter(stream))
            {
                using (var jsonWriter = new JsonTextWriter(textWriter))
                {
                    jsonSerializer.Serialize(jsonWriter, input);
                }
            }
        }

        [DeserializerMethod]
        public static object Deserialize(Type expected, IDeserializationContext context)
        {
            var jsonSerializer = GetSerializer(context);

            var stream = new StreamReaderWrapper(context.StreamReader);

            using (var textReader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    return jsonSerializer.Deserialize(jsonReader, expected);
                }
            }
        }

        private static JsonSerializer GetSerializer(ISerializerContext context)
        {
            try
            {
                return context?.ServiceProvider?.GetRequiredService<JsonSerializer>();
            }
            catch
            {
                return JsonSerializer.Create();
            }
        }
    }
}
