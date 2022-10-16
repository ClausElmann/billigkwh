using System;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BilligKwhWebApp.Services.Economic.CustomersPost
{
    /// <summary>
    /// A schema for creating a customer, aka. Debtor.
    /// </summary>
    public partial class CustomerPost
    {
        /// <summary>
        /// Address for the customer including street and number.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PurpleMinMaxLengthCheckConverter))]
        public string Address { get; set; }

        /// <summary>
        /// The outstanding amount for this customer.
        /// </summary>
        [JsonProperty("balance", NullValueHandling = NullValueHandling.Ignore)]
        public double? Balance { get; set; }

        /// <summary>
        /// Boolean indication of whether the customer is barred from invoicing.
        /// </summary>
        [JsonProperty("barred", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Barred { get; set; }

        /// <summary>
        /// The customer's city.
        /// </summary>
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FluffyMinMaxLengthCheckConverter))]
        public string City { get; set; }

        /// <summary>
        /// Corporate Identification Number. For example CVR in Denmark.
        /// </summary>
        [JsonProperty("corporateIdentificationNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(TentacledMinMaxLengthCheckConverter))]
        public string CorporateIdentificationNumber { get; set; }

        /// <summary>
        /// The customer's country.
        /// </summary>
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FluffyMinMaxLengthCheckConverter))]
        public string Country { get; set; }

        /// <summary>
        /// A maximum credit for this customer. Once the maximum is reached or passed in connection
        /// with an order/quotation/invoice for this customer you see a warning in e-conomic.
        /// </summary>
        [JsonProperty("creditLimit", NullValueHandling = NullValueHandling.Ignore)]
        public double? CreditLimit { get; set; }

        /// <summary>
        /// Default payment currency.
        /// </summary>
        [JsonProperty("currency")]
        [JsonConverter(typeof(StickyMinMaxLengthCheckConverter))]
        public string Currency { get; set; }

        /// <summary>
        /// In order to set up a new customer, it is necessary to specify a customer group. It is
        /// useful to group a company’s customers (e.g., ‘domestic’ and ‘foreign’ customers) and to
        /// link the group members to the same account when generating reports.
        /// </summary>
        [JsonProperty("customerGroup")]
        public CustomerGroup CustomerGroup { get; set; }

        /// <summary>
        /// The customer number is a positive unique numerical identifier with a maximum of 9 digits.
        /// If no customer number is specified a number will be supplied by the system.
        /// </summary>
        [JsonProperty("customerNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? CustomerNumber { get; set; }

        /// <summary>
        /// European Article Number. EAN is used for invoicing the Danish public sector.
        /// </summary>
        [JsonProperty("ean", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(IndigoMinMaxLengthCheckConverter))]
        public string Ean { get; set; }

        /// <summary>
        /// Boolean indication of whether the default sending method should be email instead of
        /// e-invoice. This property is updatable only by using PATCH to /customers/:customerNumber
        /// </summary>
        [JsonProperty("eInvoicingDisabledByDefault", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EInvoicingDisabledByDefault { get; set; }

        /// <summary>
        /// Customer e-mail address where e-conomic invoices should be emailed. Note: you can specify
        /// multiple email addresses in this field, separated by a space. If you need to send a copy
        /// of the invoice or write to other e-mail addresses, you can also create one or more
        /// customer contacts.
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(IndecentMinMaxLengthCheckConverter))]
        public string Email { get; set; }

        /// <summary>
        /// Layout to be applied for invoices and other documents for this customer.
        /// </summary>
        [JsonProperty("layout", NullValueHandling = NullValueHandling.Ignore)]
        public Layout Layout { get; set; }

        /// <summary>
        /// The customer's mobile phone number.
        /// </summary>
        [JsonProperty("mobilePhone", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FluffyMinMaxLengthCheckConverter))]
        public string MobilePhone { get; set; }

        /// <summary>
        /// The customer name.
        /// </summary>
        [JsonProperty("name")]
        [JsonConverter(typeof(HilariousMinMaxLengthCheckConverter))]
        public string Name { get; set; }

        /// <summary>
        /// The default payment terms for the customer.
        /// </summary>
        [JsonProperty("paymentTerms")]
        public PaymentTerms PaymentTerms { get; set; }

        /// <summary>
        /// Extension of corporate identification number (CVR). Identifying separate production unit
        /// (p-nummer).
        /// </summary>
        [JsonProperty("pNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(AmbitiousMinMaxLengthCheckConverter))]
        public string PNumber { get; set; }

        [JsonProperty("priceGroup")]
        public object PriceGroup { get; set; }

        /// <summary>
        /// The public entry number is used for electronic invoicing, to define the account invoices
        /// will be registered on at the customer.
        /// </summary>
        [JsonProperty("publicEntryNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FluffyMinMaxLengthCheckConverter))]
        public string PublicEntryNumber { get; set; }

        /// <summary>
        /// Reference to the employee responsible for contact with this customer.
        /// </summary>
        [JsonProperty("salesPerson", NullValueHandling = NullValueHandling.Ignore)]
        public SalesPerson SalesPerson { get; set; }

        /// <summary>
        /// The customer's telephone and/or fax number.
        /// </summary>
        [JsonProperty("telephoneAndFaxNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(IndecentMinMaxLengthCheckConverter))]
        public string TelephoneAndFaxNumber { get; set; }

        /// <summary>
        /// The customer's value added tax identification number. This field is only available to
        /// agreements in Sweden, UK, Germany, Poland and Finland. Not to be mistaken for the danish
        /// CVR number, which is defined on the corporateIdentificationNumber property.
        /// </summary>
        [JsonProperty("vatNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(CunningMinMaxLengthCheckConverter))]
        public string VatNumber { get; set; }

        /// <summary>
        /// Indicates in which VAT-zone the customer is located (e.g.: domestically, in Europe or
        /// elsewhere abroad).
        /// </summary>
        [JsonProperty("vatZone")]
        public VatZone VatZone { get; set; }

        /// <summary>
        /// Customer website, if applicable.
        /// </summary>
        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(IndecentMinMaxLengthCheckConverter))]
        public string Website { get; set; }

        /// <summary>
        /// The customer's postcode.
        /// </summary>
        [JsonProperty("zip", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(MagentaMinMaxLengthCheckConverter))]
        public string Zip { get; set; }
    }

    /// <summary>
    /// In order to set up a new customer, it is necessary to specify a customer group. It is
    /// useful to group a company’s customers (e.g., ‘domestic’ and ‘foreign’ customers) and to
    /// link the group members to the same account when generating reports.
    /// </summary>
    public partial class CustomerGroup
    {
        /// <summary>
        /// The unique identifier of the customer group.
        /// </summary>
        [JsonProperty("customerGroupNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? CustomerGroupNumber { get; set; }

        /// <summary>
        /// A unique link reference to the customer group item.
        /// </summary>
        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Self { get; set; }
    }

    /// <summary>
    /// Layout to be applied for invoices and other documents for this customer.
    /// </summary>
    public partial class Layout
    {
        /// <summary>
        /// The unique identifier of the layout.
        /// </summary>
        [JsonProperty("layoutNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? LayoutNumber { get; set; }

        /// <summary>
        /// A unique link reference to the layout item.
        /// </summary>
        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Self { get; set; }
    }

    /// <summary>
    /// The default payment terms for the customer.
    /// </summary>
    public partial class PaymentTerms
    {
        /// <summary>
        /// The unique identifier of the payment terms.
        /// </summary>
        [JsonProperty("paymentTermsNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaymentTermsNumber { get; set; }

        /// <summary>
        /// A unique link reference to the payment terms item.
        /// </summary>
        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Self { get; set; }
    }

    /// <summary>
    /// Reference to the employee responsible for contact with this customer.
    /// </summary>
    public partial class SalesPerson
    {
        /// <summary>
        /// The unique identifier of the employee.
        /// </summary>
        [JsonProperty("employeeNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? EmployeeNumber { get; set; }

        /// <summary>
        /// A unique link reference to the employee resource.
        /// </summary>
        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Self { get; set; }
    }

    /// <summary>
    /// Indicates in which VAT-zone the customer is located (e.g.: domestically, in Europe or
    /// elsewhere abroad).
    /// </summary>
    public partial class VatZone
    {
        /// <summary>
        /// A unique link reference to the VAT-zone item.
        /// </summary>
        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Self { get; set; }

        /// <summary>
        /// The unique identifier of the VAT-zone.
        /// </summary>
        [JsonProperty("vatZoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? VatZoneNumber { get; set; }
    }

    public partial class CustomerPost
    {
        public static CustomerPost FromJson(string json) => JsonConvert.DeserializeObject<CustomerPost>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CustomerPost self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class PurpleMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length <= 510)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length <= 510)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly PurpleMinMaxLengthCheckConverter Singleton = new PurpleMinMaxLengthCheckConverter();
    }

    internal class FluffyMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length <= 50)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length <= 50)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly FluffyMinMaxLengthCheckConverter Singleton = new FluffyMinMaxLengthCheckConverter();
    }

    internal class TentacledMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length <= 40)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length <= 40)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly TentacledMinMaxLengthCheckConverter Singleton = new TentacledMinMaxLengthCheckConverter();
    }

    internal class StickyMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length >= 3 && value.Length <= 3)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length >= 3 && value.Length <= 3)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly StickyMinMaxLengthCheckConverter Singleton = new StickyMinMaxLengthCheckConverter();
    }

    internal class IndigoMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length <= 13)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length <= 13)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly IndigoMinMaxLengthCheckConverter Singleton = new IndigoMinMaxLengthCheckConverter();
    }

    internal class IndecentMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length <= 255)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length <= 255)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly IndecentMinMaxLengthCheckConverter Singleton = new IndecentMinMaxLengthCheckConverter();
    }

    internal class HilariousMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length >= 1 && value.Length <= 255)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length >= 1 && value.Length <= 255)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly HilariousMinMaxLengthCheckConverter Singleton = new HilariousMinMaxLengthCheckConverter();
    }

    internal class AmbitiousMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length >= 10 && value.Length <= 10)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length >= 10 && value.Length <= 10)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly AmbitiousMinMaxLengthCheckConverter Singleton = new AmbitiousMinMaxLengthCheckConverter();
    }

    internal class CunningMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length <= 20)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length <= 20)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly CunningMinMaxLengthCheckConverter Singleton = new CunningMinMaxLengthCheckConverter();
    }

    internal class MagentaMinMaxLengthCheckConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            if (value.Length <= 30)
            {
                return value;
            }
            throw new Exception("Cannot unmarshal type string");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (string)untypedValue;
            if (value.Length <= 30)
            {
                serializer.Serialize(writer, value);
                return;
            }
            throw new Exception("Cannot marshal type string");
        }

        public static readonly MagentaMinMaxLengthCheckConverter Singleton = new MagentaMinMaxLengthCheckConverter();
    }
}


