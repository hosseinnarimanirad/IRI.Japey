using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
    public static class EventHandlerExtensions
    {
        public static void SafeInvoke(this EventHandler handler, object sender)
        {
            if (handler != null) handler(sender, EventArgs.Empty);
        }

        public static void SafeInvoke<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            if (handler != null) handler(sender, args);
        }
    }
}
