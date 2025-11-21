using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Resorts.Queries
{
    public class GetAllResortsByCountryIdQuery : IRequest<Result<List<Resort>>>
    {
        public int CountryId { get; set; }

        public GetAllResortsByCountryIdQuery() { }

        public GetAllResortsByCountryIdQuery(int id) { CountryId = id; }
    }

    public class GetAllResortsByCountryIdQueryHandler : IRequestHandler<GetAllResortsByCountryIdQuery, Result<List<Resort>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllResortsByCountryIdQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Resort>>> Handle(GetAllResortsByCountryIdQuery request, CancellationToken cts)
        {
            try
            {
                var resorts = _repository.Entities<Resort>();

                var data = resorts.Where(r => r.CountryId == request.CountryId).ToList();

                if (data != null)
                    return await Result<List<Resort>>.SuccessAsync(data);

                return await Result<List<Resort>>.FailureAsync($"Resorts with CountryId {request.CountryId} were not found");
            }
            catch (Exception ex)
            {
                return await Result<List<Resort>>.FailureAsync(ex);
            }
        }
    }
}
