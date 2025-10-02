using AutoMapper;
using WebAPIDemo.DTO.Bestellingen;
using WebAPIDemo.DTO.Klant;
using WebAPIDemo.DTO.Orderlijn;
using WebAPIDemo.DTO.Product;

namespace WebAPIDemo.Configuration
{
    public class AutoMapperProfile: Profile
    {
        //Hier bepalen we welke klassen gemapt (geconverteerd) kunnen worden naar elkaar.
        public AutoMapperProfile()
        {
            // Defenieer hier je mappings in de ctor
            // Producten
            CreateMap<AddProductDTO, Product>()
                .ForMember(dest => dest.Prijs, x => x.MapFrom(src => src.Price));//Expliciete mappings met ForMember m.b.v. Method Chaining

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.Prijs, x => x.MapFrom(src => src.Price));//Expliciete mappings met ForMember m.b.v. Method Chaining

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Price, x=> x.MapFrom(src => src.Prijs));

            // Klanten
            CreateMap<Klant, KlantBestellingenDTO>()
                .ForMember(dest => dest.KlantNaam, x => x.MapFrom(src => $"{src.Naam} {src.Voornaam}"));

            CreateMap<Klant, KlantDTO>()
                .ForMember(dest => dest.KlantNaam, x => x.MapFrom(src => $"{src.Naam} {src.Voornaam}"));

            CreateMap<AddKlantDTO, Klant>();
            CreateMap<UpdateKlantDTO, Klant>();

            // Bestellingen
            CreateMap<Bestelling, BestellingDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Orderlijnen.Select(ol => new ProductDTO
                {
                    Aantal = ol.Aantal,
                    ProductNaam = ol.Product.Naam,
                    Price = ol.Product.Prijs,
                    Totaal = ol.Aantal * Convert.ToDouble(ol.Product.Prijs),
                    Beschrijving = ol.Product.Beschrijving
                }).ToList()));

            CreateMap<AddBestellingDTO, Bestelling>();
            CreateMap<UpdateBestellingDTO, Bestelling>();

            // Orderlijnen
            CreateMap<Orderlijn, OrderlijnDTO>();
            CreateMap<AddOrderlijnBestellingDTO, Orderlijn>();
            CreateMap<UpdateOrderLijnDTO, Orderlijn>();
        }
    }
}
