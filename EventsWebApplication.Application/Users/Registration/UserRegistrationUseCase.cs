using AutoMapper;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using EventsWebApplication.Infrastructure;

namespace EventsWebApplication.Application.Users.Registration
{
    public class UserRegistrationUseCase 
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationService _validationService;
        private readonly IJwtProvider _jwtProvider;

        public UserRegistrationUseCase(IPasswordHasher hasher,IMapper mapper, IUnitOfWork unitOfWork, ValidationService validationService, IJwtProvider jwtProvider)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.userRepository;
            _hasher = hasher;
            _mapper = mapper;
            _validationService = validationService;
            _jwtProvider = jwtProvider;

        }
        public async Task<UserRegistrationResponse> Register(UserRegistrationRequest request)
        {
            await _validationService.ValidateAsync(request);
            UserEntity user = _mapper.Map<UserEntity>(request);
            

            var hashedPassword = _hasher.Generate(request.Password);
            var refreshToken = _jwtProvider.GenerateRefreshToken();
            user.PasswordHash = hashedPassword;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireHours = DateTime.UtcNow.AddHours(12);
           
            await _repository.Create(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserRegistrationResponse
            {
                UserId = user.Id,
                RefreshToken=refreshToken,
            };

        }
    }
}
