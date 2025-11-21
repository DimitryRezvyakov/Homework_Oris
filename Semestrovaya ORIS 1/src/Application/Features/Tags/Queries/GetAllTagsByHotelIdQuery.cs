using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tags.Queries
{
    public class GetAllTagsByHotelIdQuery : IRequest<Result<List<HotelTag>>>
    {
        public int HotelId { get; set; }
        public GetAllTagsByHotelIdQuery() { }
        public GetAllTagsByHotelIdQuery(int hotelId)
        {
            HotelId = hotelId;
        }
    }

    internal class GetAllTagsByHotelIdQueryHandler : IRequestHandler<GetAllTagsByHotelIdQuery, Result<List<HotelTag>>>
    {
        private readonly IGenericRepository _repository;
        public GetAllTagsByHotelIdQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<HotelTag>>> Handle(GetAllTagsByHotelIdQuery request, CancellationToken cts)
        {
            var sql = @"SELECT ht.""Id"", ht.""Type"", ht.""Name""
                        FROM ""HotelTagTable"" as htt
                        INNER JOIN ""HotelTag"" as ht ON htt.""TagId"" = ht.""Id""
                        WHERE htt.""HotelId"" = @hotelId";

            var parameters = new Dictionary<string, object>
            {
                {"hotelId", request.HotelId},
            };
            try
            {
                var tags = await _repository.UseSqlCommandQueryCollection<HotelTag>(sql, parameters);

                if (tags != null)
                    return await Result<List<HotelTag>>.SuccessAsync(tags);

                return await Result<List<HotelTag>>.FailureAsync($"Can`t find tags of the hotel {request.HotelId}");
            }
            catch (Exception ex)
            {
                return await Result<List<HotelTag>>.FailureAsync(ex);
            }

        }
    }
}
