using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wdhrtosis.Models
{
    public class ProcessSummary
    {
        public int Id { get; set; }
        public DateTime LastRun { get; set; }
        public string ObjectProcessed { get; set; }
        public int ObjectCount { get; set; }
        public string Description { get; set; }

    }
}
