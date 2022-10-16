using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Eltavler.Repository;
using BilligKwhWebApp.Services.Interfaces;
using System.Collections.Generic;
using BilligKwhWebApp.Services.Enums;
using System;
using System.Threading.Tasks;
using BilligKwhWebApp.Services.Invoicing.Economic.Customers;
using BilligKwhWebApp.Services.Customers;
using System.Linq;
using System.Data;
using Z.Dapper.Plus;
using System.Linq.Expressions;

namespace BilligKwhWebApp.Services
{
    public class EltavleService : IEltavleService
    {
        // Props
        private readonly ISystemLogger _logger;
        private readonly IEltavlerRepository _eltavlerRepository;
        private readonly IBaseRepository _baseRepository;
        private readonly IEconomicHttpClient _economicHttpClient;
        private readonly ICustomerService _customerService;

        // Ctor
        public EltavleService(
            ISystemLogger logger,
            IEltavlerRepository eltavlerRepository,
            IBaseRepository baseRepository,
            IEconomicHttpClient economicHttpClient,
            ICustomerService customerService)
        {
            _logger = logger;
            _eltavlerRepository = eltavlerRepository;
            _baseRepository = baseRepository;
            _economicHttpClient = economicHttpClient;
            _customerService = customerService;
        }

        public IReadOnlyCollection<SektionElKomponentDto> GetAllSektionElKomponents(int tavleId, bool onlyPlacering = false)
        {
            return _eltavlerRepository.GetAllSektionElKomponents(tavleId, onlyPlacering);
        }

        public IReadOnlyCollection<LaageElKomponentDto> GetAllLaageElKomponents(int tavleId, int laageId, bool onlyPlacering = false)
        {
            return _eltavlerRepository.GetAllLaageElKomponents(tavleId, laageId, onlyPlacering);
        }

        public IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionElKomponent(int tavleId)
        {
            return _eltavlerRepository.GetAllElTavleSektionElKomponent(tavleId);
        }

        public IReadOnlyCollection<ElTavleLaageElKomponent> GetAllElTavleLaageElKomponent(int laageId)
        {
            return _eltavlerRepository.GetAllElTavleLaageElKomponenter(laageId);
        }

        public IReadOnlyCollection<ElKredsKomponent> GetAllElKredsKomponenter()
        {
            return _eltavlerRepository.GetAllElKredsKomponenter();
        }

        public ElTavleDto GetDtoById(int tavleId)
        {
            return _eltavlerRepository.GetDtoById(tavleId);
        }

        public ElTavle GetById(int tavleId)
        {
            return _eltavlerRepository.GetById(tavleId);
        }

        public ElTavleLaage GetElTavleLaageById(int laageId)
        {
            return _eltavlerRepository.GetElTavleLaageById(laageId);
        }

        public void Update(ElTavle eltavle)
        {
            if (eltavle != null)
            {
                _eltavlerRepository.Update(eltavle);
            }
            else
            {
                _logger.Warning("eltavle is NULL in Update!", null, "EltavleService");
            }
        }

        public void Insert(ElTavle eltavle)
        {
            _eltavlerRepository.Insert(eltavle);
        }

        public IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionKomponenterByTavle(int tavleId)
        {
            return _eltavlerRepository.GetAllByTavle(tavleId);
        }

        public IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionKomponenterForPlacering(int tavleId)
        {
            return _eltavlerRepository.GetAllForPlacering(tavleId);
        }

        public IReadOnlyCollection<SektionKomponentAntalDto> GetAllSektionElKomponentsAntalDto(int tavleId)
        {
            return _eltavlerRepository.GetAllSektionElKomponentsAntalDto(tavleId);
        }
        public IReadOnlyCollection<KabinetKomponentDto> GetAllKabinetter(int tavleId)
        {
            return _eltavlerRepository.GetAllKabinetter(tavleId);
        }

        public IReadOnlyCollection<ElTavleDto> GetAllElTavleDto(string filter, string varetype)
        {
            return _eltavlerRepository.GetAllElTavleDto(filter, varetype);
        }

        public void SletExtraDispPlads(int tavleId)
        {
            _eltavlerRepository.SletExtraDispPlads(tavleId);
        }

        public IReadOnlyCollection<ElTavleSektionElKomponent> GetAllKomponentPlacering(int tavleId)
        {
            return _eltavlerRepository.GetAllElTavleSektionElKomponent(tavleId);
        }

        public IReadOnlyCollection<ElTavleLaageElKomponent> GetAllLaageKomponentPlacering(int tavleid)
        {
            return _eltavlerRepository.GetAllElTavleLaageElKomponenter(tavleid);
        }

        public IReadOnlyCollection<ElKomponentItemDto> AlleKomponenter()
        {
            return _eltavlerRepository.AlleKomponenter();
        }

        public IReadOnlyCollection<ElKomponent> AlleElKomponenter()
        {
            return _eltavlerRepository.AlleElKomponenter();
        }

        public IReadOnlyCollection<ElTavleSektionDto> AlleSektioner(int tavleId)
        {
            return _eltavlerRepository.AlleSektioner(tavleId);
        }

        public ElTavleSektionElKomponent GetElTavleSektionKomponentById(int id)
        {
            return _eltavlerRepository.GetElTavleSektionKomponentById(id);
        }

        public void UpdateElTavleSektionKomponent(ElTavleSektionElKomponent entity)
        {
            if (entity != null)
            {
                _eltavlerRepository.UpdateElTavleSektionKomponent(entity);
            }
            else
            {
                _logger.Warning("entity is NULL in UpdateElTavleSektionKomponent!", null, "EltavleService");
            }
        }

        public void GenberegnKabinetter(int tavleId, int? moduler)
        {
            if (moduler == null)
                _eltavlerRepository.UpdateModuler(tavleId);

            var eltavle = _eltavlerRepository.GetById(tavleId);

            if (eltavle.TypeID != 2)
                return;

            _baseRepository.Execute(@"DELETE ElTavleSektionElKomponent   
             FROM   ElTavleSektionElKomponent INNER JOIN
             ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
             ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
             WHERE (ElTavleSektionElKomponent.ElTavleID = @TavleId) AND (ElKomponentKategori.ErKabinet = 1)", new { TavleId = eltavle.Id });

            var sektion0 = _eltavlerRepository.AlleSektioner(eltavle.Id).Where(w => w.TypeID == 0).FirstOrDefault();

            var kabinetter = UgTavlerFromModuler(eltavle, sektion0.ID);


            foreach (var item in kabinetter)
            {
                _eltavlerRepository.InsertElTavleSektionKomponent(item);
            }

            _eltavlerRepository.UpdateModuler(tavleId);
        }

        public void GenberegnKomponenterPris(int tavleId)
        {
            var eltavle = _eltavlerRepository.GetById(tavleId);

            if (eltavle.TypeID == 2)
            {
                eltavle.KomponenterPris = _baseRepository.QueryFirstOrDefault<int>(@"
                    SELECT SUM(ElKomponent.BruttoPris) 
                    FROM   ElTavleSektionElKomponent INNER JOIN
                                ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID
                    WHERE ElTavleSektionElKomponent.ElTavleID = @TavleId", new { TavleId = tavleId });
            }
            else
            {
                eltavle.KomponenterPris = 0;
            }

            eltavle.NettoPris = eltavle.KomponenterPris;

            if (eltavle.BonusGivende)
                eltavle.OptjentBonus = Convert.ToInt32(eltavle.NettoPris * 0.05);
            else
                eltavle.OptjentBonus = 0;

            if (eltavle.InitialRabat)
                eltavle.NettoPris = Convert.ToInt32(eltavle.KomponenterPris * 0.8);

            eltavle.SidstRettet = DateTime.UtcNow;

            _eltavlerRepository.Update(eltavle);

            GenberegnVarmetab(eltavle.Id);
        }

        public void GenberegnVarmetab(int tavleId)
        {
            var eltavle = _eltavlerRepository.GetById(tavleId);

            if (eltavle.TypeID == 2)
            {
                eltavle.MaxEffekt = _baseRepository.QueryFirstOrDefault<int>(@"
                    SELECT SUM(ElKomponent.Effekt) 
                    FROM   ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
                    WHERE ElTavleSektionElKomponent.ElTavleID = @TavleId AND ElKomponentKategori.ErKabinet = 1", new { TavleId = tavleId });

                var antalHovedkredse = _baseRepository.QueryFirstOrDefault<int>(@"
                    SELECT count(*) 
                    FROM   ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
                    WHERE ElTavleSektionElKomponent.ElTavleID = @TavleId AND ElKomponentKategori.ErHovedKreds = 1 ", new { TavleId = tavleId });


                eltavle.Samtidighed = Samtidighed(antalHovedkredse);

                decimal ledninger = 1.15M;

                var effekt = _baseRepository.QueryFirstOrDefault<int>(@"
                    SELECT SUM(ElKomponent.Effekt) 
                    FROM   ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
                    WHERE ElTavleSektionElKomponent.ElTavleID = @TavleId AND ElKomponentKategori.HarEffekt = 0", new { TavleId = tavleId });

                eltavle.InstalleretEffekt = effekt * ledninger * eltavle.Samtidighed;
            }

            eltavle.SidstRettet = DateTime.UtcNow;

            _eltavlerRepository.Update(eltavle);
        }

        private static decimal Samtidighed(int antalHovedkredse)
        {
            switch (antalHovedkredse)
            {
                case int n when (n > 9):
                    return 0.5M;
                case int n when (n > 5 && n <= 9):
                    return 0.6M;
                case int n when (n > 3 && n <= 5):
                    return 0.7M;
                case int n when (n > 1 && n <= 3):
                    return 0.8M;
                default:
                    return 1;
            }
        }

        private static List<ElTavleSektionElKomponent> UgTavlerFromModuler(ElTavle eltavle, int sektionID)
        {
            List<ElTavleSektionElKomponent> list = new();

            int left = eltavle.Moduler;

            while (left > 0)
            {
                switch (eltavle.TavlefabrikatID)
                {
                    case (int)Tavlefabrikat.Ikke___angivet:
                    case (int)Tavlefabrikat.LK_UG_150:
                        {
                            switch (left)
                            {
                                case int n when (n > 36):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.UG24, KomponentKategori.LK_Ug_150));
                                    left -= 48;
                                    break;
                                case int n when (n > 24 && n <= 36):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.UG18, KomponentKategori.LK_Ug_150));
                                    left -= 36;
                                    break;
                                case int n when (n > 12 && n <= 24):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.UG12, KomponentKategori.LK_Ug_150));
                                    left -= 24;
                                    break;
                                case int n when (n <= 12):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.UG6, KomponentKategori.LK_Ug_150));
                                    left -= 12;
                                    break;
                            }
                        }
                        break;
                    case (int)Tavlefabrikat.LK_PGE___planforsænket_med_låg:
                        {
                            switch (left)
                            {
                                case int n when (n > 28):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.PGE21, KomponentKategori.LK_PGE_Planforsænket_med_låg));
                                    left -= 42;
                                    break;
                                case int n when (n > 14 && n <= 28):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.PGE14, KomponentKategori.LK_PGE_Planforsænket_med_låg));
                                    left -= 28;
                                    break;
                                case int n when (n <= 14):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.PGE7, KomponentKategori.LK_PGE_Planforsænket_med_låg));
                                    left -= 14;
                                    break;
                            }
                        }
                        break;
                    case (int)Tavlefabrikat.Hager_Gamma:
                        {
                            switch (left)
                            {
                                case int n when (n > 39):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HG52, KomponentKategori.Hager_Gamma));
                                    left -= 52;
                                    break;
                                case int n when (n > 26 && n <= 39):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HG39, KomponentKategori.Hager_Gamma));
                                    left -= 39;
                                    break;
                                case int n when (n > 13 && n <= 26):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HG26, KomponentKategori.Hager_Gamma));
                                    left -= 26;
                                    break;
                                case int n when (n <= 13):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HG13, KomponentKategori.Hager_Gamma));
                                    left -= 13;
                                    break;
                            }
                        }
                        break;
                    case (int)Tavlefabrikat.Hager___planforsænket_med_låg:
                        {
                            switch (left)
                            {
                                case int n when (n > 36):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HGV48, KomponentKategori.Hager_Volta_Planforsænket_med_låg));
                                    left -= 48;
                                    break;
                                case int n when (n > 24 && n <= 36):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HGV36, KomponentKategori.Hager_Volta_Planforsænket_med_låg));
                                    left -= 36;
                                    break;
                                case int n when (n > 12 && n <= 24):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HGV24, KomponentKategori.Hager_Volta_Planforsænket_med_låg));
                                    left -= 24;
                                    break;
                                case int n when (n <= 12):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HGV12, KomponentKategori.Hager_Volta_Planforsænket_med_låg));
                                    left -= 12;
                                    break;
                            }
                        }
                        break;
                    case (int)Tavlefabrikat.Schneider_Resi9:
                        {
                            switch (left)
                            {
                                case int n when (n > 39):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.SR52, KomponentKategori.Sneider_Resi9));
                                    left -= 52;
                                    break;
                                case int n when (n > 26 && n <= 39):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.SR39, KomponentKategori.Sneider_Resi9));
                                    left -= 39;
                                    break;
                                case int n when (n > 13 && n <= 26):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.SR26, KomponentKategori.Sneider_Resi9));
                                    left -= 26;
                                    break;
                                case int n when (n <= 13):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.SR13, KomponentKategori.Sneider_Resi9));
                                    left -= 13;
                                    break;
                            }
                        }
                        break;
                    case (int)Tavlefabrikat.Hager_Vector_IP_65:
                        {
                            switch (left)
                            {
                                case int n when (n > 36):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HV48, KomponentKategori.Hager_Vector_IP_65));
                                    left -= 48;
                                    break;
                                case int n when (n > 24 && n <= 36):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HV36, KomponentKategori.Hager_Vector_IP_65));
                                    left -= 36;
                                    break;
                                case int n when (n > 12 && n <= 24):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HV24, KomponentKategori.Hager_Vector_IP_65));
                                    left -= 24;
                                    break;
                                case int n when (n <= 12):
                                    list.Add(UGElKomponent(eltavle, sektionID, Komponent.HV12, KomponentKategori.Hager_Vector_IP_65));
                                    left -= 12;
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return list;
        }
        private static ElTavleSektionElKomponent UGElKomponent(ElTavle eltavle, int sektionID, Komponent komponent, KomponentKategori komponentKategori)
        {
            ElTavleSektionElKomponent e = new()
            {
                KundeID = eltavle.KundeID,
                ElTavleID = eltavle.Id,
                KomponentID = (int)komponent,
                ElTavleSektionID = sektionID,
                Placering = 0,
                ErExtraDisp = false,
                Line = 0,
                Navn = "",
                SidstRettet = DateTime.UtcNow,
            };
            return e;
        }

        public void UpdateModuler(int eltavleId)
        {
            _eltavlerRepository.UpdateModuler(eltavleId);
        }

        public void DeleteElTavleSektionKomponent(int id)
        {
            _eltavlerRepository.DeleteElTavleSektionKomponent(id);
        }

        public void InsertElTavleSektionKomponent(ElTavleSektionElKomponent entity)
        {
            _eltavlerRepository.InsertElTavleSektionKomponent(entity);
        }

        public int SektionKomponentMaxPlacering(int? tavleSektionId, int tavleId)
        {
            return _eltavlerRepository.SektionKomponentMaxPlacering(tavleSektionId, tavleId);
        }

        public async Task<int> CreateOrUpdateOrder(ElTavle eltavle)
        {
            var customerResult = await _economicHttpClient.GetEconomicCustomer(eltavle.KundeID).ConfigureAwait(false);

            var economicCustomerContacts = await _economicHttpClient.GetEconomicCustomerContacts(eltavle.KundeID);

            EconomicOrderLine[] lines = GetLines(eltavle);

            EconomicOrder economicOrder = OrderFromEltavle(eltavle, customerResult, (int)economicCustomerContacts.First().CustomerContactNumber, lines);

            var orderResult = await _economicHttpClient.CreateOrUpdateOrder(economicOrder, eltavle.EconomicOrderNumber).ConfigureAwait(false);
            if (orderResult.IsFailure)
            {
                throw new EconomicException(orderResult.Message);
            }

            eltavle.EconomicOrderNumber = orderResult.Value.OrderNumber;
            eltavle.SidstRettet = DateTime.UtcNow;
            eltavle.EconomicSidstRettet = eltavle.SidstRettet;

            Update(eltavle);

            return orderResult.Value.OrderNumber.Value;
        }

        public async Task<int> CreateOrUpdateInvoiceDraft(ElTavle eltavle)
        {
            var customerResult = await _economicHttpClient.GetEconomicCustomer(eltavle.KundeID).ConfigureAwait(false);
            var economicCustomerContacts = await _economicHttpClient.GetEconomicCustomerContacts(eltavle.KundeID);

            EconomicOrderLine[] lines = GetLines(eltavle);

            EconomicOrder economicOrder = OrderFromEltavle(eltavle, customerResult, (int)economicCustomerContacts.First().CustomerContactNumber, lines);

            var orderResult = await _economicHttpClient.CreateOrUpdateInvoiceDraft(economicOrder, eltavle.EconomicDraftInvoiceNumber).ConfigureAwait(false);
            if (orderResult.IsFailure)
            {
                throw new EconomicException(orderResult.Message);
            }

            eltavle.EconomicDraftInvoiceNumber = orderResult.Value.DraftInvoiceNumber;
            eltavle.SidstRettet = DateTime.UtcNow;
            eltavle.EconomicSidstRettet = eltavle.SidstRettet;
            Update(eltavle);

            return orderResult.Value.DraftInvoiceNumber.Value;
        }

        private EconomicOrderLine[] GetLines(ElTavle eltavle)
        {
            int noOflines = 2;
            var line = 0;

            if (eltavle.InitialRabat)
                noOflines++;

            var lines = new EconomicOrderLine[noOflines];

            var komponenter = GetAllSektionElKomponentsAntalDto(eltavle.Id);

            string description = @"Gruppetavle
Bestykning:
Seknr    Antal    KomponentNavn
";
            if (eltavle.TypeID == 2)
            {
                foreach (var item in komponenter)
                {
                    description += $"		{item.Sektion}										{item.Antal}								{item.Navn}" + Environment.NewLine;
                }

                description += Environment.NewLine;
            }
            else if (eltavle.TypeID == 1)
            {
                description = @"Forsyningstavle:" + Environment.NewLine + eltavle.Kommentar;
            }
            else if (eltavle.TypeID == 9)
            {
                description = eltavle.Kommentar;
            }

            lines[line] = new EconomicOrderLine() { LineNumber = line + 1, Description = description, Product = new EconomicProduct(eltavle.TypeID.ToString()), Quantity = eltavle.Antal, UnitNetPrice = eltavle.NettoPris };
            line++;

            if (eltavle.Fragt != 0)
            {
                lines[line] = new EconomicOrderLine() { LineNumber = line + 1, Description = "Fragt", Product = new EconomicProduct("10"), Quantity = 1, UnitNetPrice = eltavle.Fragt };
            }
            else
            {
                lines[line] = new EconomicOrderLine() { LineNumber = line + 1, Description = "Fragtfri", Product = new EconomicProduct("21"), Quantity = 1, UnitNetPrice = eltavle.Fragt };
            }

            if (eltavle.InitialRabat)
            {
                line++;
                lines[line] = new EconomicOrderLine() { LineNumber = line + 1, Description = "20% Rabat ved første køb via ElmoApp", Product = new EconomicProduct("20"), Quantity = eltavle.Antal, UnitNetPrice = Convert.ToInt32(eltavle.NettoPris * -0.2) };
            }

            return lines;
        }
        private static EconomicOrder OrderFromEltavle(ElTavle eltavle, EconomicCustomer customerResult, int customerContactNumber, EconomicOrderLine[] lines)
        {
            string tavler = "";
            string heading = "Tavle nr:";

            if (eltavle.TypeID == 9)
            {
                heading = "Ordrenr.:";
            }

            if (eltavle.Antal > 1)
            {
                tavler = $" -01 til -{eltavle.Antal:00} ({eltavle.Antal} stk.)";
            }

            var economicOrder = new EconomicOrder()
            {
                Layout = new EconomicOrderLayout() { LayoutNumber = 21, Self = new Uri("https://restapi.e-conomic.com/layouts/21") },
                Currency = "DKK",
                Customer = new(eltavle.KundeID),
                Date = DateTime.Today.ToString("yyyy-MM-dd"),
                PaymentTerms = new EconomicPaymentTerms(2),
                Recipient = new(customerResult),
                Lines = lines,
                Notes = new()
                {
                    Heading = $"{heading} {eltavle.TavleNr}{tavler}",
                    TextLine1 = $"Angivet adresse: {eltavle.Adresse}",
                    TextLine2 = ""
                },
                References = new()
                {
                    CustomerContact = new EconomicCustomerContactReference(eltavle.KundeID, customerContactNumber),
                    Other = eltavle.Rekvisition,
                }
            };
            return economicOrder;
        }

        public void FlytKunde(int eltavleId, int kundeId, int brugerId)
        {
            _eltavlerRepository.FlytKunde(eltavleId, kundeId, brugerId);
        }


        public void ImportKomponenter(int eltavleId, string indhold)
        {
            //_eltavlerRepository.FlytKunde(eltavleId, kundeId, brugerId);
        }

        

        public async Task<int> BookInvoice(ElTavle eltavle)
        {
            var bookResult = await _economicHttpClient.BookInvoice((int)eltavle.EconomicDraftInvoiceNumber).ConfigureAwait(false);
            if (bookResult.IsFailure)
            {
                throw new EconomicException(bookResult.Message);
            }

            eltavle.EconomicBookedInvoiceNumber = bookResult.Value.BookedInvoiceNumber;

            Update(eltavle);

            return bookResult.Value.BookedInvoiceNumber.Value;
        }

        public int NextLoebeNr(int aar, int typeId)
        {
            return _eltavlerRepository.NextLoebeNr(aar, typeId);
        }

        public void GemKomponentPlaceringer(int tavleId, IEnumerable<SektionElKomponentDto> komponentPlaceringer, bool fromFrontEnd = true)
        {
            int placering = 0;
            int[] dispIds = { 44, 45, 83 };

            if (fromFrontEnd)
            {
                //recalculate placeringer fra typescript array
                foreach (var dto in komponentPlaceringer)
                {
                    placering++;
                    dto.Placering = placering;
                }
                placering = 0;
            }

            // find sektions id/nr for disp
            var firstSektion = komponentPlaceringer.OrderBy(o => o.Placering).FirstOrDefault();

            int lastSektionID = firstSektion.Id;
            int lastSektionNr = firstSektion.Sektion;
            foreach (var item in komponentPlaceringer)
            {
                if (item.ElTavleSektionID > 0)
                {
                    lastSektionID = item.ElTavleSektionID;
                    lastSektionNr = (int)item.Sektion;
                }

                item.ElTavleSektionID = lastSektionID;
                item.Sektion = lastSektionNr;
            }

            List<ElTavleSektionElKomponent> dbplaceringer = new();

            if (fromFrontEnd)
            {
                SletExtraDispPlads(tavleId);
            }

            var placeringer = GetAllKomponentPlacering(tavleId);

            var first = placeringer.First();

            lastSektionNr = 0;

            int q = 1;
            int f = 1;
            int qf = 1;
            int p = 1;
            int d = 1;

            foreach (var dto in komponentPlaceringer.OrderBy(o => o.Sektion).ThenBy(o => o.Placering))
            {
                placering++;

                var dbItem = placeringer.Where(p => p.Id == dto.Id).FirstOrDefault();

                // re-create disp
                if (dbItem == null)
                {
                    dbItem = new ElTavleSektionElKomponent()
                    {
                        ElTavleID = first.ElTavleID,
                        ElTavleSektionID = dto.ElTavleSektionID,
                        KomponentID = dto.KomponentID,
                        KundeID = first.KundeID,
                        Placering = placering,
                        Navn = dto.Navn,
                        Line = dto.Line,
                        ErExtraDisp = dto.ErExtraDisp,
                        SidstRettet = DateTime.UtcNow,
                        AngivetNavn = dto.AngivetNavn,
                    };
                    dto.KategoriID = (int)KomponentKategori.Øvrige_komponenter;
                }
                else
                {
                    dbItem.Placering = placering;
                    lastSektionID = dbItem.ElTavleSektionID;
                }

                if (lastSektionNr != dto.Sektion)
                {
                    lastSektionNr = (int)dto.Sektion;
                    q = 1;
                    f = 1;
                    qf = 1;
                    p = 1;
                    d = 1;
                }

                if (dto.AngivetNavn)
                {
                    dbItem.Navn = dto.Navn;
                }
                else
                {
                    switch (dto.KategoriID)
                    {
                        case (int)KomponentKategori.D02_Forsikring:
                            dbItem.Navn = "F0";
                            break;
                        case (int)KomponentKategori.Transient_beskyttelse:
                            dbItem.Navn = "Q0";
                            break;
                        case (int)KomponentKategori.HPFI:
                            dbItem.Navn = $"Q{lastSektionNr}";
                            break;
                        case (int)KomponentKategori.Automat_sikringer_3pn:
                        case (int)KomponentKategori.Automat_sikringer_1pn:
                            dbItem.Navn = $"F{lastSektionNr}.{f}";
                            f++;
                            break;
                        case (int)KomponentKategori.Kombirelæ_3pn:
                        case (int)KomponentKategori.Kombirelæ_1pn:
                            dbItem.Navn = $"QF{lastSektionNr}.{qf}";
                            qf++;
                            break;
                        case (int)KomponentKategori.Øvrige_komponenter:
                            if (dto.KomponentID == 33 || dto.KomponentID == 55 || dto.KomponentID == 60) //Astro Ur el Bimåler 3p+n 63A
                            {
                                //dbItem.Navn = $"P{lastSektionNr}.???";
                                //p++;
                            }
                            if (!dto.ErExtraDisp && (dto.KomponentID == 44 || dto.KomponentID == 45 || dto.KomponentID == 83)) //Disp
                            {
                                dbItem.Navn = $"D{lastSektionNr}.{d}";
                                d++;
                            }
                            else
                            {
                                dbItem.AngivetNavn = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
                dbplaceringer.Add(dbItem);
            }

            using (var connection = ConnectionFactory.GetOpenConnection())
            using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                transaction.BulkMerge(dbplaceringer);
                transaction.Commit();
            }
        }

        public void GemLaageKomponentPlaceringer(int laageId, IEnumerable<LaageElKomponentDto> komponentPlaceringer, bool fromFrontEnd = true)
        {
            int placering = 0;
            int[] dispIds = { 44, 45, 83 };

            if (fromFrontEnd)
            {
                //recalculate placeringer fra typescript array
                foreach (var dto in komponentPlaceringer)
                {
                    placering++;
                    dto.Placering = placering;
                }
                placering = 0;
            }

            var laage = _eltavlerRepository.GetElTavleLaageById(laageId);

            //foreach (var item in komponentPlaceringer)
            //{
            //    if (item.ElTavleSektionID > 0)
            //    {
            //        lastSektionID = item.ElTavleSektionID;
            //        lastSektionNr = (int)item.Sektion;
            //    }

            //    item.ElTavleSektionID = lastSektionID;
            //    item.Sektion = lastSektionNr;
            //}

            List<ElTavleLaageElKomponent> dbplaceringer = new();

            //if (fromFrontEnd)
            //{
            //    SletExtraDispPlads(tavleId);
            //}

            var placeringer = GetAllLaageKomponentPlacering(laage.TavleID);

            //var first = placeringer.First();

            var lastSektionNr = 0;

            int q = 1;
            int f = 1;
//          int qf = 1;
            int p = 1;
            int d = 1;
            int row = 1;
            bool lastIsComp = false;

            foreach (var dto in komponentPlaceringer.OrderBy(o => o.Placering))
            {
                placering++;

                var dbItem = placeringer.Where(p => p.Id == dto.Id).FirstOrDefault();

                // re-create disp
                //if (dbItem == null)
                //{
                //    dbItem = new ElTavleLaageElKomponent()
                //    {
                //        ElTavleID = first.ElTavleID,
                //        ElTavleLaageId = laageId,
                //        TavleSektionNr = dto.TavleSektionNr,
                //        KomponentID = dto.KomponentID,
                //        KundeID = first.KundeID,
                //        Placering = placering,
                //        Navn = dto.Navn,
                //        Line = dto.Line,
                //        ErExtraDisp = dto.ErExtraDisp,
                //        AngivetNavn = dto.AngivetNavn,
                //    };
                //    dto.KategoriID = (int)KomponentKategori.Øvrige_komponenter;
                //}
                //else
                //{
                dbItem.ElTavleLaageId = laageId;
                    dbItem.Placering = placering;
                //}

                //if (dto.Id==31)
                //{
                //}

                //if (dto.AngivetNavn)
                //{
                //    dbItem.Navn = dto.Navn;
                //}
                //else
                {
                    var sektionNr = lastSektionNr;


                    switch ((KredsKomponentKategori)dto.KomponentKategoriId)
                    {
                        case KredsKomponentKategori.DISP:
                            break;
                        case KredsKomponentKategori.HPFI:
                             lastSektionNr++;
                            dbItem.Navn = $"Q{lastSektionNr}";
                            row = 1;
                            lastIsComp = false;
                            q++;
                            break;
                        case KredsKomponentKategori.Kombirelæ:
                            lastSektionNr++;
                            dbItem.Navn = $"QF{lastSektionNr}";
                            row = 1;
                            lastIsComp = false;
                            q++;
                            break;
                        case KredsKomponentKategori.Automat_sikring_3p:
                        case KredsKomponentKategori.Automat_sikring_1p:
                            dbItem.Navn = $"F{lastSektionNr}.{f}";
                            row = 2;
                            lastIsComp = false;
                            f++;
                            break;
                        case KredsKomponentKategori.Sikring:
                            break;
                        case KredsKomponentKategori.Måler:
                            break;
                        case KredsKomponentKategori.Transient_beskyttelse:
                            break;
                        case KredsKomponentKategori.Energi_maaler:
                            dbItem.Navn = $"D{lastSektionNr}{(f > 1 ? "." + (f - 1) : "")}.{d}";
                            d++;
                            break; 
                        case KredsKomponentKategori.Ur:
                            dbItem.Navn = $"D{lastSektionNr}{(f > 1 ? "." + (f - 1) : "")}.{d}";
                            if (!lastIsComp) row++;
                            lastIsComp = true;
                            d++;
                            break;
                        case KredsKomponentKategori.Kontaktor:
                            break;
                        case KredsKomponentKategori.Kiprelæ:
                            break;
                        case KredsKomponentKategori.MCB_lille:
                            break;
                        case KredsKomponentKategori.MCB:
                            break;
                        default:
                            break;
                    }


                    //switch (dto.KomponentKategoriId)
                    //{
                    //    //case (int)KredsKomponentKategori.D02_Forsikring:
                    //    //    //dbItem.Navn = "F0";
                    //    //    //dbItem.Row = 1;
                    //    //    break;
                    //    case (int)KredsKomponentKategori.Transient_beskyttelse:
                    //        //dbItem.Navn = "Q0";
                    //        //dbItem.Row = 1;
                    //        break;
                    //    case (int)KredsKomponentKategori.HPFI:
                           
                    //        break;
                    //    case (int)KredsKomponentKategori.Automat_sikring_3p:
                    //    case (int)KredsKomponentKategori.Automat_sikring_1p:
                    //        dbItem.Navn = $"F{lastSektionNr}.{f}";
                    //        row = 2;
                    //        lastIsComp = false;
                    //        f++;
                    //        break;
                    //    case (int)KredsKomponentKategori.Kombirelæ:
                           
                    //        break;
                    //    case (int)KredsKomponentKategori.Øvrige_komponenter:

                    //        if (!lastIsComp)
                    //            row++;

                    //        lastIsComp = true;

                    //        //dbItem.Row = 5;
                    //        if (dto.KomponentID == 33 || dto.KomponentID == 55 || dto.KomponentID == 60) //Astro Ur el Bimåler 3p+n 63A
                    //        {
                    //            //dbItem.Navn = $"P{lastSektionNr}.{p}";
                    //            dbItem.Navn = $"D{lastSektionNr}{(f > 1 ? "." + (f - 1) : "")}.{d}";
                    //            p++;
                    //        }
                    //        if (!dto.ErExtraDisp && (dto.KomponentID == 44 || dto.KomponentID == 45 || dto.KomponentID == 83)) //Disp
                    //        {
                    //            dbItem.Navn = $"D{lastSektionNr}.{d}";
                    //            d++;
                    //        }
                    //        else
                    //        {
                    //            dbItem.AngivetNavn = true;
                    //        }
                    //        break;
                    //    default:
                    //        break;
                    //}

                    dbItem.TavleSektionNr = lastSektionNr;
                    dbItem.Row = row;
                    if (sektionNr != lastSektionNr)
                    {
                        q = 1;
                        f = 1;
                        //qf = 1;
                        p = 1;
                        d = 1;
                    }

                }
                dbplaceringer.Add(dbItem);
            }

            using (var connection = ConnectionFactory.GetOpenConnection())
            using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                transaction.BulkMerge(dbplaceringer);
                transaction.Commit();
            }
        }


        public ElTavleSektionElKomponent GetElTavleSektionElKomponentById(int id)
        {
            return _eltavlerRepository.GetElTavleSektionElKomponentById(id);
        }

        public void UpdateElTavleSektionElKomponent(ElTavleSektionElKomponent entity)
        {
            _eltavlerRepository.UpdateElTavleSektionElKomponent(entity);
        }

        public IReadOnlyCollection<KomponentAntalEffektDto> GetAllElKomponentsAntalEffektsDto(int tavleId)
        {
            return _eltavlerRepository.GetAllElKomponentsAntalEffektsDto(tavleId);
        }

        public IReadOnlyCollection<KomponentAntalEffektDto> GetAllKabinetsAntalEffektsDto(int tavleId)
        {
            return _eltavlerRepository.GetAllKabinetsAntalEffektsDto(tavleId);
        }

     
    }
}
