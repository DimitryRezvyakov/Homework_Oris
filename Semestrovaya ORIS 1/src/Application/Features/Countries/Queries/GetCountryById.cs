using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Countries.Queries
{
    public class GetCountryById : IRequest<Result<Country>>
    {
        public int Id { get; set; }

        public GetCountryById() { }
        public GetCountryById(int id)
        {
            Id = id;
        }
    }

    public class GetCountryByIdHandler : IRequestHandler<GetCountryById, Result<Country>>
    {
        private readonly IGenericRepository _repository;

        public GetCountryByIdHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Country>> Handle(GetCountryById request, CancellationToken cts)
        {
            try
            {
                var country = await _repository.GetById<Country>(request.Id);

                if (country != null)
                    return await Result<Country>.SuccessAsync(country);

                return await Result<Country>.FailureAsync($"Country with id {request.Id} was not found");
            }
            catch (Exception ex)
            {
                return await Result<Country>.FailureAsync(ex);
            }
        }
    }
}
