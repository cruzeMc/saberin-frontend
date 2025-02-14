using System.Net.Http.Json;
using ClassLibrary1.DTOs;
using ClassLibrary1.Services;

namespace BlazorApp2.Services
{
    namespace BlazorApp2.Services
    {
        public class ContactService : IContactService
        {
            private readonly HttpClient _httpClient;
            private readonly string _baseUrl;

            public ContactService(HttpClient httpClient, IConfiguration configuration)
            {
                _httpClient = httpClient;
                _baseUrl = configuration["ApiSettings:BaseUrl"]
                           ?? throw new InvalidOperationException("API Base URL is not configured.");
            }

            public async Task<IEnumerable<ContactDTO>> GetContactsAsync(int pageNumber, int pageSize)
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>(
                           $"{_baseUrl}?pageNumber={pageNumber}&pageSize={pageSize}")
                       ?? new List<ContactDTO>();
            }

            public async Task<IEnumerable<ContactDTO>> SearchContactAsync(string name, int pageNumber, int pageSize)
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>(
                           $"{_baseUrl}/search?name={name}&pageNumber={pageNumber}&pageSize={pageSize}")
                       ?? new List<ContactDTO>();
            }

            public async Task<ContactDTO> GetContactByIdAsync(int id)
            {
                return await _httpClient.GetFromJsonAsync<ContactDTO>($"{_baseUrl}/{id}")
                       ?? throw new InvalidOperationException($"No contact found with id {id}");
            }

            public async Task<ContactDTO> CreateContactAsync(ContactDTO contact)
            {
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, contact);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ContactDTO>();
            }

            public async Task<bool> UpdateContactAsync(int id, ContactDTO contact)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", contact);
                return response.IsSuccessStatusCode;
            }

            public async Task<bool> DeleteContactAsync(int id)
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
        }
    }
}