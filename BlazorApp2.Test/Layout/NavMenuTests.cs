using BlazorApp2.Layout;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp2.Test.Layout;

public class NavMenuTests : TestContext
{
    [Fact]
    public void NavMenu_RendersLinksCorrectly()
    {
        // Act
        var component = Render<NavMenu>();

        // Assert - Check if navigation links exist
        var links = component.FindAll("a.nav-link");

        Assert.Equal(2, links.Count);
        Assert.Contains("/contacts", links[0].GetAttribute("href"));
        Assert.Contains("/createcontact", links[1].GetAttribute("href"));
        Assert.Equal("Contacts", links[0].TextContent);
        Assert.Equal("Create Contact", links[1].TextContent);
    }

    [Fact]
    public void NavMenu_HighlightsActiveLink()
    {
        // Act - Set the navigation to `/contacts`
        var navContext = Services.GetRequiredService<NavigationManager>();
        navContext.NavigateTo("/contacts");

        var component = Render<NavMenu>();

        // Assert - "Contacts" link should have "active" class
        var activeLink = component.Find("a[href='/contacts']");
        Assert.Contains("active", activeLink.ClassList.ToString());
    }
}