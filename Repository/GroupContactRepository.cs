using mailing_list_net.Data;
using mailing_list_net.Interfaces;
using mailing_list_net.Models;
using Microsoft.EntityFrameworkCore;

namespace mailing_list_net.Repository
{
    public class GroupContactRepository : IGroupContactRepository
    {
        private readonly DataContext _context;
        public GroupContactRepository(DataContext context)
        {
            _context = context;
        }
        public bool addGroupContact(GroupContact groupContact)
        {
            _context.GroupContacts.Add(groupContact);
            return Save();
        }

        public bool DeleteGroupContact(GroupContact groupContact)
        {
            _context.Remove(groupContact);
            return Save();
        }

        public GroupContact GetGroupContact(int groupId, int contactId)
        {
            return _context.GroupContacts
                .Where(c => c.ContactId == contactId)
                .Where(d => d.GroupId == groupId).FirstOrDefault();
        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
