﻿using ShopPlatform.Shared.ModelsDTO;

namespace ShopPlatform.Server.Services.Infrastuce
{
    public interface ISupplierService
    {
        public Task<SupplierDTO> GetSupplieById(Guid id);
        public Task<List<SupplierDTO>> GetSupplie();
        public Task<SupplierDTO> CreateSupplie(SupplierDTO Supplie);
        public Task<SupplierDTO> UpdateSupplie(SupplierDTO Supplie);
        public Task<bool> DeleteSupplieById(Guid id);
    }
}
