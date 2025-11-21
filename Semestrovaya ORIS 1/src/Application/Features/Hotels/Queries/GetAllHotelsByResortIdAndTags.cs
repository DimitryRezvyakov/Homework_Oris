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
    public class GetAllHotelsByResortIdAndTags : IRequest<Result<List<Hotel>>>
    {
        public int ResortId { get; set; }
        public List<string> Tags { get; set; }
        public GetAllHotelsByResortIdAndTags() { }

        public GetAllHotelsByResortIdAndTags(int id, List<string> tags)
        {
            ResortId = id;
            Tags = tags;
        }
    }

    public class GetAllHotelsByResortIdAndTagsHandler : IRequestHandler<GetAllHotelsByResortIdAndTags, Result<List<Hotel>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllHotelsByResortIdAndTagsHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Hotel>>> Handle(GetAllHotelsByResortIdAndTags request, CancellationToken cts)
        {
            try
            {
                if (request.Tags == null || !request.Tags.Any())
                {
                    var hotels = await _repository.GetAll<Hotel>();
                    return Result<List<Hotel>>.Success(hotels.Where(h => h.ResortId == request.ResortId).ToList());
                }

                var tags = request.Tags;

                var tagParams = string.Join(",", tags.Select((_, i) => $"@tag{i}"));

                var sql = $@"
                    SELECT h.*
                    FROM Hotel h
                    INNER JOIN HotelTagTable htt ON h.Id = htt.HotelId
                    INNER JOIN HotelTag ht ON htt.TagId = ht.Id
                    WHERE h.ResortId = @resortId
                      AND ht.Name IN ({tagParams})
                    GROUP BY h.Id, h.Name, h.ResortId /* и все остальные поля Hotel */
                    HAVING COUNT(DISTINCT ht.Name) = @tagCount";

                var parameters = new Dictionary<string, object>
                {
                    ["resortId"] = request.ResortId,
                    ["tagCount"] = tags.Count
                };

                for (int i = 0; i < tags.Count; i++)
                {
                    parameters[$"tag{i}"] = tags[i];
                }

                var data = await _repository.UseSqlCommandQueryCollection<Hotel>(sql, parameters);
                return Result<List<Hotel>>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<List<Hotel>>.Failure(ex);
            }
        }
    }
}