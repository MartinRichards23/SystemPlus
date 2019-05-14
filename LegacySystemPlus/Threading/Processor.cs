using System;
using System.Threading;

namespace SystemPlus.Threading
{
    public sealed class Processor<T> : ProcessorBase<T>
    {
        readonly Action<T> process;

        public Processor(int maxThreads, CancellationToken cancelToken, Action<T> process)
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