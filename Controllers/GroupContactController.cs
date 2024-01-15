using AutoMapper;
using mailing_list_net.Interfaces;
using Microsoft.AspNetCore.Mvc;
using mailing_list_net.Dto;
using mailing_list_net.Models;
using mailing_list_net.Repository;

namespace mailing_list_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupContactController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupContactRepository _groupContactRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IContactRepository _contactRepository;
        public GroupContactController(IGroupContactRepository groupContactRepository, IMapper mapper, IGroupRepository groupRepository, IContactRepository contactRepository)
        {
            _mapper = mapper;
            _groupContactRepository = groupContactRepository;
            _groupRepository = groupRepository;
            _contactRepository = contactRepository;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGroupContact([FromBody] GroupContactDto groupContactCreate)
        {

            if (groupContactCreate == null)
                return BadRequest(ModelState);

            var group = _groupRepository.GetGroups()
                .Where(g => g.Uuid.Trim() == groupContactCreate.GroupId.ToString())
                .FirstOrDefault();

            var contact = _contactRepository.GetContacts()
                .Where(c => c.Uuid == groupContactCreate.ContactId.ToString())
                .FirstOrDefault();

            if (group == null || contact == null)
            {
                ModelState.AddModelError("", "Data is not valid!");
                return StatusCode(422, ModelState);
            }

            groupContactCreate.ContactId = contact.Id.ToString();
            groupContactCreate.GroupId = group.Id.ToString();

            var groupCreateMap = _mapper.Map<GroupContact>(groupContactCreate);

            if (!_groupContactRepository.addGroupContact(groupCreateMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteGroupContact(GroupContactDto groupReceived) 
        {
            if (groupReceived == null)
                return BadRequest(ModelState);
            
            var group = _groupRepository.GetGroup(groupReceived.GroupId);
            var contact = _contactRepository.GetContact(groupReceived.ContactId);
            
            var groupContactDelete = _groupContactRepository.GetGroupContact(group.Id, contact.Id);

            if (groupContactDelete == null)
            {
                ModelState.AddModelError("", "Data is not valid!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_groupContactRepository.DeleteGroupContact(groupContactDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting group");
            }

            return NoContent();
        }

    }
}
