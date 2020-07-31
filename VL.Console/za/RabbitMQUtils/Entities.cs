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
    public class NamedMessage2
    {
        public NamedMessage2(long id)
        {
            Id = id;
        }

        public long Id { set; get; }
    }
    public class NamedMessage2Sub1
    {
        public NamedMessage2Sub1(long id)
        {
            Id = id;
        }

        public long Id { set; get; }
    }
    public class NamedMessage2Sub2
    {
        public NamedMessage2Sub2(long id)
        {
            Id = id;
        }

        public long Id { set; get; }
    }
    public class NamedMessageDirect1
    {
        public NamedMessageDirect1(long id)
        {
            Id = id;
        }

        public long Id { set; get; }
    }
    public class NamedMessage4
    {
        public NamedMessage4(long id)
        {
            Id = id;
        }

        public long Id { set; get; }
    }
}
