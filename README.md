# Contact Manager - Blazor WebAssembly Frontend

## Overview

This is the **frontend** for the Contact Manager application, built with **Blazor WebAssembly**. It provides an
intuitive user interface for managing contacts and their addresses while interacting with the backend API.

## Features

- **Blazor WebAssembly** for a modern client-side application.
- **Bootstrap UI** for a clean and responsive design.
- **CRUD operations** for managing contacts and addresses.
- **Pagination support** for better data handling.
- **API Integration** with the backend for seamless data management.

## Folder Structure

```
ContactManagerFrontend/
├── wwwroot/ # Static assets (CSS, JS, etc.)
├── Pages/
│   ├── Contacts.razor # Displays a paginated list of contacts.
│   ├── ContactForm.razor # Form for adding/editing contacts.
│   ├── ContactDetails.razor # Shows details of a specific contact.
│   ├── Index.razor # Home page.
│   ├── Error.razor # Error handling page.
├── Components/
│   ├── Pagination.razor # Component for pagination.
│   ├── AddressForm.razor # Address entry component.
│   ├── ContactList.razor # Displays contacts in a table.
├── Services/
│   ├── ContactService.cs # Handles API calls for contacts.
│   ├── AddressService.cs # Handles API calls for addresses.
├── Models/
│   ├── Contact.cs # Defines Contact model.
│   ├── Address.cs # Defines Address model.
│   ├── ApiResponse.cs # Model for API responses.
├── DTOs/
│   ├── ContactDTO.cs # DTO for contacts.
│   ├── AddressDTO.cs # DTO for addresses.
├── Shared/
│   ├── MainLayout.razor # Main UI layout.
│   ├── NavMenu.razor # Navigation menu.
│   ├── Footer.razor # Footer component.
├── App.razor # Root Blazor component.
├── Program.cs # Configures Blazor services.
└── ContactManagerFrontend.csproj # Project file.
```

## Setup Instructions

1. Clone the Repository
   ```bash
    git clone <repository-url>

2. Configure API Endpoint  
   Modify appsettings.json or ContactService.cs to point to the backend API:
   ```bash
   private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5246/api/") };

3. **Restore Dependencies:**  
   From the solution root, run
    ```bash
    dotnet restore

4. **Build the Project:**  
   Navigate to the ClassLibrary1 directory and build the project:
   ```bash
   dotnet build 

5. **Run Tests:**  
   If tests are provided (e.g., for mapping configurations or model validations), navigate to the test project folder
   and run:
   ```bash
   dotnet test

6. **Run:**  
   Run the API by executing:
   ```bash
   dotnet run

## How to Use

* **View Contacts:** Navigate to the Contacts page to see a list of saved contacts.
* **Search Contacts:** Use the search bar to find contacts by name.
* **Add New Contact:** Click "Add Contact," fill out the form, and save.
* **Edit Contact:** Click "Edit" next to a contact to update it.
* **Delete Contact:** Click "Delete" to remove a contact permanently.
* **Pagination:** Navigate through contacts using the pagination controls.