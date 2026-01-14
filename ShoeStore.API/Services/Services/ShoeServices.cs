using AutoMapper;
using ShoeStore.API.Core;
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

        // ===================== CREATE =====================
        public async Task<ShoeResponseDto> CreateShoeAsync(ShoeCreateDto dto)
        {
            // Business rule example
            if (dto.Price <= 0)
                throw new BadRequestException("Price must be greater than 0");

            var shoe = _mapper.Map<Shoe>(dto);
            await _shoeRepository.AddAsync(shoe);

            return _mapper.Map<ShoeResponseDto>(shoe);
        }

        // ===================== GET BY ID =====================
        public async Task<ShoeResponseDto> GetShoeByIdAsync(int id)
        {
            var shoe = await _shoeRepository.GetByIdAsync(id);

            if (shoe == null)
                throw new NotFoundException($"Shoe with id {id} not found");

            return _mapper.Map<ShoeResponseDto>(shoe);
        }

        public async Task<PagedResult<ShoeResponseDto>> GetShoesAsync(
            FilterParams filters,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("PageNumber and PageSize must be greater than 0");

            var pagedShoes = await _shoeRepository.SearchAsync(filters, pageNumber, pageSize);

            return new PagedResult<ShoeResponseDto>
            {
                Items = _mapper.Map<List<ShoeResponseDto>>(pagedShoes.Items),
                TotalItems = pagedShoes.TotalItems,
                PageNumber = pagedShoes.PageNumber,
                PageSize = pagedShoes.PageSize
            };
        }

        public async Task<ShoeResponseDto> UpdateShoeAsync(int id, ShoeCreateDto dto)
        {
            var existingShoe = await _shoeRepository.GetByIdAsync(id);

            if (existingShoe == null)
                throw new NotFoundException($"Shoe with id {id} not found");

            _mapper.Map(dto, existingShoe);
            await _shoeRepository.UpdateAsync(existingShoe);

            return _mapper.Map<ShoeResponseDto>(existingShoe);
        }

        public async Task<ShoeResponseDto> DeleteShoeAsync(int id)
        {
            var shoe = await _shoeRepository.GetByIdAsync(id);

            if (shoe == null)
                throw new NotFoundException($"Shoe with id {id} not found");

            await _shoeRepository.DeleteAsync(id);

            return _mapper.Map<ShoeResponseDto>(shoe);
        }
    }
}
