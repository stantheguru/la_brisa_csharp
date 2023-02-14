using la_brisa.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace la_brisa.DBOperations
{
    public interface IUser
    {
        Task<IEnumerable<User>> Get();
        Task<User> Create(User user);
        
    }
}
