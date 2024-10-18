using AutoMapper;
using EventsWebApplication.Application.Events.UseCases.UploadImage;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EventsWebApplication.Core.Abstractions;

namespace EventsWebApplication.Tests.EventsUseCases
{
    public class UploadImageTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly UploadImageUseCase _uploadImageUseCase;

        public UploadImageTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _fileServiceMock = new Mock<IFileService>();
            _validationServiceMock = new Mock<IValidationService>();

            _unitOfWorkMock.Setup(u => u.eventRepository).Returns(_eventRepositoryMock.Object);
            _uploadImageUseCase = new UploadImageUseCase(_unitOfWorkMock.Object, new Mock<IMapper>().Object, _fileServiceMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task UploadImage_ShouldUploadImage_WhenRequestIsValid()
        {
            var request = new UploadImageRequest
            {
                Id = Guid.NewGuid(),
                File = new Mock<IFormFile>().Object 
            };

            var eventEntity = new EventEntity
            {
                Id = request.Id,
                EventImage = "oldImage.png"
            };

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id)).ReturnsAsync(eventEntity);
            _fileServiceMock.Setup(fs => fs.SaveFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("newImage.png");

            await _uploadImageUseCase.UploadImage(request);

            _eventRepositoryMock.Verify(repo => repo.UploadImage(request.Id, "newImage.png"), Times.Once);
            _fileServiceMock.Verify(fs => fs.DeleteFile(Path.Combine("wwwroot", "images", eventEntity.EventImage)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UploadImage_ShouldThrowKeyNotFoundException_WhenEventDoesNotExist()
        {
            var request = new UploadImageRequest
            {
                Id = Guid.NewGuid(),
                File = new Mock<IFormFile>().Object
            };

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id)).ReturnsAsync((EventEntity?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _uploadImageUseCase.UploadImage(request));
        }

        [Fact]
        public async Task UploadImage_ShouldThrowArgumentNullException_WhenFileIsNull()
        {
            var request = new UploadImageRequest
            {
                Id = Guid.NewGuid(),
                File = null 
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _uploadImageUseCase.UploadImage(request));
        }
    }
}
