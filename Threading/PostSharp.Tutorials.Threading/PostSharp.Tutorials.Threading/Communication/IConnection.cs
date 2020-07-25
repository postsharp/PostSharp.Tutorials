using System;

namespace PostSharp.Tutorials.Threading.Communication
{
    internal interface IConnection
    {
        event EventHandler Closed;
        void Close();
    }
}
