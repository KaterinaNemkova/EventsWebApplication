using AutoMapper;
using EventsWebApplication.Core.Models;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.Infrastructure;


namespace EventsWebApplication.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IPasswordHasher hasher, IJwtProvider jwtProvider,IMapper mapper)
        {
            _repository = repo;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
            _mapper = mapper;

        }

        public async Task Register(string name, string email, string password)
        {
            var hashedPassword = _hasher.Generate(password);

            var user = User.Create
            (
                Guid.NewGuid(),
                name,
                email,
                hashedPassword
            );
            await _repository.Add(user);

        }

        public async Task<string> Login(string email, string password)
        {
            var userEntity=await _repository.GetByEmail(email);

            var user=_mapper.Map<User>(userEntity);

            if(user== null)
            {
                throw new Exception("This user doesn't exist");
            }

            var result=_hasher.Verify(password, user.PasswordHash);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

        public Task<bool> AlreadyExist(Guid userId)
        {
            var userExist = _repository.AlreadyExist(userId);

            return userExist;
        }
    }
}
