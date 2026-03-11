using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events
{
    public class PersonSavedEventArgs : EventArgs
    {
        public int PersonId { get; }

        public PersonSavedEventArgs(int personId)
        {
            PersonId = personId;
        }
    }
}
