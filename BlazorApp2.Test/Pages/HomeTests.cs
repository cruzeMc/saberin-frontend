using BlazorApp2.Pages;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp2.Test.Pages;

public class HomeTests : TestContext
{
    [Fact]
    public void SearchButton_NavigatesToSearchPage()
    {
        // Arrange - Use bUnit's built-in TestNavigationManager
        var navigationManager = Services.GetRequiredService<NavigationManager>();

        var cut = Render<Home>();
        var input = cut.Find("input");
        var button = cut.Find("button");

        // Act
        input.TriggerEvent("oninput", new ChangeEventArgs { Value = "John2 Doe" }); // ✅ Use "oninput"
        button.Click(); // Click the search button

        // Extract only the relative path from the full URI
        var relativeUri = new Uri(navigationManager.Uri).PathAndQuery;

        // Assert - Verify navigation occurred
        Assert.Equal("/search?query=John2%20Doe", relativeUri);
    }


    [Fact]
    public void EnterKeyPress_NavigatesToSearchPage()
    {
        // Arrange - Use bUnit's built-in TestNavigationManager
        var navigationManager = Services.GetRequiredService<NavigationManager>();

        var cut = Render<Home>();
        var input = cut.Find("input");

        // Act
        input.TriggerEvent("oninput", new ChangeEventArgs { Value = "Jane Doe" }); // ✅ Use "oninput"
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        // Extract only the relative path from the full URI
        var relativeUri = new Uri(navigationManager.Uri).PathAndQuery;

        // Assert - Verify navigation occurred
        Assert.Equal("/search?query=Jane%20Doe", relativeUri);
    }

}