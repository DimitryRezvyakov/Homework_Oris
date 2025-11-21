using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotels.Commands
{
    public class DeleteAllHotelTagsByHotelIdCommand : IRequest<Result<bool>>
    {
        public int HotelId { get; set; }
        public DeleteAllHotelTagsByHotelIdCommand() { }
        public DeleteAllHotelTagsByHotelIdCommand(int hotelId)
        {
            HotelId = hotelId;
        }
    }

    internal class DeleteAllHotelTagsByHotelIdCommandHandler : IRequestHandler<DeleteAllHotelTagsByHotelIdCommand, Result<bool>>
    {
        private readonly IGenericRepository _repository;

        public DeleteAllHotelTagsByHotelIdCommandHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(DeleteAllHotelTagsByHotelIdCommand request, CancellationToken cts)
        {
            var sql = @"DELETE FROM ""HotelTagTable"" as htt WHERE htt.""HotelId"" = @HotelId";

            var parameters = new Dictionary<string, object>
            {
                { "HotelId", request.HotelId }
            };

            try
            {
                await _repository.UseSqlCommandNonQuery(sql, parameters);

                return await Result<bool>.SuccessAsync();
            }
            catch (Exception ex)
            {
                return await Result<bool>.FailureAsync(ex);
            }
        }
    }
}
