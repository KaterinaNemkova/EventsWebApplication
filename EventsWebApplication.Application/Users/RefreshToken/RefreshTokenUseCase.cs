﻿
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using EventsWebApplication.Infrastructure;
using System.Security.Claims;

namespace EventsWebApplication.Application.Users.RefreshToken
{
    public class RefreshTokenUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationService _validationService;
        public RefreshTokenUseCase(IJwtProvider jwtProvider, IUnitOfWork unitOfWork, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.userRepository;
            _jwtProvider = jwtProvider;
            _validationService = validationService;
        }

        public async Task<RefreshTokenResponse> Refresh(RefreshTokenRequest request)
        {
            await _validationService.ValidateAsync(request);
            ClaimsPrincipal? principal = _jwtProvider.GetClaimsPrincipal(request.JwtToken);

            Claim? userIdClaim = principal?.Claims?.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new Exception("Invalid data in token");
            }

            UserEntity? user = await _repository.GetByIdAsync(userId) ?? throw new KeyNotFoundException("Invalid user from token");
            if (user.RefreshToken == null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpireHours < DateTime.UtcNow)
            {
                user.RefreshToken = "";
                user.RefreshTokenExpireHours = null;

                await _repository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                throw new Exception("Refresh token is not valid");
            }

            string newJwtToken = _jwtProvider.GenerateToken(user);
            string newRefreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpireHours = DateTime.UtcNow.AddHours(5);

            await _repository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new RefreshTokenResponse
            {
                JwtToken = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }

    }
}