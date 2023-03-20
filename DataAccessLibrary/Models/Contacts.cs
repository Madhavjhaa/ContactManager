using System;

namespace DataAccessLibrary.Models
{
    public class Contacts
    {
        public Int32 Id { get; set; }
        public String Salutation { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String DisplayName { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public DateTime? LastChangeTimestamp { get; set; }
        public Boolean NotifyHasBirthdaySoon { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
    }
}