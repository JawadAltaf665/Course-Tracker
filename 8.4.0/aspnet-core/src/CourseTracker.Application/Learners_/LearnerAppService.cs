using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using CourseTracker.Entities;
using CourseTracker.Learners_.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseTracker.Authorization.CourseTrackerAuthorizationProvider;

namespace CourseTracker.Learners_
{
    public class LearnerAppService: ApplicationService, ILearnerAppService
    {
        private readonly IRepository<Learner, int> _learnerRepo;
        private readonly IMapper _mapper;
        public LearnerAppService(IRepository<Learner, int> learnerRepo, IMapper mapper)
        {
            _learnerRepo = learnerRepo;
            _mapper = mapper;
        }

        [AbpAuthorize(LearnerPermissions.Learners_Create)]
        public async Task CreateLearnerAsync(CreateUpdateLearnerDTO input)
        {
            var learner = new Learner
            {
                Name = input.Name,
                Email = input.Email
            };

            var learnerDto = _mapper.Map<LearnerDto>(learner);

            _learnerRepo.InsertAndGetId(learner);

            await _learnerRepo.InsertAsync(learner);
           
        }

        [AbpAuthorize(LearnerPermissions.Learners_Delete)]
        public async Task DeleteLearnerAsync(int id)
        {
            var selectedLearner = await _learnerRepo.FirstOrDefaultAsync(id);
            if (selectedLearner == null)
            {
                throw new Exception($"Learner with ID {id} not found.");
            }

            await _learnerRepo.DeleteAsync(selectedLearner);
        }

        public async Task<List<LearnerDto>> GetAllLearnerAsync()
        {
            var learners = await _learnerRepo.GetAllListAsync();

            return _mapper.Map<List<LearnerDto>>(learners);
        }

        public async Task<LearnerDto> GetLearnerByIdAsync(int id)
        {
            var selectedLearner = await _learnerRepo.FirstOrDefaultAsync(id);
            if (selectedLearner == null)
            {
                throw new UserFriendlyException($"Learner with ID {id} not found.");
            }

            var Dto = _mapper.Map<LearnerDto>(selectedLearner);

            return Dto;
        }

        /// <summary>
        /// Updates an existing learner's details based on the provided input.
        /// </summary>
        /// <param name="input">DTO containing updated learner information, including ID, name, and email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="UserFriendlyException">
        /// Thrown when the learner with the specified ID is not found in the database.
        /// </exception>
        [AbpAuthorize(LearnerPermissions.Learners_Update)]
        public async Task UpdateLearnerAsync(CreateUpdateLearnerDTO input)
        {
            var learner = await _learnerRepo.FirstOrDefaultAsync(input.Id);

            if (learner == null)
            {
                throw new UserFriendlyException($"Learner with ID {input.Id} not found.");
            }
            learner.Name = input.Name;
            learner.Email = input.Email;

            var dto = _mapper.Map<LearnerDto>(learner);

            await _learnerRepo.UpdateAsync(learner);
        }
    }
}
