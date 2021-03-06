﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nimbus.Handlers;
using Pizza.Maker.Messages;
using Pizza.RetailWeb.Models.Home;

namespace Pizza.RetailWeb.ReadModels
{
    public class PizzaOrderStatusReadModel : IAmAReadModel, IHandleMulticastEvent<NewOrderRecieved>, IHandleMulticastEvent<PizzaIsReady>
    {
        private readonly ConcurrentDictionary<string, PizzaOrderStatus> _orders = new ConcurrentDictionary<string, PizzaOrderStatus>();

        public async Task Handle(NewOrderRecieved busEvent)
        {
            var orderStatus = new PizzaOrderStatus
                              {
                                  CustomerName = busEvent.CustomerName,
                                  Ordered = DateTimeOffset.UtcNow,
                              };

            _orders[busEvent.CustomerName] = orderStatus;
        }

        public async Task Handle(PizzaIsReady busEvent)
        {
            PizzaOrderStatus orderStatus;
            if (!_orders.TryGetValue(busEvent.CustomerName, out orderStatus)) return;

            orderStatus.Ready = DateTimeOffset.UtcNow;
        }

        public IEnumerable<PizzaOrderStatus> Orders
        {
            get { return _orders.Values; }
        }
    }
}