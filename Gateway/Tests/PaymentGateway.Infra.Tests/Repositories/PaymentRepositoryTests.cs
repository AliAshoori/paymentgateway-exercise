using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Domain.Models;
using PaymentGateway.Infrastructure.Context;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Infra.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("Infra.Repositories")]
    public class PaymentRepositoryTests
    {
        private AppDbContext _context;
        private readonly long _merchantId = 101;

        [TestInitialize]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "payment_api").Options;
            _context = new AppDbContext(options);
        }

        [TestCleanup]
        public void TearDown() => _context?.Dispose();

        [TestMethod]
        public async Task PersistPaymentAsync_HappyScenario_ReturnsPaymentId()
        {
            // Arrange
            var payment = new Payment
            {
                MerchantId = _merchantId,
                Amount = 33.44m,
                Currency = "GBP",
                CardNumber = "1111 2222 3333 4444",
                CardCvv = "111",
                CardExpiry = DateTime.Now.AddDays(365),
                Succeeded = true
            };

            var repository = new PaymentRepository(_context);

            // Act
            var result = await repository.PersistPaymentAsync(payment);

            // Assert
            var persistedPayment = await _context.Payments.LastAsync();
            persistedPayment.Id.Should().Be(result);

            await CleanupPaymentAsync(payment);
        }

        [TestMethod]
        public void PaymentRepository_WithNullContext_ThrowsArgumentNullException()
        {
            // Arrange
            AppDbContext context = null;

            // Act
            Action init = () => new PaymentRepository(context);

            // Assert
            init.Should().ThrowExactly<ArgumentNullException>(nameof(context));
        }

        [TestMethod]
        public async Task GetMerchantPaymentById_HappyScenario_ReturnsPayment()
        {
            // Arrange
            var payment = new Payment
            {
                MerchantId = 3300,
                CreatedAt = DateTime.Now,
                CardNumber = "1233 1233 1233 1233",
                CardCvv = "123",
                CardExpiry = DateTime.Now.AddMonths(8),
                Currency = "GBP",
                Amount = 345.1234m,
                Succeeded = true
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var desiredPayment =
                await new PaymentRepository(_context).GetMerchantPaymentById(payment.MerchantId, payment.Id);

            // Assert
            desiredPayment.Should().BeEquivalentTo(payment);

            await CleanupPaymentAsync(payment);
        }

        [TestMethod]
        public async Task GetMerchantPaymentById_WithPaymentNotExists_ReturnsNull()
        {
            // Arrange
            var payment = new Payment
            {
                MerchantId = 3300,
                CreatedAt = DateTime.Now,
                CardNumber = "1233 1233 1233 1233",
                CardCvv = "123",
                CardExpiry = DateTime.Now.AddMonths(8),
                Currency = "GBP",
                Amount = 345.1234m,
                Succeeded = true
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var desiredPayment =
                await new PaymentRepository(_context).GetMerchantPaymentById(payment.MerchantId, 1000);

            // Assert
            desiredPayment.Should().BeNull();

            await CleanupPaymentAsync(payment);
        }


        [TestMethod]
        public async Task GetMerchantPaymentById_WithPaymentNotBelongToMerchant_ReturnsNull()
        {
            // Arrange
            var payment = new Payment
            {
                MerchantId = 3300,
                CreatedAt = DateTime.Now,
                CardNumber = "1233 1233 1233 1233",
                CardCvv = "123",
                CardExpiry = DateTime.Now.AddMonths(8),
                Currency = "GBP",
                Amount = 345.1234m,
                Succeeded = true
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var desiredPayment =
                await new PaymentRepository(_context).GetMerchantPaymentById(999, payment.Id);

            // Assert
            desiredPayment.Should().BeNull();

            await CleanupPaymentAsync(payment);
        }

        //

        private async Task CleanupPaymentAsync(Payment payment)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}
