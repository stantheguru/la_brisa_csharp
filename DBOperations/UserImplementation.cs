using la_brisa.Models;

using Microsoft.EntityFrameworkCore;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

namespace la_brisa.DBOperations
{
    public class UserImplmentation : IUser
    {
        private readonly DBContext _context;

        public UserImplmentation(DBContext context)

        {

            _context = context;

        }

        public async Task<User> Create(User user)

        {

            //Add student record

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;

        }

 

        public async Task<IEnumerable<User>> Get()

        {

            //Select and list all entries

            return await _context.Users.ToListAsync();

        }

       

        

    }

}

