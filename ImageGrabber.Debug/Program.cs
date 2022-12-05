using System;
using System.Drawing;
using System.IO;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageGrabber.Debug
{
    class Sample<T>
    {
        public delegate void GenericEventHandler<T>(object sender, T eventArgs);

        public event GenericEventHandler<T> OnGenericEvent;
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}