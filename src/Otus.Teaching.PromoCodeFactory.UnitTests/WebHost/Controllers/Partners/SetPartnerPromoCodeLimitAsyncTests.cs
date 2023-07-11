using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Otus.Teaching.PromoCodeFactory.Domain.Models;
using System.Linq;
using Otus.Teaching.PromoCodeFactory.UnitTests.Data;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

                
        /// <summary>
        /// Если партнер не найден, то также нужно выдать ошибку 404;
        /// </summary>
        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            Partner partner = null;

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            // Act
            var actResult = await _partnersController.SetPartnerPromoCodeLimitAsync(Guid.NewGuid(), request);

            // Assert

            actResult.Should().BeAssignableTo<NotFoundResult>();
        }

        /// <summary>
        /// Если партнер заблокирован, то есть поле IsActive=false в классе Partner, то также нужно выдать ошибку 400;
        /// </summary>
        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_PartnerIsNotActive_ReturnsBadRequest()
        {
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            var partner = PartnersBuilder
                .CreateBasePartner();
            partner.IsActive = false;

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partner.Id))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        /// <summary>
        /// Если партнеру выставляется лимит, то мы должны обнулить количество промо-кодов, которые партнер выдал NumberIssuedPromoCodes, если лимит не закончился
        /// </summary>
        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_PartnerSetLimit_ZeroNumberIssuedPromoCodes()
        {
            // Arrange
            var partner = PartnersBuilder
                .CreateBasePartner()
                .SetActiveLimit();

            var partnerId = partner.Id;
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();
            
            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);
           
            // Assert
            partner.NumberIssuedPromoCodes.Should().Be(0);
        }

        /// <summary>
        /// Если партнеру выставляется лимит, то мы должны обнулить количество промо-кодов, которые партнер выдал NumberIssuedPromoCodes, если лимит закончился, то количество не обнуляется
        /// </summary>
        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_PartnerSetLimit_NotZeroNumberIssuedPromoCodes()
        {
            // Arrange
            var partner = PartnersBuilder
                .CreateBasePartner();

            partner.NumberIssuedPromoCodes = 10;

            var partnerId = partner.Id;
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            partner.NumberIssuedPromoCodes.Should().NotBe(0);
        }

        /// <summary>
        /// При установке лимита нужно отключить предыдущий лимит
        /// </summary>
        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_HasActiveLimit_ShouldSetCancelDateNow()
        {
            var partner = PartnersBuilder
                .CreateBasePartner()
                .SetActiveLimit();

            var targetLimit = partner.PartnerLimits.First();
            var partnerId = partner.Id;
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();
            var now = DateTime.Now.AddMinutes(-15);

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);


            var act = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            targetLimit.CancelDate.Should().HaveValue();
        }

        /// <summary>
        /// Лимит должен быть больше 0, иначе ошибка
        /// </summary>
        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_PartnerSetNotOverZeroLimit_ReturnsBadRequest()
        {
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            request.Limit = -100;

            var partner = PartnersBuilder.CreateBasePartner();

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partner.Id))
                .ReturnsAsync(partner);


            var actResult = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);


            actResult.Should().BeAssignableTo<BadRequestResult>();
        }

        //Нужно убедиться, что сохранили новый лимит в базу данных (это нужно проверить Unit-тестом);
        //Если в текущей реализации найдутся ошибки, то их нужно исправить и желательно написать тест, чтобы они больше не повторялись.

        [Fact]
        public async void SetPartnerPromoCodeLimitAsyncTests_ValidSave_SuccessUpdate()
        {

            var partner = PartnersBuilder.CreateBasePartner();
            var partnerId = partner.Id;
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();
            
            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);


            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);


            _partnersRepositoryMock.Verify(repo => repo.UpdateAsync(partner), Times.Once);
        }
    }
}