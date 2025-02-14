# Contact Manager - Blazor WebAssembly Frontend

## Overview

This is the **frontend** for the Contact Manager application, built with **Blazor WebAssembly**. It provides an
intuitive user interface for managing contacts and their addresses while interacting with the backend API.

## Features

- **Blazor WebAssembly** for a modern client-side application.
- **Bootstrap UI** for a clean and responsive design.
- **Pagination support** for better data handling.
- **API Integration** with the backend for seamless data management.

## Folder Structure

```
BlazorApp2/
├── wwwroot/ # Static assets (CSS, JS, etc.)
├── Pages/
│   ├── Contacts.razor # Displays a paginated list of contacts.
│   ├── CreateContact.razor # Form for adding contacts.
│   ├── EditContact.razor # Form for edit contacts.
│   ├── Home.razor # Home page which allows for searching.
│   ├── SearchResults.razor # Page with search results.
│   ├── ViewContact.razor # Page which allows for viewing of individual contac.
├── Services/
│   ├── ContactService.cs # Handles API calls for contacts.
├── Layout/
│   ├── MainLayout.razor # Main UI layout.
│   ├── NavMenu.razor # Navigation menu.
│   ├── Footer.razor # Footer component.
├── App.razor # Root Blazor component.
├── Program.cs # Configures Blazor services.
└── BlazorApp2.csproj # Project file.
```

## Setup Instructions

1. Clone the Repository
   ```bash
    git clone <repository-url>

2. **Navigate to the root directory:**
   ```bash
   cd {root directory}

3. Configure API Endpoint  
   Modify appsettings.json to point to the backend API:
   ```bash
   "ApiSettings": {
         "BaseUrl": "http://localhost:5246/api/contact"
   }

4. **Restore Dependencies:**  
    ```bash
    dotnet restore

5. **Build the Project:**  
   ```bash
   dotnet build 

6. **Run Tests:**  
   ```bash
   dotnet test

7. **Run:**  
   ```bash
   dotnet run

## How to Use

* **View Contacts:** Navigate to the Contacts page to see a list of saved contacts.
* **Search Contacts:** Use the search bar to find contacts by name.
* **Add New Contact:** Click "Add Contact," fill out the form, and save.
* **Edit Contact:** Click "Edit" next to a contact to update it.
* **Delete Contact:** Click "Delete" to remove a contact permanently.
* **Pagination:** Navigate through contacts using the pagination controls.