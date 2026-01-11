using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public abstract class EntityBase
    {
        public int ID { get; protected set; }

        public EntityPersistenceState State { get; protected set; }

        protected EntityBase()
        {
            ID = -1;
            State = EntityPersistenceState.New;
        }

        protected void MarkAsExisting()
        {
            State = EntityPersistenceState.Existing;
        }

        public abstract bool Save();
    }

}
