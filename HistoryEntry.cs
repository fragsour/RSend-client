using System;

namespace Client
{
    [Serializable]
    public class HistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }
        public bool IsIncoming { get; set; } // true – входящее, false – исходящее
    }
}