namespace Application.Models
{
    public class User
    {
        public Guid ID { get; set; } // Primary Key, Same as Login ID
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid AddressID { get; set; } // Foreign Key referencing Address table
        public string? PostalCode { get; set; }
        public bool Blacklisted { get; set; }
        public Guid RoleID { get; set; } // Foreign Key referencing Role table
    }
}
