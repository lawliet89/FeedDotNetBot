using System;

namespace FeedDotNetBot.Commands
{
    public class Argument
    {
        public string Name { get; protected set; }
        public bool Optional { get; protected set; }
        public Type Type { get; protected set; }

        public Argument(string name) : this(name, true)
        {
        }

        public Argument(string name, bool optinoal) : this(name, optinoal, typeof (string))
        {
        }

        public Argument(string name, bool optional, Type type)
        {
            Name = name;
            Optional = optional;
            Type = type;
        }
    }
}
