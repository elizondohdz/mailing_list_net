using AutoMapper;
using mailing_list_net.Dto;
using mailing_list_net.Interfaces;
using mailing_list_net.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace mailing_list_net.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactController(IContactRepository contactRepository, IMapper mapper)
        {
            this._contactRepository = contactRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Contact>))]
        public IActionResult GetContacts()
        {
            var contacts = _mapper.Map<List<ContactDto>>(_contactRepository.GetContacts());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(contacts);
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(200, Type = typeof(Contact))]
        [ProducesResponseType(400)]
        public IActionResult GetContact(string uuid)
        {
            if (!_contactRepository.ContactExists(uuid))
                return NotFound();

            var contact = _mapper.Map<ContactDto>(_contactRepository.GetContact(uuid));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(contact);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateContact([FromBody] ContactDto contactCreate)
        {
            if (contactCreate == null)
                return BadRequest(ModelState);

            var contact = _contactRepository.GetContacts()
                .Where(c => c.Email.Trim().ToUpper() == contactCreate.Email.ToUpper())
                .FirstOrDefault();

            if (contact != null)
            {
                ModelState.AddModelError("", "Contact already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            contactCreate.Uuid = Guid.NewGuid().ToString();

            var contactMap = _mapper.Map<Contact>(contactCreate);

            if (!_contactRepository.CreateContact(contactMap))
            {
                ModelState.AddModelError("", "Something went  wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(contact);
        }

        [HttpPut("{contactUuid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateContact(string contactUuid, [FromBody]ContactDto updatedContact) 
        {
            if (updatedContact == null)
                return BadRequest(ModelState);

            if (contactUuid != updatedContact.Uuid)
                return BadRequest(ModelState);

            if (!_contactRepository.ContactExists(contactUuid))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var currentContact = _contactRepository.GetContact(contactUuid);

            currentContact.Email = updatedContact.Email;
            currentContact.Phone = updatedContact.Phone;
            currentContact.Name = updatedContact.Name;

            if (!_contactRepository.UpdateContact(contactUuid, currentContact))
            {
                ModelState.AddModelError("", "Something went wrong updating");
            }

            return NoContent();
        }

        [HttpDelete("{contactUuid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteContact(string contactUuid)
        {
            if (!_contactRepository.ContactExists(contactUuid))
                return NotFound(ModelState);

            var contactDelete = _contactRepository.GetContact(contactUuid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contactRepository.DeleteContact(contactDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting contact");
            }

            return NoContent();

        }


    }


}
