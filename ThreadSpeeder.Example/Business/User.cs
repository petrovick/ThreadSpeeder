using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadSpeeder.Example.Business
{
    public class User
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public byte Age { get; set; }
        public virtual Address Address { get; set; }
    }
}
