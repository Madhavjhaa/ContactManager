using Dapper;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary.Services
{
    public class ContactService : IContactService
    {
        private NpgsqlConnection connection;
        private const String TABLE_NAME = "contacts";

        public ContactService(NpgsqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.connection.Open();
        }

        public IConfiguration Config { get; }

        public async Task<IEnumerable<Contacts>> GetContactsAsync()
        {
            String query = $"SELECT * FROM {TABLE_NAME}";

            return await this.connection.QueryAsync<Contacts>(query);
        }

        public async Task<Contacts> GetContactByIdAsync(Int32 id)
        {
            String query = $"SELECT * FROM {TABLE_NAME} WHERE Id = @id";
            var queryArgs = new { Id = id };

            return await this.connection.QueryFirstAsync<Contacts>(query, queryArgs);
        }

        public async Task<Int32> CreateContactAsync(Contacts contact)
        {
            String query = $@"INSERT INTO {TABLE_NAME} (Id, Salutation, FirstName, LastName, DisplayName, Birthdate, CreationTimestamp, LastChangeTimestamp, NotifyHasBirthdaySoon, Email, PhoneNumber)
                                VALUES (@id, @salutation, @firstName, @lastName, @displayName, @birthdate, @creationTimestamp, @lastChangeTimestamp, @notifyHasBirthdaySoon, @email, @phoneNumber)";

            var queryArgs = new
            {
                id = contact.Id,
                salutation = contact.Salutation,
                firstName = contact.FirstName,
                lastName = contact.LastName,
                displayName = contact.DisplayName,
                birthdate = contact.Birthdate,
                creationTimestamp = contact.CreationTimestamp,
                lastChangeTimestamp = contact.LastChangeTimestamp,
                notifyHasBirthdaySoon = contact.NotifyHasBirthdaySoon,
                email = contact.Email,
                phoneNumber = contact.PhoneNumber
            };

            return await this.connection.ExecuteAsync(query, queryArgs);
        }

        public async Task UpdateContactAsync(Contacts contact)
        {
            String query = $@"UPDATE {TABLE_NAME}
                SET Id = @id,
                    Salutation = @salutation,
                    FirstName = @firstName,
                    LastName = @lastName,
                    DisplayName = @displayName,
                    Birthdate = @birthdate,
                    CreationTimestamp = @creationTimestamp,
                    LastChangeTimestamp = @lastChangeTimestamp,
                    NotifyHasBirthdaySoon = @notifyHasBirthdaySoon,
                    Email = @email,
                    PhoneNumber = @phoneNumber
                WHERE id = @id";

            var queryArgs = new
            {
                id = contact.Id,
                salutation = contact.Salutation,
                firstName = contact.FirstName,
                lastName = contact.LastName,
                displayName = contact.DisplayName,
                birthdate = contact.Birthdate,
                creationTimestamp = contact.CreationTimestamp,
                lastChangeTimestamp = contact.LastChangeTimestamp,
                notifyHasBirthdaySoon = contact.NotifyHasBirthdaySoon,
                email = contact.Email,
                phoneNumber = contact.PhoneNumber
            };

            await this.connection.ExecuteAsync(query, queryArgs);
        }

        public async Task DeleteContactAsync(Int32 id)
        {
            String query = $"DELETE FROM {TABLE_NAME} WHERE Id = (@id)";
            var queryArgs = new { Id = id };

            await this.connection.ExecuteAsync(query, queryArgs);
        }
    }
}