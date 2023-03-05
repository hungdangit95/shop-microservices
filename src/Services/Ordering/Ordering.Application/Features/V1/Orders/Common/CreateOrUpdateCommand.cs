using AutoMapper;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrUpdateCommand : IMapFrom<Order>
    { 
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string _invoiceAddress;
        public string? InvoiceAddress
        {
            get => _invoiceAddress;
            set => _invoiceAddress = value ?? ShippingAddress;
        }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrUpdateCommand, Order>();
        }
    }
}
