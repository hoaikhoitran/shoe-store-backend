using AutoMapper;
using ShoeStore.API.Models.DTOs;
using ShoeStore.API.Models.Entities;
using ShoeStore.API.Repositories.IShoeRepository;
using ShoeStore.API.Services.Interfaces;

namespace ShoeStore.API.Services.Services
{
    public class ShoeServices : IShoeServices
    {
        private readonly IShoeRepository _shoeRepository;
        private readonly IMapper _mapper;

        public ShoeServices(IShoeRepository shoeRepository, IMapper mapper)
        {
            _shoeRepository = shoeRepository;
            _mapper = mapper;
        }

        public async Task<ShoeResponseDto> CreateShoeAsync(ShoeCreateDto dto)
        {
            // 1. Map DTO -> Entity
            var shoe = _mapper.Map<Shoe>(dto);

            // 2. Save DB
            await _shoeRepository.AddAsync(shoe);

            // 3. Map Entity -> Response DTO
            return _mapper.Map<ShoeResponseDto>(shoe);
        }


        public async Task<ShoeResponseDto> DeleteShoeAsync(int id)
        {
            var shoe = await _shoeRepository.GetByIdAsync(id);
            if (shoe == null)
            {
                return null;
            }
            await _shoeRepository.DeleteAsync(id);
            return _mapper.Map<ShoeResponseDto>(shoe);
        }

        public async Task<ShoeResponseDto> GetShoeByIdAsync(int id)
        {
            var shoe =  await _shoeRepository.GetByIdAsync(id);
            return _mapper.Map<ShoeResponseDto>(shoe);
        }

        public async Task<PagedResult<ShoeResponseDto>> GetShoesAsync(FilterParams filters, int pageNumber, int pageSize)
        {
            var pagedShoes = await _shoeRepository.SearchAsync(filters, pageNumber, pageSize);
            var shoeDtos = _mapper.Map<List<ShoeResponseDto>>(pagedShoes.Items);
            return new PagedResult<ShoeResponseDto>
            {
                Items = shoeDtos,
                TotalItems = pagedShoes.TotalItems,
                PageNumber = pagedShoes.PageNumber,
                PageSize = pagedShoes.PageSize
            };
        }

        public async Task<ShoeResponseDto> UpdateShoeAsync(int id, ShoeCreateDto dto)
        {
            var existingShoe = await _shoeRepository.GetByIdAsync(id);
            if(existingShoe == null)
            {
                return null;
            }
            _mapper.Map(dto, existingShoe);
            await _shoeRepository.UpdateAsync(existingShoe);
            return _mapper.Map<ShoeResponseDto>(existingShoe);
        }
    }
}
