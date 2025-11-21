using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotels.Queries
{
    public class GetAllHotelsQuery : IRequest<Result<List<Hotel>>>
    {

    }

    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, Result<List<Hotel>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllHotelsQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Hotel>>> Handle(GetAllHotelsQuery request, CancellationToken cts)
        {
            try
            {
                var data = await _repository.GetAll<Hotel>();

                if (data != null)
                    return  await Result<List<Hotel>>.SuccessAsync(data);

                return await Result<List<Hotel>>.FailureAsync("Hotels were not found");
            }
            catch (Exception ex)
            {
                return await Result<List<Hotel>>.FailureAsync(ex);
            }
        }
    }
}
