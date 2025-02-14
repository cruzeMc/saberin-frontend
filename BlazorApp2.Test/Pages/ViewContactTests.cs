using BlazorApp2.Pages;
using Bunit;
using ClassLibrary1.DTOs;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class ViewContactTests : TestContext
{
    private readonly Mock<IContactService> mockContactService;
    private readonly NavigationManager navigationManager;

    public ViewContactTests()
    {
        mockContactService = new Mock<IContactService>();
        Services.AddSingleton(mockContactService.Object);
        navigationManager = Services.GetRequiredService<NavigationManager>();
    }

    [Fact]
    public async Task ViewContact_ShouldShowLoadingSpinner_WhenFetchingContact()
    {
        // Arrange: Simulate delay by not returning a contact immediately
        var tcs = new TaskCompletionSource<ContactDTO>(); // Holds response

        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .Returns(tcs.Task); // Does not complete immediately

        var cut = Render<ViewContact>(parameters => parameters.Add(p => p.id, 1));

        // Act: Wait until the component enters a loading state
        cut.WaitForState(() => cut.Markup.Contains("spinner-border"));

        // Assert: Ensure loading spinner appears
        Assert.Contains("spinner-border", cut.Markup);
    }


    [Fact]
    public async Task ViewContact_ShouldDisplayContactDetails_WhenContactIsFound()
    {
        // Arrange: Mock contact data
        var contact = new ContactDTO
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Addresses = new List<AddressDTO>
            {
                new AddressDTO { Street = "123 Main St", City = "New York", State = "NY", ZipCode = "10001" }
            }
        };

        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        var cut = Render<ViewContact>(parameters => parameters.Add(p => p.id, 1));

        // Assert: Verify contact info appears
        Assert.Contains("John Doe", cut.Markup);
        Assert.Contains("123 Main St", cut.Markup);
        Assert.Contains("New York", cut.Markup);
        Assert.Contains("NY", cut.Markup);
        Assert.Contains("10001", cut.Markup);
    }

    [Fact]
    public async Task ViewContact_ShouldDisplayNotFound_WhenContactIsNull()
    {
        // Arrange: Simulate contact not found
        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync((ContactDTO)null);

        var cut = Render<ViewContact>(parameters => parameters.Add(p => p.id, 1));

        // Assert: Check for 'Contact not found' message
        Assert.Contains("Contact not found", cut.Markup);
    }

    [Fact]
    public async Task ViewContact_ShouldRenderMultipleAddresses_WhenAvailable()
    {
        // Arrange: Contact with multiple addresses
        var contact = new ContactDTO
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Addresses = new List<AddressDTO>
            {
                new AddressDTO { Street = "123 Main St", City = "New York", State = "NY", ZipCode = "10001" },
                new AddressDTO { Street = "456 Elm St", City = "Los Angeles", State = "CA", ZipCode = "90001" }
            }
        };

        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        var cut = Render<ViewContact>(parameters => parameters.Add(p => p.id, 1));

        // Assert: Verify multiple addresses appear
        Assert.Contains("123 Main St", cut.Markup);
        Assert.Contains("New York", cut.Markup);
        Assert.Contains("456 Elm St", cut.Markup);
        Assert.Contains("Los Angeles", cut.Markup);
    }

    [Fact]
    public async Task ViewContact_ShouldNavigateBackToContacts_WhenBackButtonClicked()
    {
        // Arrange
        var contact = new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe" };

        mockContactService
            .Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        var cut = Render<ViewContact>(parameters => parameters.Add(p => p.id, 1));

        // Act: Click the back button
        cut.Find("button.btn-outline-secondary").Click();

        // Assert: Verify navigation
        Assert.Equal("http://localhost/contacts", navigationManager.Uri);
    }
}