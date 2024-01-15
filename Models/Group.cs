namespace mailing_list_net.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public string Uuid { get; set; }
        public ICollection<GroupContact> GroupsContacts { get; set;}
    }
}
