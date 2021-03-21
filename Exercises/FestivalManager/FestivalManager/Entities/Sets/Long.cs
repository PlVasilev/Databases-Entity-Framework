using System;
using System.Collections.Generic;
using System.Text;

namespace FestivalManager.Entities.Sets
{
    class Long : Set
    {
        public Long(string name) : base(name, new TimeSpan(1,0,0))
        {
        }
    }
}
