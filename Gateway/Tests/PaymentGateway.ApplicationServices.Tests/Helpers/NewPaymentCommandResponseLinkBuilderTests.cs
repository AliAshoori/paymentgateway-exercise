using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.ApplicationServices.Helpers;
using PaymentGateway.ApplicationServices.Responses;

namespace PaymentGateway.ApplicationServices.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("APIs.AppServices.Helpers")]
    public class NewPaymentCommandResponseLinkBuilderTests
    {
        [TestMethod]
        public void NewPaymentCommandResponseLinkBuilder_WithNullContextProvider_ThrowsNullException()
        {
            // Arrange
            IHttpContextProvider contextProvider = null;

            // Act
            Action init = () => new NewPaymentCommandResponseLinkBuilder(contextProvider);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(contextProvider));
        }

        [TestMethod]
        public void GenerateLinks_HappyScenario_BuildsApiDiscoveryLinks()
        {
            // Arrange
            const string baseUrl = "https://localhost";
            var mockContextAccessor = new Mock<IHttpContextProvider>();
            mockContextAccessor.Setup(m => m.GetAppBaseUrl()).Returns(baseUrl);

            var response = new NewPaymentCommandResponse
            {
                MerchantId = 1001,
                PaymentId = 303
            };

            var builder = new NewPaymentCommandResponseLinkBuilder(mockContextAccessor.Object);

            var expected = new List<ApiDiscoveryLink>
            {
                new()
                {
                    Action = "POST",
                    Href = new Uri($"{baseUrl}/merchants/{response.MerchantId}/payments"),
                    Rel = "self"
                },
                new()
                {
                    Action = "GET",
                    Href = new Uri($"{baseUrl}/merchants/{response.MerchantId}/payments/{response.PaymentId}"),
                    Rel = "get payment by id"
                }
            };

            // Act
            var links = builder.GenerateLinks(response);

            // Assert
            links.Should().BeEquivalentTo(expected);
        }

    }
}