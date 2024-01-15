using mailing_list_net.Models;

namespace mailing_list_net.Interfaces
{
    public interface IContactRepository
    {
        ICollection<Contact> GetContacts();
        Contact GetContact(string uuid);
        bool ContactExists(string uuid);
        bool CreateContact(Contact contact);
        bool UpdateContact(string uuid, Contact contact);
        bool DeleteContact(Contact contact);
    }
}
