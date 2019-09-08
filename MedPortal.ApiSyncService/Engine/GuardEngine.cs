using MedPortal.Data.DTO;
using MedPortal.Data.Logging;
using MedPortal.Data.Repositories;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace MedPortal.ApiSyncService.Engine
{
    public class GuardEngine
    {
        private readonly IHighloadedRepository<HClinic> _clinicsRepository;

        private readonly ILogger _logger;

        public GuardEngine(IHighloadedRepository<HClinic> clinicsRepository, ILogger logger)
        {
            _clinicsRepository = clinicsRepository;
            _logger = logger;
        }

        public async Task FixClinicLocationsAsync()
        {
            var clinicsToFix = await _clinicsRepository.GetAsync(c => c.Latitude == 0 || c.Longitude == 0);
            var client = new RestClient()
            {
                BaseUrl = new Uri("https://geocode-maps.yandex.ru/1.x")
            };
            CancellationTokenSource cts = new CancellationTokenSource();
            foreach (var clinic in clinicsToFix)
            {
                string address = $"{clinic.HCity.Name},{clinic.HStreet.Name},{clinic.House}";
                try
                {
                    
                    //string address = "Санкт-Петербург,Железноводская32";
                    var request = new RestRequest($"?format=json&apikey=58e1bfc0-7fda-4ac2-b497-3bb821e969c5&geocode={address}");
                    request.AddHeader("Content-Type", "application/json");
                    var result = await client.ExecuteGetTaskAsync<RootObject>(request, cts.Token);

                    var position = result.Data.response.GeoObjectCollection.featureMember.FirstOrDefault()?.GeoObject?.Point?.pos;
                    if (!string.IsNullOrEmpty(position)) {
                        var coordinates = position.Replace(".", ",").Split(" ");
                        double longitude = Double.Parse(coordinates[0]);
                        double latitude = Double.Parse(coordinates[1]);

                        clinic.Latitude = latitude;
                        clinic.Longitude = longitude;
                    }


                } catch(Exception e)
                {
                    _logger.LogError($"Error on getting location for address: {address}", e);
                }
                
            }

            await _clinicsRepository.BulkUpdateAsync(clinicsToFix);

        }

        public class GeocoderResponseMetaData
        {
            public string request { get; set; }
            public string found { get; set; }
            public string results { get; set; }
        }

        public class MetaDataProperty
        {
            public GeocoderResponseMetaData GeocoderResponseMetaData { get; set; }
        }

        public class Component
        {
            public string kind { get; set; }
            public string name { get; set; }
        }

        public class Address
        {
            public string country_code { get; set; }
            public string formatted { get; set; }
            public List<Component> Components { get; set; }
        }

        public class Premise
        {
            public string PremiseNumber { get; set; }
        }

        public class Thoroughfare
        {
            public string ThoroughfareName { get; set; }
            public Premise Premise { get; set; }
        }

        public class DependentLocality
        {
            public string DependentLocalityName { get; set; }
            public Thoroughfare Thoroughfare { get; set; }
        }

        public class Locality
        {
            public string LocalityName { get; set; }
            public DependentLocality DependentLocality { get; set; }
        }

        public class SubAdministrativeArea
        {
            public string SubAdministrativeAreaName { get; set; }
            public Locality Locality { get; set; }
        }

        public class AdministrativeArea
        {
            public string AdministrativeAreaName { get; set; }
            public SubAdministrativeArea SubAdministrativeArea { get; set; }
        }

        public class Country
        {
            public string AddressLine { get; set; }
            public string CountryNameCode { get; set; }
            public string CountryName { get; set; }
            public AdministrativeArea AdministrativeArea { get; set; }
        }

        public class AddressDetails
        {
            public Country Country { get; set; }
        }

        public class GeocoderMetaData
        {
            public string kind { get; set; }
            public string text { get; set; }
            public string precision { get; set; }
            public Address Address { get; set; }
            public AddressDetails AddressDetails { get; set; }
        }

        public class MetaDataProperty2
        {
            public GeocoderMetaData GeocoderMetaData { get; set; }
        }

        public class Envelope
        {
            public string lowerCorner { get; set; }
            public string upperCorner { get; set; }
        }

        public class BoundedBy
        {
            public Envelope Envelope { get; set; }
        }

        public class Point
        {
            public string pos { get; set; }
        }

        public class GeoObject
        {
            public MetaDataProperty2 metaDataProperty { get; set; }
            public string description { get; set; }
            public string name { get; set; }
            public BoundedBy boundedBy { get; set; }
            public Point Point { get; set; }
        }

        public class FeatureMember
        {
            public GeoObject GeoObject { get; set; }
        }

        public class GeoObjectCollection
        {
            public MetaDataProperty metaDataProperty { get; set; }
            public List<FeatureMember> featureMember { get; set; }
        }

        public class Response
        {
            public GeoObjectCollection GeoObjectCollection { get; set; }
        }

        public class RootObject
        {
            public Response response { get; set; }
        }


    }
}
