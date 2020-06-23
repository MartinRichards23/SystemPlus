using System;
using System.Collections.Generic;

namespace SystemPlus.Web.W3C
{
    [Serializable]
    public class W3CResult
    {
        public List<Message>? messages { get; set; }
    }

    [Serializable]
    public class Message
    {
        public string? type { get; set; }
        public int lastLine { get; set; }
        public int lastColumn { get; set; }
        public int firstColumn { get; set; }
        public string? message { get; set; }
        public string? extract { get; set; }
        public int hiliteStart { get; set; }
        public int hiliteLength { get; set; }
    }
}
