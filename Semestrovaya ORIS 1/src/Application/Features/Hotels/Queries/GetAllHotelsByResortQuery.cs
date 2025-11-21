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
    public class GetAllHotelsByResortQuery : IRequest<Result<List<Hotel>>>
    {
        public int ResortId { get; set; }

        public GetAllHotelsByResortQuery() { }

        public GetAllHotelsByResortQuery(int resortId)
        {
            ResortId = resortId;
        }
    }

    public class GetAllHotelsByResortQueryHandler : IRequestHandler<GetAllHotelsByResortQuery, Result<List<Hotel>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllHotelsByResortQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Hotel>>> Handle(GetAllHotelsByResortQuery request, CancellationToken cts)
        {
            try
            {
                var sql = "SELECT * FROM \"Hotel\" as h WHERE h.\"ResortId\" = @resortId";

                var parameters = new Dictionary<string, object>
                {
                    ["resortId"] = request.ResortId
                };

                var data = await _repository.UseSqlCommandQueryCollection<Hotel>(sql, parameters);

                if (data?.Count > 0)
                    return await Result<List<Hotel>>.SuccessAsync(data);

                return await Result<List<Hotel>>.FailureAsync($"Hotels with ResortId {request.ResortId} were not found");
            }
            catch (Exception ex)
            {
                return Result<List<Hotel>>.Failure(ex);
            }
        }
    }
}