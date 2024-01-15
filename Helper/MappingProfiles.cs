using AutoMapper;
using mailing_list_net.Dto;
using mailing_list_net.Models;

namespace mailing_list_net.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Contact, ContactDto>();
            CreateMap<Group, GroupDto>();

            CreateMap<ContactDto, Contact>();
            CreateMap<GroupDto, Group>();

            CreateMap<GroupContactDto, GroupContact>();
            CreateMap<GroupContact, GroupContactDto>();
        }
    }
}
