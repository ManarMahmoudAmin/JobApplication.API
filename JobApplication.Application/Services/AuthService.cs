using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobApplication.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Candidate> _candidateRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            IGenericRepository<Candidate> candidateRepository,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _candidateRepository = candidateRepository;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var candidate = new Candidate
            {
                Name = dto.Name,
                CvUrl = dto.CvUrl,
            };

            await _candidateRepository.AddAsync(candidate);
            await _candidateRepository.SaveChangesAsync();

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email.Split('@')[0],
                CandidateId = candidate.Id
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return null;

            var validPassword = await _userManager.CheckPasswordAsync(
                user,
                dto.Password);

            if (!validPassword)
                return null;

            return await _tokenService.CreateTokenAsync(
                user,
                _userManager);
        }
    }
}