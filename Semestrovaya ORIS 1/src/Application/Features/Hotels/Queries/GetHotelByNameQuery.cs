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
    public class GetHotelByNameQuery : IRequest<Result<Hotel>>
    {
        public string Name { get; set; }
        public GetHotelByNameQuery() { }
        public GetHotelByNameQuery(string name)
        {
            Name = name;
        }
    }

    internal class GetHotelByNameQueryHandler : IRequestHandler<GetHotelByNameQuery, Result<Hotel>>
    {
        private readonly IGenericRepository _repository;

        public GetHotelByNameQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Hotel>> Handle(GetHotelByNameQuery request, CancellationToken cts)
        {
            try
            {
                var sql = @"SELECT * FROM ""Hotel"" WHERE ""Hotel"".""Name"" = @name";

                var param = new Dictionary<string, object>()
                {
                    {"name", request.Name},
                };
                var result = await _repository.UseSqlCommandQuery<Hotel>(sql, param);

                if (result != null)
                    return await Result<Hotel>.SuccessAsync(result);

                return await Result<Hotel>.FailureAsync($"Can`t find hotel with name {request.Name}");
            }
            catch (Exception ex)
            {
                return await Result<Hotel>.FailureAsync(ex);
            }
        }
    }
}
