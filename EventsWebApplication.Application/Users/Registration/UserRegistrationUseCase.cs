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
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationService _validationService;

        public UserRegistrationUseCase(IPasswordHasher hasher, IJwtProvider jwtProvider,IMapper mapper, IUnitOfWork unitOfWork, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.userRepository;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _validationService = validationService;

        }
        public async Task<UserRegistrationResponse> Register(UserRegistrationRequest request)
        {
            await _validationService.ValidateAsync(request);
            UserEntity user = _mapper.Map<UserEntity>(request);
            

            var hashedPassword = _hasher.Generate(request.Password);
            var jwtToken = _jwtProvider.GenerateToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            user.PasswordHash = hashedPassword;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireHours= DateTime.UtcNow.AddHours(5);

            await _repository.Create(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserRegistrationResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            };

        }
    }
}
