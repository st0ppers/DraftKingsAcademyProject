using Confluent.Kafka;
using MessagePack;

namespace KafkaServices.Serializers
{
    public class MsgPackDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}
