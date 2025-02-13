using System.Net.Http.Json;
using ClassLibrary1.DTOs;

namespace BlazorApp2.Services
{
    public class ContactService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // In a real app, you might inject IConfiguration or use a named HttpClient.
            _baseUrl = "http://localhost:5246/api/contact";
        }

        public async Task<List<ContactDTO>> GetContacts(int pageNumber, int pageSize)
        {
            try
            {
                var contacts = await _httpClient.GetFromJsonAsync<List<ContactDTO>>(
                    $"{_baseUrl}?pageNumber={pageNumber}&pageSize={pageSize}");
                return contacts ?? new List<ContactDTO>();
            }
            catch (Exception ex)
            {
                // Log error as needed
                throw new ApplicationException("Error fetching contacts", ex);
            }
        }

        public async Task<List<ContactDTO>> SearchContacts(string name, int pageNumber, int pageSize)
        {
            return await _httpClient.GetFromJsonAsync<List<ContactDTO>>(
                $"{_baseUrl}/search?name={name}&pageNumber={pageNumber}&pageSize={pageSize}");
        }

        public async Task<ContactDTO> GetContactById(int id)
        {
            try
            {
                var contact = await _httpClient.GetFromJsonAsync<ContactDTO>($"{_baseUrl}/{id}");
                if (contact == null)
                {
                    throw new ApplicationException($"No contact found with id {id}");
                }

                return contact;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching contact with id {id}", ex);
            }
        }

        public async Task<HttpResponseMessage> CreateContact(ContactDTO contact)
        {
            try
            {
                return await _httpClient.PostAsJsonAsync(_baseUrl, contact);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating contact", ex);
            }
        }

        public async Task<HttpResponseMessage> UpdateContact(int id, ContactDTO contact)
        {
            try
            {
                return await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", contact);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating contact with id {id}", ex);
            }
        }

        public async Task<HttpResponseMessage> DeleteContact(int id)
        {
            try
            {
                return await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting contact with id {id}", ex);
            }
        }
    }
}