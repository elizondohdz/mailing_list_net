using mailing_list_net.Models;

namespace mailing_list_net.Interfaces
{
    public interface IGroupRepository
    {
        ICollection<Group> GetGroups();
        Group GetGroup(string uuid);
        bool GroupExists(string uuid);
        ICollection<Contact> GetGroupContacts(string uuid);
        bool CreateGroup(Group group);
        bool UpdateGroup(string uuid, Group group);
        bool DeleteGroup(Group group);
    }
}
