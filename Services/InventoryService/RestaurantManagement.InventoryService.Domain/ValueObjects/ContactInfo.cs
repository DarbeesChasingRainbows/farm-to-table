using System;
using System.Text.RegularExpressions;
using RestaurantManagement.InventoryService.Domain.Exceptions;

namespace RestaurantManagement.InventoryService.Domain.ValueObjects
{
    public class ContactInfo
    {
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex = new Regex(@"^(\+\d{1,3}( )?)?((\(\d{1,3}\))|\d{1,3})[- .]?\d{3,4}[- .]?\d{4}$", RegexOptions.Compiled);

        private ContactInfo() { }

        public ContactInfo(
            string name,
            string email,
            string phone,
            string website = null,
            string notes = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InventoryDomainException("Contact name cannot be empty");

            if (!string.IsNullOrWhiteSpace(email) && !EmailRegex.IsMatch(email))
                throw new InventoryDomainException("Invalid email format");

            if (!string.IsNullOrWhiteSpace(phone) && !PhoneRegex.IsMatch(phone))
                throw new InventoryDomainException("Invalid phone format");

            Name = name;
            Email = email;
            Phone = phone;
            Website = website;
            Notes = notes;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Website { get; private set; }
        public string Notes { get; private set; }

        public static ContactInfo Empty => new ContactInfo(
            name: "Unknown",
            email: null,
            phone: null,
            website: null,
            notes: null
        );

        public ContactInfo WithEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && !EmailRegex.IsMatch(email))
                throw new InventoryDomainException("Invalid email format");

            return new ContactInfo(
                name: Name,
                email: email,
                phone: Phone,
                website: Website,
                notes: Notes
            );
        }

        public ContactInfo WithPhone(string phone)
        {
            if (!string.IsNullOrWhiteSpace(phone) && !PhoneRegex.IsMatch(phone))
                throw new InventoryDomainException("Invalid phone format");

            return new ContactInfo(
                name: Name,
                email: Email,
                phone: phone,
                website: Website,
                notes: Notes
            );
        }

        public ContactInfo WithWebsite(string website)
        {
            return new ContactInfo(
                name: Name,
                email: Email,
                phone: Phone,
                website: website,
                notes: Notes
            );
        }

        public ContactInfo WithNotes(string notes)
        {
            return new ContactInfo(
                name: Name,
                email: Email,
                phone: Phone,
                website: Website,
                notes: notes
            );
        }

        public bool HasEmail()
        {
            return !string.IsNullOrWhiteSpace(Email);
        }

        public bool HasPhone()
        {
            return !string.IsNullOrWhiteSpace(Phone);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ContactInfo other = (ContactInfo)obj;
            return Name == other.Name &&
                   Email == other.Email &&
                   Phone == other.Phone &&
                   Website == other.Website &&
                   Notes == other.Notes;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Email, Phone, Website, Notes);
        }

        public override string ToString()
        {
            string contact = $"{Name}";
            
            if (!string.IsNullOrWhiteSpace(Email))
                contact += $" | {Email}";
                
            if (!string.IsNullOrWhiteSpace(Phone))
                contact += $" | {Phone}";
                
            return contact;
        }
    }
}
