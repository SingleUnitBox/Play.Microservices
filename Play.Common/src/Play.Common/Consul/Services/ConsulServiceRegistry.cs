﻿using Play.Common.Consul.Models;

namespace Play.Common.Consul.Services;

public class ConsulServiceRegistry : IConsulServicesRegistry
{
    private readonly Random _random = new();
    private readonly IConsulService _consulService;
    private readonly IDictionary<string, ISet<string>> _usedServices = new Dictionary<string, ISet<string>>();

    public ConsulServiceRegistry(IConsulService consulService)
    {
        _consulService = consulService;
    }

    public async Task<ServiceAgent> GetAsync(string name)
    {
        var services = await _consulService.GetServicesAsync(name);
        if (!services.Any())
        {
            return null;
        }

        if (!_usedServices.ContainsKey(name))
        {
            _usedServices[name] = new HashSet<string>();
        }
        else if (services.Count == _usedServices[name].Count)
        {
            _usedServices[name].Clear();
        }

        return GetService(services, name);
    }

    private ServiceAgent GetService(IDictionary<string, ServiceAgent> services, string name)
    {
        switch (services.Count)
        {
            case 0:
                return null;
            case 1:
                return services.First().Value;
            default:
                return ChooseService(services, name);
        }
    }

    private ServiceAgent ChooseService(IDictionary<string, ServiceAgent> services, string name)
    {
        ServiceAgent service;
        var unusedServices = services.Where(s => !_usedServices[name].Contains(s.Key)).ToArray();
        if (unusedServices.Any())
        {
            service = unusedServices.ElementAt(_random.Next(0, unusedServices.Count())).Value;
        }
        else
        {
            service = unusedServices.First().Value;
            _usedServices[name].Clear();
        }

        _usedServices[name].Add(service.Id);

        return service;
    }
}