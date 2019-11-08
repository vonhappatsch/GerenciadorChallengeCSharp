using System;
using System.Collections.Generic;
using System.Text;

namespace Source
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string MainShirtColor { get; set; }
        public string SecondaryShirtColor { get; set; }
        public long? CaptainId { get; set; }
        public long TeamId { get; internal set; }
    }
}
