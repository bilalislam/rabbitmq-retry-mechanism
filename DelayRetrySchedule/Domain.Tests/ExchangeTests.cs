using System;
using Bogus;
using Domain.Dtos;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Tests
{
    public class ExchangeTests
    {
        private static readonly Faker Faker = new Faker();

        [Test]
        public void Load_ThrowIfDomainInvalidException_WhenExhangeNameIsEmpty()
        {
            //Arrange
            Action queue = () => { Exchange.Load(string.Empty, Faker.Random.String()); };

            //Act & Assert
            queue.Should().Throw<Exception>().And.Message.Should().Be("exchange name should not be empty");
        }

        [Test]
        public void Load_ThrowIfDomainInvalidException_WhenExchangeTypeIsEmpty()
        {
            //Arrange
            Action queue = () => { Exchange.Load(Faker.Random.ToString(), string.Empty); };

            //Act & Assert
            queue.Should().Throw<Exception>().And.Message.Should().Be("exchange type should not be empty");
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
            Action exchange = () => { Exchange.Load(Faker.Random.ToString(), Faker.Random.String()); };

            //Assert
            exchange.Should().NotBeNull();
        }
    }
}