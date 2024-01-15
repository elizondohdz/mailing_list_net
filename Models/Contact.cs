namespace mailing_list_net.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Uuid {  get; set; }
        public ICollection<GroupContact> GroupContacts { get; set; }
    }
}
