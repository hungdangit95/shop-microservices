﻿using AutoMapper;
using Basket.API.Entities;
using Shared.Dtos.Basket;

namespace Basket.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<BasketCheckout, BasketCheckoutEvent>();
        CreateMap<CartDto, Cart>().ReverseMap();
        CreateMap<CartItemDto, CartItem>().ReverseMap();
    }
}
