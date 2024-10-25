using AutoMapper;
using EventsWebApplication.Application.Users.Registration;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using EventsWebApplication.Infrastructure;

namespace EventsWebApplication.Application.Users.Login
{
    public class UserLoginUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationService _validationService;
        public UserLoginUseCase(IPasswordHasher hasher, IJwtProvider jwtProvider,  IUnitOfWork unitOfWork, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.userRepository;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
            _validationService = validationService;
            
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest request)
        {
            await _validationService.ValidateAsync(request);
            UserEntity? user=await _repository.GetByEmail(request.Email);

            if (user == null)
            {
                throw new KeyNotFoundException("This user doesn't exist");
            }
            var result = _hasher.Verify(request.Password, user.PasswordHash);

            if (result == false)
            {
                throw new InvalidOperationException("Failed to login");
            }

            var jwtToken = _jwtProvider.GenerateToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireHours = DateTime.UtcNow.AddHours(12);

            await _repository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserLoginResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            };

        }
    }
}
