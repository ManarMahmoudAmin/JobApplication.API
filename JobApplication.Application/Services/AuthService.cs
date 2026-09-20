using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobApplication.Application.Services
{
    public class AuthService : IAuthService
    {
            private readonly IGenericRepository<Candidate> _candidateRepository;
            private readonly IGenericRepository<Recruiter> _recruiterRepository;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ITokenService _tokenService;

            public AuthService(
                IGenericRepository<Candidate> candidateRepository,
                IGenericRepository<Recruiter> recruiterRepository,
                UserManager<ApplicationUser> userManager,
                ITokenService tokenService)
            {
                _candidateRepository = candidateRepository;
                _recruiterRepository = recruiterRepository;
                _userManager = userManager;
                _tokenService = tokenService;
            }


        public async Task RegisterAsync(RegisterDto registerDto)
        {
            //var candidate = new Candidate
            //{
            //    Name = dto.Name,
            //    CvUrl = dto.CvUrl,
            //};

            //await _candidateRepository.AddAsync(candidate);
            //await _candidateRepository.SaveChangesAsync();

            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                //CandidateId = candidate.Id
            };

            //Add Recruiter Role + Candidate Role
            if (registerDto.Role == "Candidate")
            {
                var candidate = new Candidate
                {
                    Name = registerDto.Name,
                    CvUrl = registerDto.CvUrl
                };

                await _candidateRepository.AddAsync(candidate);
                await _candidateRepository.SaveChangesAsync();

                user.CandidateId = candidate.Id;
            }
            else if (registerDto.Role == "Recruiter")
            {
                var recruiter = new Recruiter
                {
                    Name = registerDto.Name
                };

                await _recruiterRepository.AddAsync(recruiter);
                await _recruiterRepository.SaveChangesAsync();

                user.RecruiterId = recruiter.Id;
            }
            else
            {
                throw new InvalidOperationException("Invalid role.");
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }
            await _userManager.AddToRoleAsync(user, registerDto.Role);

        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return null;

            var validPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!validPassword)
                return null;

            return await _tokenService.CreateTokenAsync(user, _userManager);
        }
    }
}