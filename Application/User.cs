using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class User
    {

        public static bool Exists(int personId)
        {
            return UserData.IsUserExistsByPersonId(personId);
        }
    }
}
