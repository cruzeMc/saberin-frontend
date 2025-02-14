using BlazorApp2.Pages;
using Bunit;
using ClassLibrary1.DTOs;
using ClassLibrary1.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class ContactsTests : TestContext
{
    [Fact]
    public async Task Contacts_ShouldLoadAndDisplayContacts()
    {
        // Arrange
        var mockService = new Mock<IContactService>();
        var fakeContacts = new List<ContactDTO>
        {
            new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe", Addresses = new List<AddressDTO>() },
            new ContactDTO { Id = 2, FirstName = "Jane", LastName = "Smith", Addresses = new List<AddressDTO>() }
        };

        mockService.Setup(s => s.GetContactsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(fakeContacts);

        Services.AddSingleton(mockService.Object);

        // Act
        var cut = Render<Contacts>();

        // Assert
        Assert.Contains("John", cut.Markup);
        Assert.Contains("Doe", cut.Markup);
        Assert.Contains("Jane", cut.Markup);
        Assert.Contains("Smith", cut.Markup);
    }

    [Fact]
    public async Task Contacts_ShouldCallService_WhenNextPageIsClicked()
    {
        // Arrange
        var mockService = new Mock<IContactService>();

        // Create 12 fake contacts to simulate pagination
        var fakeContacts = Enumerable.Range(1, 12).Select(i => new ContactDTO
        {
            Id = i,
            FirstName = $"First{i}",
            LastName = $"Last{i}",
            Addresses = new List<AddressDTO>()
        }).ToList();

        mockService.Setup(s => s.GetContactsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((int page, int pageSize) =>
                fakeContacts.Skip((page - 1) * pageSize).Take(pageSize).ToList());

        Services.AddSingleton(mockService.Object);

        var cut = Render<Contacts>();

        // Act: Click "Next" button
        cut.Find("button:contains('Next')").Click();

        // Wait for Blazor to process state updates
        await cut.InvokeAsync(() => Task.Delay(100));

        // Assert: Ensure service was called again for the second page
        mockService.Verify(s => s.GetContactsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeast(2));
    }
}