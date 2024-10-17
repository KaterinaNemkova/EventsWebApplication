using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;

namespace EventsWebApplication.Application.Members.UseCases.GetMemberById
{
    public class GetMemberByIdUseCase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        private readonly ValidationService _validationService;
        public GetMemberByIdUseCase(IMapper mapper,IUnitOfWork unitOfWork, ValidationService validationService)
        {
            _memberRepository = unitOfWork.memberRepository;
            _mapper = mapper;
            _validationService = validationService;
        }
        public async Task<MemberDto> GetById(GetMemberByIdRequest request)
        {
            await _validationService.ValidateAsync(request);    
            var memberEntity = await _memberRepository.GetByIdAsync(request.Id);

            if (memberEntity is null)
            {
                throw new KeyNotFoundException("Member not found.");
            }

            var member = _mapper.Map<MemberDto>(memberEntity);
            return member;
        }
    }
}
