using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.ApplicationServices.Helpers;

namespace PaymentGateway.ApplicationServices.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.AppServices.Helpers")]
    public class HttpContextProviderTests
    {
        [TestMethod]
        public void GetAppBaseUrl_HappyScenario_ReturnsAppBaseUrl()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = new HostString("localhost");
            context.Request.PathBase = new PathString("/payments");
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.Setup(m => m.HttpContext).Returns(context);

            var provider = new HttpContextProvider(mockContextAccessor.Object);

            var expected = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";

            // Act
            var result = provider.GetAppBaseUrl();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void HttpContextProvider_WithNullContextAccessor_ThrowsArgumentNUllException()
        {
            // Arrange
            IHttpContextAccessor accessor = null;

            // Act
            Action init = () => new HttpContextProvider(accessor);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(accessor));
        }
    }
}
