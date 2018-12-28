using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Security
{
    public interface IUserRepository<TUser> where TUser : class
    {
        void AddNewUser(TUser user);

        void UpdateUser(TUser user);

        void DeleteUser(TUser user);

        TUser Find(int id);
    }
}
