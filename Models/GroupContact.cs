namespace mailing_list_net.Models
{
    public class GroupContact
    {
        public int GroupId { get; set; }
        public int ContactId { get; set; }
        public Group Group { get; set; }
        public Contact Contact { get; set; }

    }
}
