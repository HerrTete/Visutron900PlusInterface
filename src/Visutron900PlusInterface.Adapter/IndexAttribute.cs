using System;

namespace Visutron900PlusInterface.Adapter
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        public int Index { get; set; }

        public IndexAttribute(int index)
        {
            Index = index;
        }
    }
}