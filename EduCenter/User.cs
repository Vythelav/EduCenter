using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCenter
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime DateOfBirth { get; set; }
    }


}
