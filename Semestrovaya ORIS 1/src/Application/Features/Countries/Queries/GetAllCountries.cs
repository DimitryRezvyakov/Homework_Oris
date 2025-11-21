using Application.Entities;
using Application.Services.Repositories;
using Mediator.Interfaces;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Countries.Queries
{
    public class GetAllCountries : IRequest<Result<List<Country>>>
    {

    }

    public class GetAllCountriesHandler : IRequestHandler<GetAllCountries, Result<List<Country>>>
    {
        private readonly IGenericRepository _repository;
        public GetAllCountriesHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Country>>> Handle(GetAllCountries request, CancellationToken cts)
        {
            try
            {
                var data = await _repository.GetAll<Country>();

                if (data != null)
                    return await Result<List<Country>>.SuccessAsync(data);

                return await Result<List<Country>>.FailureAsync("Countries was null");
            }
            catch (Exception ex)
            {
                return await Result<List<Country>>.FailureAsync(ex);
            }
        }
    }
}
