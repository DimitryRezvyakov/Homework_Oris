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
    public class GetHotelsByResortRangeQuery : IRequest<Result<List<Hotel>>>
    {
        public List<int> Resorts { get; set; }

        public GetHotelsByResortRangeQuery() { }

        public GetHotelsByResortRangeQuery(List<int> resorts)
        {
            Resorts = resorts;
        }
    }

    public class GetHotelsByResortRangeQueryHandler : IRequestHandler<GetHotelsByResortRangeQuery, Result<List<Hotel>>>
    {
        private readonly IGenericRepository _repository;

        public GetHotelsByResortRangeQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Hotel>>> Handle(GetHotelsByResortRangeQuery request, CancellationToken cts)
        {
            try
            {
                var hotels = await _repository.GetAll<Hotel>();

                var data = hotels.Where(h => request.Resorts.Contains(h.ResortId)).ToList();

                if (data != null)
                    return await Result<List<Hotel>>.SuccessAsync(data);

                return await Result<List<Hotel>>.FailureAsync($"Hotels with Resorts Ids {request.Resorts} were not found");
            }
            catch (Exception ex)
            {
                return await Result<List<Hotel>>.FailureAsync(ex);
            }
        }
    }
}
