using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game
{
    public static class Debug
    {
        public static void Assert(bool Test)
        {
#if DEBUG
            if (!Test)
            {
                throw new Exception();
            }
#endif
        }
    }
}
