using Application.DTOs;
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
    public class GetAllHotelsSmallDTOQuery : IRequest<Result<List<HotelSmallDTO>>>
    {
    }

    public class GetAllHotelsSmallDTOQueryHandler : IRequestHandler<GetAllHotelsSmallDTOQuery, Result<List<HotelSmallDTO>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllHotelsSmallDTOQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<HotelSmallDTO>>> Handle(GetAllHotelsSmallDTOQuery request, CancellationToken cts)
        {
            try
            {
                var sql = @"
                SELECT 
                    ""Id"", 
                    ""ResortId"", 
                    ""Name""
                FROM ""Hotel""";

                var data = await _repository.UseSqlCommandQueryCollection<HotelSmallDTO>(sql, new Dictionary<string, object>());

                return await Result<List<HotelSmallDTO>>.SuccessAsync(data);
            }
            catch (Exception ex)
            {
                return await Result<List<HotelSmallDTO>>.FailureAsync(ex.Message);
            }
        }
    }
}
