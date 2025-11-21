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
    public class GetAllHotelsMediumDTOQuery : IRequest<Result<List<HotelMediumDTO>>>
    {
    }

    public class GetAllHotelsMediumDTOQueryHandler : IRequestHandler<GetAllHotelsMediumDTOQuery, Result<List<HotelMediumDTO>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllHotelsMediumDTOQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<HotelMediumDTO>>> Handle(GetAllHotelsMediumDTOQuery request, CancellationToken cts)
        {
            try
            {
                var sql = @"
                            SELECT 
                            h.""Id"",
                            h.""ResortId"",
                            h.""Name"",
                            h.""Price"",
                            h.""Stars"",
                            h.""Raiting"",
                            h.""Description"",
                            h.""Images""->>0 AS ""Image"",
                            CONCAT(c.""Name"", ', ', r.""Name"") AS ""Location""
                            FROM ""Hotel"" h
                            INNER JOIN ""Resort"" r ON h.""ResortId"" = r.""Id""
                            INNER JOIN ""Country"" c ON r.""CountryId"" = c.""Id""
                            WHERE h.""Images"" IS NOT NULL 
                              AND jsonb_array_length(h.""Images"") > 0";

                var data = await _repository.UseSqlCommandQueryCollection<HotelMediumDTO>(sql, new Dictionary<string, object>());

                if (data != null)
                    return await Result<List<HotelMediumDTO>>.SuccessAsync(data);

                return await Result<List<HotelMediumDTO>>.FailureAsync($"Can`t get any HotelMediumDto");
            }
            catch (Exception ex)
            {
                return await Result<List<HotelMediumDTO>>.FailureAsync(ex);
            }
        }
    }
}
