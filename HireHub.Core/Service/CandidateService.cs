using HireHub.Core.Data.Filters;
using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.Utils.Common;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class CandidateService
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly ISaveRepository _saveRepository;
    private readonly ILogger<CandidateService> _logger;

    public CandidateService(ICandidateRepository candidateRepository,
        ISaveRepository saveRepository, ILogger<CandidateService> logger)
    {
        _candidateRepository = candidateRepository;
        _saveRepository = saveRepository;
        _logger = logger;
    }


    #region Query Services

    public async Task<Response<List<CandidateDTO>>> GetCandidates(CandidateExperienceLevel? experienceLevel,
        bool isLatestFirst, DateTime? startDate, DateTime? endDate, int? pageNumber, int? pageSize)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetCandidates));

        var filter = new CandidateFilter
        {
            ExperienceLevel = experienceLevel, IsLatestFirst = isLatestFirst,
            StartDate = startDate, EndDate = endDate,
            PageNumber = pageNumber, PageSize = pageSize
        };
        var candidates = await _candidateRepository.GetAllAsync(filter, CancellationToken.None);

        var candidateDTOs = ConverToDTO(candidates);

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetCandidates));

        return new()
        {
            Data = candidateDTOs
        };
    }

    #endregion

    #region Command Services



    #endregion

    #region Private Methods

    private List<CandidateDTO> ConverToDTO(List<Candidate> candidates)
    {
        var candidateDTOs = new List<CandidateDTO>();
        candidates.ForEach(candidate =>
        {
            var candidateDTO = Helper.Map<Candidate, CandidateDTO>(candidate);
            candidateDTOs.Add(candidateDTO);
        });
        return candidateDTOs;
    }

    #endregion
}