using mailing_list_net.Data;
using mailing_list_net.Interfaces;
using mailing_list_net.Models;

namespace mailing_list_net.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;
        public GroupRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateGroup(Group group)
        {
            _context.Groups.Add(group);
            return Save();
        }

        public bool DeleteGroup(Group group)
        {
            _context.Remove(group);
            return Save();
        }

        public Group GetGroup(string uuid)
        {
            return _context.Groups.Where(g => g.Uuid == uuid).FirstOrDefault();
        }

        public ICollection<Contact> GetGroupContacts(string uuid)
        {
            var group = _context.Groups.Where(g => g.Uuid == uuid).FirstOrDefault();

            return _context.GroupContacts.Where(gc => gc.Group.Id == group.Id).Select(c => c.Contact).ToList();
        }

        public ICollection<Group> GetGroups()
        {
            return _context.Groups.OrderBy(g => g.Id).ToList();
        }

        public bool GroupExists(string uuid)
        {
            return _context.Groups.Any(g => g.Uuid == uuid);
        }

        public bool UpdateGroup(string uuid, Group group)
        {
            _context.Update(group);
            return Save();
        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
