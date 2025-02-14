using BlazorApp2.Layout;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

public class LayoutTests : TestContext
{
    [Fact]
    public void Footer_DisplaysCurrentYear()
    {
        // Arrange
        var currentYear = DateTime.Now.Year.ToString();

        // Act
        var component = Render<MainLayout>();

        // Assert - Check if footer contains the correct year
        var footer = component.Find("footer");
        Assert.Contains(currentYear, footer.TextContent);
        Assert.Contains("Contact App", footer.TextContent);
    }

    [Fact]
    public void Layout_RendersNavMenu()
    {
        // Act
        var component = Render<MainLayout>();

        // Assert - Check if <NavMenu> exists in the header
        var header = component.Find("header");
        Assert.NotNull(header);
        Assert.NotEmpty(header.InnerHtml); // Ensures something is inside the header
    }

    [Fact]
    public void Layout_RendersBodyContent()
    {
        // Arrange - Wrap the layout in a test component with child content
        var component = Render<LayoutTestWrapper>();

        // Assert - Check if body contains the expected content
        var mainContent = component.Find("main");
        Assert.Contains("Test Content", mainContent.InnerHtml);
    }

// A simple wrapper to inject body content into the layout
    public class LayoutTestWrapper : LayoutComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<MainLayout>(0);
            builder.AddAttribute(1, "Body",
                (RenderFragment)(builder2 => { builder2.AddMarkupContent(2, "<p>Test Content</p>"); }));
            builder.CloseComponent();
        }
    }
}