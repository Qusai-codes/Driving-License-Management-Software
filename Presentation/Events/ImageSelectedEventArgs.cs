using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events
{
    public class ImageSelectedEventArgs : EventArgs
    {
        public string FilePath { get; }

        public ImageSelectedEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
