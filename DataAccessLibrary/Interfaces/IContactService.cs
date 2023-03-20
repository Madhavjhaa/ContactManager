using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contacts>> GetContactsAsync();

        Task<Contacts> GetContactByIdAsync(Int32 id);

        Task<Int32> CreateContactAsync(Contacts contact);

        Task UpdateContactAsync(Contacts contact);

        Task DeleteContactAsync(Int32 id);
    }
}