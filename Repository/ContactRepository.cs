using mailing_list_net.Data;
using mailing_list_net.Interfaces;
using mailing_list_net.Models;

namespace mailing_list_net.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _context;
        public ContactRepository(DataContext context)
        {
            this._context = context;
        }

        public bool ContactExists(string uuid)
        {
            return _context.Contacts.Any(c => c.Uuid == uuid);
        }

        public bool CreateContact(Contact contact)
        {
            _context.Add(contact);
            return Save();
        }

        public bool DeleteContact(Contact contact)
        {
            _context.Remove(contact);
            return Save();
        }

        public Contact GetContact(string uuid)
        {
            return _context.Contacts.Where(c => c.Uuid == uuid).FirstOrDefault();
        }

        public ICollection<Contact> GetContacts()
        {
            return _context.Contacts.OrderBy(c => c.Id).ToList();
        }

        public bool UpdateContact(string uuid, Contact contact)
        {
            
            _context.Update(contact);
            return Save();
        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
