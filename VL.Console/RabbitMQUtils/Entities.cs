using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consoling.RabitMQUtils
{
    public class NamedMessage1
    {
        public NamedMessage1(long id)
        {
            Id = id;
        }

        public long Id { set; get; }
    }
}
