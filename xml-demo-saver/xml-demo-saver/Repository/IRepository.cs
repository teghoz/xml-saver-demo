using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xml_demo_saver.Models;

namespace xml_demo_saver.Repository
{
    public interface IRepository
    {
        List<User> Users(UserFilter filter);
        User UpdateUser(User user);
        void AddUser(User user);
        void DeleteUser(int userId);
        void DeleteUser(User user);
    }
}
