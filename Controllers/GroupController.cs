using AutoMapper;
using mailing_list_net.Dto;
using mailing_list_net.Interfaces;
using mailing_list_net.Models;
using mailing_list_net.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace mailing_list_net.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class GroupController : Controller
        {
            private readonly IGroupRepository _groupRepository;
            private readonly IMapper _mapper;

            public GroupController(IGroupRepository groupRepository, IMapper mapper)
            {
                this._groupRepository = groupRepository;
                this._mapper = mapper;
            }

            [HttpGet]
            [ProducesResponseType(200, Type = typeof(IEnumerable<Group>))]
            public IActionResult GetGroups()
            {
                var groups = _mapper.Map<List<GroupDto>>(_groupRepository.GetGroups());

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(groups);
            }

            [HttpGet("{uuid}")]
            [ProducesResponseType(200, Type = typeof(Contact))]
            [ProducesResponseType(400)]
            public IActionResult GetGroup(string uuid)
            {
                if (!_groupRepository.GroupExists(uuid))
                    return NotFound();

                var group = _mapper.Map<GroupDto>(_groupRepository.GetGroup(uuid));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(group);
            }

        [HttpGet("{uuid}/contacts")]
        [ProducesResponseType(200, Type = typeof(Contact))]
        [ProducesResponseType(400)]
        public IActionResult GetGroupContacts(string uuid)
        {
            if (!_groupRepository.GroupExists(uuid))
                return NotFound();

            var contacts = _mapper.Map<List<ContactDto>>(_groupRepository.GetGroupContacts(uuid));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(contacts);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGroup([FromBody] GroupDto groupCreate)
        {
            if (groupCreate == null)
                return BadRequest(ModelState);

            var group = _groupRepository.GetGroups()
                .Where(g => g.Name.Trim().ToUpper() == groupCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (group != null)
            {
                ModelState.AddModelError("", "Group already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            groupCreate.Uuid = Guid.NewGuid().ToString();

            var groupMap = _mapper.Map<Group>(groupCreate);

            if (!_groupRepository.CreateGroup(groupMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }

            return Ok(group);
        }

        [HttpPut("{groupUuid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateGroup(string groupUuid, [FromBody] GroupDto updatedGroup)
        {
            if (updatedGroup == null)
                return BadRequest(ModelState);

            if (groupUuid != updatedGroup.Uuid)
                return BadRequest(ModelState);

            if (!_groupRepository.GroupExists(groupUuid))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var groupUpdate = _groupRepository.GetGroup(groupUuid);

            groupUpdate.Name = updatedGroup.Name;
            groupUpdate.Description = updatedGroup.Description;

            if (!_groupRepository.UpdateGroup(groupUuid, groupUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating");
            }

            return NoContent();
        }

        [HttpDelete("{groupUuid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteGroup(string groupUuid) 
        {
            if (!_groupRepository.GroupExists(groupUuid))
                return NotFound(ModelState);

            var groupDelete = _groupRepository.GetGroup(groupUuid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_groupRepository.DeleteGroup(groupDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting group");
            }

            return NoContent();
        }

    }
}
