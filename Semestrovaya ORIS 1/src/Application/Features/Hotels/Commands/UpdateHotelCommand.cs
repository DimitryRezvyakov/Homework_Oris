using Application.Entities;
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
    public class UpdateHotelCommand : IRequest<Result<bool>>
    {
        public Hotel Hotel { get; set; }
        public UpdateHotelCommand() { }
        public UpdateHotelCommand(Hotel hotel)
        {
            Hotel = hotel;
        }
    }

    internal class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result<bool>>
    {
        private readonly IGenericRepository _repository;

        public UpdateHotelCommandHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(UpdateHotelCommand request, CancellationToken cts)
        {
            var sql = @"UPDATE ""Hotel"" 
                SET 
                    ""ResortId"" = @ResortId,
                    ""Name"" = @Name,
                    ""HotelType"" = @HotelType,
                    ""Price"" = @Price,
                    ""Stars"" = @Stars,
                    ""Raiting"" = @Raiting,
                    ""Nutrition"" = @Nutrition,
                    ""Description"" = @Description,
                    ""HtmlDescription"" = @HtmlDescription,
                    ""Images"" = @Images
                WHERE ""Id"" = @Id";

            var parameters = new Dictionary<string, object>()
            {
                { "Id", request.Hotel.Id },
                { "ResortId", request.Hotel.ResortId },
                { "Name", request.Hotel.Name },
                { "HotelType", request.Hotel.HotelType ?? (object)DBNull.Value },
                { "Price", request.Hotel.Price },
                { "Stars", request.Hotel.Stars },
                { "Raiting", request.Hotel.Raiting },
                { "Nutrition", request.Hotel.Nutrition },
                { "Description", request.Hotel.Description },
                { "HtmlDescription", request.Hotel.HtmlDescription },
                { "Images", request.Hotel.Images != null ? request.Hotel.Images: new List<string>() }
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
