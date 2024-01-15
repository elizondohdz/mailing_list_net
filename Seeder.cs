using mailing_list_net.Data;
using mailing_list_net.Models;

namespace mailing_list_net
{
    public class Seeder
    {
        private readonly DataContext dataContext;
        public Seeder(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            var groupContacts = new List<GroupContact>()
            {
                new GroupContact()
                {
                    Contact = new Contact()
                    {
                        Name = "Ellijah Wood",
                        Phone = "555-696969",
                        Email = "Ellijah@fake.com",
                        Uuid = "ASDF-1234-ZXCV-5678"
                    },
                    Group = new Group()
                    {
                        Name = "Movie Stars",
                        Description = "Lorem ipsum dolor sit amet",
                        Uuid = "0987-POIU-7646-TGBY"
                    }
                }
            };
            dataContext.GroupContacts.AddRange(groupContacts);
            dataContext.SaveChanges();
        }
    }
}
