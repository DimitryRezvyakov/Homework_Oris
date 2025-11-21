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
    public class DeleteHotelByIdCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public DeleteHotelByIdCommand() { }
        public DeleteHotelByIdCommand(int id)
        {
            Id = id;
        }
    }
    internal class DeleteHotelByIdCommandHandler : IRequestHandler<DeleteHotelByIdCommand, Result<bool>>
    {
        private readonly IGenericRepository _repository;

        public DeleteHotelByIdCommandHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(DeleteHotelByIdCommand request, CancellationToken cts)
        {
            try
            {
                await _repository.Delete<Hotel>(request.Id);

                return await Result<bool>.SuccessAsync(true);
            }
            catch (Exception ex)
            {
                return await Result<bool>.FailureAsync(ex);
            }
        }
    }
}
