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
    public class GetAllResorts : IRequest<Result<List<Resort>>>
    {
    }

    public class GetAllResortsHandler : IRequestHandler<GetAllResorts, Result<List<Resort>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllResortsHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Resort>>> Handle(GetAllResorts request, CancellationToken cts)
        {
            try
            {
                var data = await _repository.GetAll<Resort>();

                if (data != null)
                    return await Result<List<Resort>>.SuccessAsync(data);

                return await Result<List<Resort>>.FailureAsync("Resorts was not found");
            }
            catch (Exception ex)
            {
                return await Result<List<Resort>>.FailureAsync(ex);
            }
        }
    }
}
