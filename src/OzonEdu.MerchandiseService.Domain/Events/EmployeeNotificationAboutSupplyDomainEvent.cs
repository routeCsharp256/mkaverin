﻿using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public sealed record EmployeeNotificationAboutSupplyDomainEvent : INotification
    {
        public Email EmployeeEmail { get; set; }
        public NameUser EmployeeName { get; set; }
        public Email ManagerEmail { get; set; }
        public NameUser ManagerName { get; set; }
        public MerchType MerchType { get; init; }
    }
}
