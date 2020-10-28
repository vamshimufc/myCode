using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wdhrtosis.Models
{
    public class History
    {
        public int Id { get; set; }
        public string CorrelationId { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public DateTimeOffset LastRun { get; set; }

    }
}
