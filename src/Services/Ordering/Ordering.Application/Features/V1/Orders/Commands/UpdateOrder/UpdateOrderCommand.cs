using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders;

public class UpdateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
{
    public int Id { get; private set; }
    public void SetId(int id)
    {
        Id = id;
    }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateOrderCommand, Order>()
            .ForMember(dest => dest.Status, opts => opts.Ignore())
            .IgnoreAllNonExisting();

    }
}