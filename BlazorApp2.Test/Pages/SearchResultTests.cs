using BlazorApp2.Pages;
using Bunit;
using ClassLibrary1.DTOs;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class SearchResultsTests : TestContext
{
    private readonly Mock<IContactService> mockContactService;
    private readonly NavigationManager navigationManager;

    public SearchResultsTests()
    {
        mockContactService = new Mock<IContactService>();
        Services.AddSingleton(mockContactService.Object);
        navigationManager = Services.GetRequiredService<NavigationManager>(); // bUnit's fake NavigationManager
    }

    [Fact]
    public async Task SearchResults_ShouldDisplayLoadingMessage_Initially()
    {
        // Arrange: Do not return results immediately
        mockContactService
            .Setup(s => s.SearchContactAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((IEnumerable<ContactDTO>)null);

        // Set the query parameter using NavigationManager
        var uri = navigationManager.GetUriWithQueryParameter("query", "John");
        navigationManager.NavigateTo(uri);

        // Act
        var cut = Render<SearchResults>();

        // Assert: Ensure the expected heading and "Loading results..." message are displayed
        cut.MarkupMatches(@"
        <h2>Search Results for ""John""</h2>
        <p>Loading results...</p>
    ");
    }


    [Fact]
    public async Task SearchResults_ShouldDisplayNoContactsMessage_WhenNoResultsFound()
    {
        // Arrange: Return an empty list
        mockContactService
            .Setup(s => s.SearchContactAsync("John", It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<ContactDTO>());

        // Set the query parameter using NavigationManager
        var uri = navigationManager.GetUriWithQueryParameter("query", "John");
        navigationManager.NavigateTo(uri);

        // Act
        var cut = Render<SearchResults>();

        // Assert: Ensure the "No contacts found" message appears
        cut.MarkupMatches(@"<h2>Search Results for ""John""</h2>
                        <p>No contacts found matching ""John"".</p>");
    }


    [Fact]
    public async Task SearchResults_ShouldDisplayResults_WhenContactsAreFound()
    {
        // Arrange
        var contacts = new List<ContactDTO>
        {
            new ContactDTO
            {
                Id = 1, FirstName = "John", LastName = "Doe",
                Addresses = new List<AddressDTO>
                    { new AddressDTO { City = "New York", State = "NY", ZipCode = "10001" } }
            },
            new ContactDTO
            {
                Id = 2, FirstName = "Jane", LastName = "Smith",
                Addresses = new List<AddressDTO>
                    { new AddressDTO { City = "Los Angeles", State = "CA", ZipCode = "90001" } }
            }
        };

        mockContactService
            .Setup(s => s.SearchContactAsync("John", It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(contacts);

        // Set the query parameter using NavigationManager
        var uri = navigationManager.GetUriWithQueryParameter("query", "John");
        navigationManager.NavigateTo(uri);

        // Act
        var cut = Render<SearchResults>();

        // Assert: Ensure both contacts are rendered in the table
        var markup = cut.Markup;
        Assert.Contains("John", markup);
        Assert.Contains("Doe", markup);
        Assert.Contains("New York", markup);
        Assert.Contains("Jane", markup);
        Assert.Contains("Smith", markup);
        Assert.Contains("Los Angeles", markup);
    }


    [Fact]
    public async Task SearchResults_ShouldNavigateToContactView_WhenViewButtonIsClicked()
    {
        // Arrange
        var contacts = new List<ContactDTO>
        {
            new ContactDTO
            {
                Id = 1, FirstName = "John", LastName = "Doe",
                Addresses = new List<AddressDTO>
                    { new AddressDTO { City = "New York", State = "NY", ZipCode = "10001" } }
            }
        };

        mockContactService
            .Setup(s => s.SearchContactAsync("John", It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(contacts);

        // Set the query parameter using NavigationManager
        var uri = navigationManager.GetUriWithQueryParameter("query", "John");
        navigationManager.NavigateTo(uri);

        // Act
        var cut = Render<SearchResults>();
        cut.Find("button.btn-primary").Click();

        // Assert: Ensure navigation to the correct contact view
        Assert.Equal("http://localhost/viewcontact/1", navigationManager.Uri);
    }

    [Fact]
    public async Task SearchResults_ShouldDisablePreviousButton_OnFirstPage()
    {
        // Arrange: Provide at least one page of contacts (pagination UI should exist)
        var contacts = Enumerable.Range(1, 12).Select(i => new ContactDTO
        {
            Id = i,
            FirstName = $"First{i}",
            LastName = $"Last{i}",
            Addresses = new List<AddressDTO>
            {
                new AddressDTO { City = $"City{i}", State = "ST", ZipCode = $"0000{i}" }
            }
        }).ToList();

        mockContactService
            .Setup(s => s.SearchContactAsync(It.IsAny<string>(), 1, It.IsAny<int>()))
            .ReturnsAsync(contacts);

        // Navigate to the search page with query param
        var uri = navigationManager.GetUriWithQueryParameter("query", "John");
        navigationManager.NavigateTo(uri);

        var cut = Render<SearchResults>();

        // Act: Find the "Previous" button safely
        var previousButton = cut.FindAll("button.btn-secondary").FirstOrDefault();

        // Assert: Ensure the button exists and is disabled
        Assert.NotNull(previousButton);
        Assert.True(previousButton.HasAttribute("disabled"), "Previous button should be disabled on the first page.");
    }


    [Fact]
    public async Task SearchResults_ShouldDisableNextButton_WhenLessThanPageSizeResults()
    {
        // Arrange: Return fewer results than the page size (10)
        var contacts = new List<ContactDTO>
        {
            new ContactDTO
            {
                Id = 1, FirstName = "John", LastName = "Doe",
                Addresses = new List<AddressDTO>
                    { new AddressDTO { City = "New York", State = "NY", ZipCode = "10001" } }
            }
        };

        mockContactService
            .Setup(s => s.SearchContactAsync(It.IsAny<string>(), It.IsAny<int>(), 10))
            .ReturnsAsync(contacts);

        // Set the query parameter using NavigationManager
        var uri = navigationManager.GetUriWithQueryParameter("query", "John");
        navigationManager.NavigateTo(uri);

        // Act
        var cut = Render<SearchResults>();

        // Assert: The "Next" button should be disabled
        var nextButton = cut.Find("button.btn-secondary:last-of-type");
        Assert.True(nextButton.HasAttribute("disabled"));
    }
}