using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;

        public ContactController(IContactService contactService)
        {
            this.contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacts>>> GetAllContacts()
        {
            IEnumerable<Contacts> contacts = await this.GetAllContacs();

            return this.Ok(contacts);
        }


        [HttpGet("{contactId}")]
        public async Task<ActionResult<Contacts>> GetContactById(Int32 contactId)
        {
            Contacts contact = await this.contactService.GetContactByIdAsync(contactId);

            return this.Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Contacts>>> CreateContact(Contacts contact)
        {
            await this.contactService.CreateContactAsync(contact);

            return this.Ok(await this.GetAllContacs());
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<Contacts>>> UpdateContact(Contacts contact)
        {
            await this.contactService.UpdateContactAsync(contact);
            return this.Ok(await this.GetAllContacs());
        }

        [HttpDelete("{contactId}")]
        public async Task<ActionResult<IEnumerable<Contacts>>> DeleteContact(Int32 contactId)
        {
            await this.contactService.DeleteContactAsync(contactId);
            return this.Ok(await this.GetAllContacs());
        }

        private async Task<IEnumerable<Contacts>> GetAllContacs()
        {
            return await this.contactService.GetContactsAsync();
        }
    }
}