using BlazorApp2.Pages;
using Bunit;
using ClassLibrary1.DTOs;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorApp2.Test.Pages;

public class CreateContactTests : TestContext
{
    private readonly Mock<IContactService> mockContactService;
    private NavigationManager navigationManager; // ✅ Use bUnit's fake NavigationManager

    public CreateContactTests()
    {
        // Setup mock service
        mockContactService = new Mock<IContactService>();

        // Register dependencies
        Services.AddSingleton(mockContactService.Object);

        // ✅ Retrieve bUnit’s fake NavigationManager
        navigationManager = Services.GetRequiredService<NavigationManager>();
    }

    [Fact]
    public void CreateContact_ShouldRenderForm()
    {
        // Arrange & Act
        var cut = Render<CreateContact>();

        // Assert: Ensure form fields exist
        Assert.NotNull(cut.Find("#firstName"));
        Assert.NotNull(cut.Find("#lastName"));
        Assert.NotNull(cut.Find("#street"));
        Assert.NotNull(cut.Find("#city"));
        Assert.NotNull(cut.Find("#state"));
        Assert.NotNull(cut.Find("#zip"));
    }

    [Fact]
    public async Task CreateContact_ShouldCallService_OnValidSubmit()
    {
        // Arrange
        var cut = Render<CreateContact>();

        // Fill form fields
        cut.Find("#firstName").Change("John");
        cut.Find("#lastName").Change("Doe");
        cut.Find("#street").Change("123 Main St");
        cut.Find("#city").Change("Springfield");
        cut.Find("#state").Change("IL");
        cut.Find("#zip").Change("62704");

        // Mock service response
        var fakeContact = new ContactDTO 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe", 
            Addresses = new List<AddressDTO> 
            { 
                new() { Street = "123 Main St", City = "Springfield", State = "IL", ZipCode = "62704" } 
            } 
        };
        
        mockContactService
            .Setup(s => s.CreateContactAsync(It.IsAny<ContactDTO>()))
            .ReturnsAsync(fakeContact);

        // Act: Submit form
        await cut.Find("form").SubmitAsync();

        // Assert: Verify service call
        mockContactService.Verify(s => s.CreateContactAsync(It.Is<ContactDTO>(
            c => c.FirstName == "John" &&
                 c.LastName == "Doe" &&
                 c.Addresses.Count == 1 &&
                 c.Addresses[0].Street == "123 Main St" &&
                 c.Addresses[0].City == "Springfield" &&
                 c.Addresses[0].State == "IL" &&
                 c.Addresses[0].ZipCode == "62704"
        )), Times.Once);

        // ✅ Verify navigation happened
        Assert.Equal("/contacts", new Uri(navigationManager.Uri).AbsolutePath);
    }

    [Fact]
    public async Task CreateContact_ShouldShowError_WhenServiceFails()
    {
        // Arrange
        var cut = Render<CreateContact>();

        // Fill form fields
        cut.Find("#firstName").Change("John");
        cut.Find("#lastName").Change("Doe");
        cut.Find("#street").Change("123 Main St");
        cut.Find("#city").Change("Springfield");
        cut.Find("#state").Change("IL");
        cut.Find("#zip").Change("62704");

        // Mock service failure
        mockContactService
            .Setup(s => s.CreateContactAsync(It.IsAny<ContactDTO>()))
            .ReturnsAsync((ContactDTO)null);

        // Act: Submit form
        await cut.Find("form").SubmitAsync();


        // ✅ Assert: Ensure the error message div exists and contains expected text
        var errorDiv = cut.Find("div.text-danger");
        Assert.Contains("Failed to create contact. Please try again.", errorDiv.TextContent);
    }

    [Fact]
    public void CreateContact_ShouldNavigateBack_WhenCancelIsClicked()
    {
        // Arrange
        var cut = Render<CreateContact>();

        // Act: Click cancel button
        cut.Find("button.btn-secondary").Click();

        // ✅ Verify navigation happened
        Assert.Equal("/contacts", new Uri(navigationManager.Uri).AbsolutePath);
    }
}
