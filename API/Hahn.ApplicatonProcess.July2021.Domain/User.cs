using System.Collections.Generic;

namespace Hahn.ApplicationProcess.July2021.Domain
{
    public class User
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public IList<Asset> Assets { get; set; }
    }
}