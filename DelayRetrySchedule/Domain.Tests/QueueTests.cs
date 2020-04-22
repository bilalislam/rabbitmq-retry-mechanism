using System;
using Bogus;
using Domain.Dtos;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Tests
{
    [TestFixture]
    public class QueueTests
    {
        private static readonly Faker Faker = new Faker();

        [Test]
        public void Load_ThrowIfDomainInvalidException_WhenQueueNameIsEmpty()
        {
            //Arrange
            var exchangeDto = new ExchangeDto()
            {
                ExhangeName = Faker.Random.String(),
                ExchangeType = Faker.Random.String()
            };

            Action queue = () => { Queue.Load(string.Empty, exchangeDto); };

            //Act & Assert
            queue.Should().Throw<Exception>().And.Message.Should().Be("queue name should not be empty");
        }

        [Test]
        public void Load_ThrowIfDomainInvalidException_WhenExchangeIsNull()
        {
            //Arrange
            Action queue = () => { Queue.Load(Faker.Random.ToString(), null); };

            //Act & Assert
            queue.Should().Throw<Exception>().And.Message.Should().Be("exchange should not be null");
        }

        [Test]
        public void Load_Success_WhenDomainIsValid()
        {
            //Arrange
            var exchangeDto = new ExchangeDto()
            {
                ExhangeName = Faker.Random.String(),
                ExchangeType = Faker.Random.String()
            };

            //Act
            Action queue = () => { Queue.Load(Faker.Random.ToString(), exchangeDto); };

            //Assert
            queue.Should().NotBeNull();
        }
    }
}