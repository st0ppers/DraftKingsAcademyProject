using System.Threading.Tasks.Dataflow;

namespace KafkaServices.Dataflow
{
    public class Dataflow<Tvalue> 
    {
        private readonly TransformBlock<Tvalue, string> TransformBlock;

        public Dataflow()
        {
            TransformBlock = new TransformBlock<Tvalue, string>(request => $"Ordered {typeof(Tvalue).Name}");
            var actionBlock = new ActionBlock<string>(Console.WriteLine);
            TransformBlock.LinkTo(actionBlock);
        }

        public async Task Post(Tvalue value)
        {
            await TransformBlock.SendAsync(value);
        }
    }
}
