using Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Events
{
    public class PersonEventArgs : EventArgs
    {
        public PersonDto Person { get; set; }

        public PersonEventArgs(PersonDto person)
        {
            Person = person;
        }
    }
}
