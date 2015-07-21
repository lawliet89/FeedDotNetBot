using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedDotNetBot.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract IEnumerable<Argument> Arguments { get; }

        public Argument GetArgument(string name)
        {
            return Arguments.Single(a => a.Name.Equals(name));
        }

        public Argument GetArgument(int index)
        {
            return Arguments.Skip(Math.Max(0, index - 1)).First();
        }

    }
}
