using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedDotNetBot.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";
        public override IEnumerable<Argument> Arguments => Enumerable.Empty<Argument>();
    }
}
