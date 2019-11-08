using System;
using System.Collections.Generic;
using System.Text;

namespace Source
{
    class Player
    {
        public long Id { get; set; }
        public long TeamId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int SkillLevel { get; set; }
        public Decimal Salary { get; set; }
    }
}
