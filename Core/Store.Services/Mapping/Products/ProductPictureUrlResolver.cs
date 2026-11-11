using AutoMapper;
using Microsoft.Extensions.Options;
using Store.Domain.Entities.Products;
using Store.Shared.DTOS.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Mapping.Products
{
    internal class PictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly UrlSettings _urlSettings;
        public PictureUrlResolver(IOptions<UrlSettings> options)
        {
            _urlSettings = options.Value;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _urlSettings.BaseUrl.TrimEnd('/');
            var path = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";
        }
    }
    public class UrlSettings
    {
        public string BaseUrl { get; set; } = default!;
    }
}
