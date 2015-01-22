using System;
using System.Security.Cryptography.X509Certificates;

namespace Visutron900PlusInterface.Adapter
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IndexAttribute : Attribute
    {
        public int Index { get; set; }

        public bool ForcePlusSign { get; set; }

        public IndexAttribute(int index)
        {
            Index = index;
        }
    }
}