using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events
{
    public class LicenseSelectedEventArgs : EventArgs
    {
        public int LicenseId { get; private set; }

        public LicenseSelectedEventArgs(int licenseId)
        {
            LicenseId = licenseId;
        }
    }
}
