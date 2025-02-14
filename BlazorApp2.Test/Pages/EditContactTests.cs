using BlazorApp2.Pages;
using Bunit;
using ClassLibrary1.DTOs;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorApp2.Test.Pages;

public class EditContactTests : TestContext
{
    private readonly Mock<IContactService> mockContactService;
    private readonly NavigationManager navigationManager;

    public EditContactTests()
    {
        mockContactService = new Mock<IContactService>();

        // Register mock service
        Services.AddSingleton(mockContactService.Object);

        // Use bUnit's Fake NavigationManager
        navigationManager = Services.GetRequiredService<NavigationManager>();
    }

    [Fact]
    public async Task EditContact_ShouldRenderForm_WithExistingContact()
    {
        // Arrange
        var contact = new ContactDTO
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Addresses = new List<AddressDTO>
            {
                new AddressDTO { Street = "123 Main St", City = "Springfield", State = "IL", ZipCode = "62704" }
            }
        };

        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        // Act
        var cut = Render<EditContact>(parameters => parameters.Add(p => p.id, 1));

        // Assert: Ensure form fields are populated correctly
        Assert.Equal("John", cut.Find("#firstName").GetAttribute("value"));
        Assert.Equal("Doe", cut.Find("#lastName").GetAttribute("value"));
        Assert.Equal("123 Main St", cut.Find("#street").GetAttribute("value"));
        Assert.Equal("Springfield", cut.Find("#city").GetAttribute("value"));
        Assert.Equal("IL", cut.Find("#state").GetAttribute("value"));
        Assert.Equal("62704", cut.Find("#zip").GetAttribute("value"));
    }

    [Fact]
    public async Task EditContact_ShouldShowNoAddressMessage_WhenNoAddressExists()
    {
        // Arrange
        var contact = new ContactDTO { Id = 2, FirstName = "Jane", LastName = "Doe", Addresses = new List<AddressDTO>() };

        mockContactService
            .Setup(s => s.GetContactByIdAsync(2))
            .ReturnsAsync(contact);

        // Act
        var cut = Render<EditContact>(parameters => parameters.Add(p => p.id, 2));

        // Assert: Check if the no address message is displayed
        var alertDiv = cut.Find("div.alert.alert-info");
        alertDiv.MarkupMatches(@"<div class='alert alert-info'>No address available. Please add an address via the Create Contact page.</div>");
    }


    [Fact]
    public async Task EditContact_ShouldCallUpdateService_OnValidSubmit()
    {
        // Arrange
        var contact = new ContactDTO
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Addresses = new List<AddressDTO>
            {
                new AddressDTO { Street = "123 Main St", City = "Springfield", State = "IL", ZipCode = "62704" }
            }
        };

        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        mockContactService
            .Setup(s => s.UpdateContactAsync(It.IsAny<int>(), It.IsAny<ContactDTO>()))
            .ReturnsAsync(true);

        var cut = Render<EditContact>(parameters => parameters.Add(p => p.id, 1));

        // Modify a field
        cut.Find("#firstName").Change("Johnny");

        // Act: Submit the form
        await cut.Find("form").SubmitAsync();

        // Assert: Verify update service was called
        mockContactService.Verify(s => s.UpdateContactAsync(1, It.Is<ContactDTO>(c => c.FirstName == "Johnny")), Times.Once);

        // Assert: Navigation should have happened
        Assert.Equal("http://localhost/contacts", navigationManager.Uri);
    }

    [Fact]
    public void EditContact_ShouldNavigateBack_WhenCancelIsClicked()
    {
        // Arrange: Mock contact service to return a valid contact
        var contact = new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe", Addresses = new List<AddressDTO>() };

        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        // Act: Render the component
        var cut = Render<EditContact>(parameters => parameters.Add(p => p.id, 1));

        // Wait for the async call to complete
        cut.WaitForState(() => cut.Find("button.btn-secondary") != null);

        // Click the cancel button
        cut.Find("button.btn-secondary").Click();

        // Assert: Ensure navigation happened
        Assert.Equal("http://localhost/contacts", navigationManager.Uri);
    }
}
