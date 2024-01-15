using mailing_list_net.Models;

namespace mailing_list_net.Interfaces
{
    public interface IGroupContactRepository
    {
        GroupContact GetGroupContact(int groupId, int contactId);
        bool addGroupContact(GroupContact groupContact);
        bool DeleteGroupContact(GroupContact groupContact);
    }
}
