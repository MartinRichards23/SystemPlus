using System.Threading;

namespace SystemPlus.Threading
{
    public sealed class Processor<T> : ProcessorBase<T>
    {
        readonly Action<T> process;

        public Processor(int maxThreads, Action<T> process, CancellationToken cancelToken)
            : base(maxThreads, cancelToken)
        {
            this.process = process;
        }

        protected override void ProcessItem(T item)
        {
            process(item);
        }
    }
}