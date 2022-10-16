using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Core.Dto;
using Z.Dapper.Plus;
using System.Data;
using BilligKwhWebApp.Services.Enums;
using BilligKwhWebApp.Services.Invoicing.Economic.Customers;
using BilligKwhWebApp.Services.Invoicing;
using BilligKwhWebApp.Services.Komponenter.Dto;
using System.Collections;
using BilligKwhWebApp.Services.Economic.InvoicesPost;

namespace BilligKwhWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(UserRolePermissionProvider.Bearer)]
    public class EltavleController : BaseController
    {
        // Dependencies
        private readonly IWorkContext _workContext;
        private readonly IEltavleService _eltavleService;
        private readonly IDokumentService _dokumentService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IInvoicingService _invoicingService;
        private readonly IKomponentService _komponentService;
        private readonly IApplicationSettingService _applicationSettingService;

        // Ctor
        public EltavleController(
            ISystemLogger logger,
            IWorkContext workContext,
            IPermissionService permissionService,
            IEltavleService eltavleService,
            IDokumentService documentService,
            IEmailTemplateService emailTemplateService,
            ICustomerService customerService,
            IEmailService emailService,
            IUserService userService,
            IInvoicingService invoicingService,
            IKomponentService komponentService,
            IApplicationSettingService applicationSettingService) : base(logger, workContext, permissionService)
        {
            _workContext = workContext;
            _eltavleService = eltavleService;
            _dokumentService = documentService;
            _emailTemplateService = emailTemplateService;
            _customerService = customerService;
            _emailService = emailService;
            _userService = userService;
            _invoicingService = invoicingService;
            _komponentService = komponentService;
            _applicationSettingService = applicationSettingService;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesDefaultResponseType]
        public ActionResult<ElTavleDto> GetEltavle(int id)
        {
            ElTavleDto dto = _eltavleService.GetDtoById(id);

            if (dto == null) return BadRequest(new { ErrorMessage = "eltavle not found", WorkContext = _workContext });

            return Ok(dto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateEltavle([FromBody] ElTavleDto model)
        {
            if (model == null || model.Id == 0) return BadRequest("Model is null");

            var entity = _eltavleService.GetById(model.Id);

            bool isSuperAdmin = PermissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.SuperAdmin);

            // Only allow this if current customer is the one to update. If not, user must be super admin
            if (entity != null)
            {
                if (_workContext.CurrentCustomer.Id != entity.Id && !isSuperAdmin)
                    return ForbidWithMessage("User must be super admin to update a different customer than current!");

                bool genberegnKabinet = false;

                if (entity.TavlefabrikatID != model.TavlefabrikatID)
                {
                    genberegnKabinet = true;
                }

                entity.TavlefabrikatID = model.TavlefabrikatID;
                entity.Adresse = model.Adresse;
                entity.Rekvisition = model.Rekvisition;
                entity.BeregnetDato = model.BeregnetDato;
                entity.BestiltDato = model.BestiltDato;
                entity.KomponenterPris = model.KomponenterPris;
                entity.KomponenterInstallatoer = model.KomponenterInstallatoer;
                entity.TimeAntal = model.TimeAntal;
                entity.TimePris = model.TimePris;
                entity.DBFaktor = model.DBFaktor;
                entity.Fragt = model.Fragt;
                entity.PrisInclTimerOgDB = model.PrisInclTimerOgDB;
                entity.Kommentar = model.Kommentar;

                // don't delete if booked
                if (entity.EconomicBookedInvoiceNumber == null)
                {
                    if (model.Slettet && !entity.Slettet)
                    {
                        entity.TavleNr = null;
                        entity.LoebeNr = null;
                    }
                    else if (!model.Slettet && entity.Slettet && entity.Aar != null)
                    {
                        entity.LoebeNr = _eltavleService.NextLoebeNr(entity.Aar.Value, entity.TypeID);
                        entity.TavleNr = int.Parse($"{entity.TypeID}{entity.Aar.Value}{entity.LoebeNr.Value:000}");
                    }
                    entity.Slettet = model.Slettet;
                }

                entity.ObjektGuid = model.ObjektGuid;
                entity.OptjentBonus = model.OptjentBonus;
                entity.UdbetaltBonus = model.UdbetaltBonus;
                entity.Moduler = model.Moduler;
                entity.KabinetModuler = model.KabinetModuler;
                entity.Antal = model.Antal;

                entity.NettoPris = model.NettoPris;
                entity.InitialRabat = model.InitialRabat;
                entity.BonusGivende = model.BonusGivende;

                if (entity.BonusGivende)
                    entity.OptjentBonus = Convert.ToInt32(entity.NettoPris * 0.05);
                else
                    entity.OptjentBonus = 0;

                entity.MaerkeStroem = model.MaerkeStroem;
                entity.KapslingsKlasse = model.KapslingsKlasse;
                entity.DriftsSpaending = model.DriftsSpaending;
                entity.MaxKortslutningsStroem = model.MaxKortslutningsStroem;

                entity.OprettetAfBrugerID = model.OprettetAfBrugerID;
                entity.SidstRettet = DateTime.UtcNow;

                _eltavleService.Update(entity);

                if (genberegnKabinet)
                {
                    _eltavleService.GenberegnKabinetter(entity.Id, null);
                    _eltavleService.GenberegnKomponenterPris(entity.Id);
                }

                return Ok(entity.Id);
            }
            return BadRequest(new { ErrorMessage = "Eltavle not found", Model = entity });
        }


        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult OpretTavle(ElTavleDto eltavle)
        {

            ElTavle entity = new()
            {
                KundeID = eltavle.KundeID,
                TavlefabrikatID = 0,
                Adresse = eltavle.Adresse,
                Rekvisition = eltavle.Rekvisition,
                OprettetAfBrugerID = eltavle.OprettetAfBrugerID,
                BrugerDeviceID = 0,
                BeregnetDato = DateTime.UtcNow,
                BestiltDato = DateTime.UtcNow,
                KomponenterPris = 0,
                KomponenterInstallatoer = 0,
                TimeAntal = 0,
                TimePris = 0,
                DBFaktor = 1,
                Fragt = eltavle.Fragt,
                PrisInclTimerOgDB = 0,
                Kommentar = "",
                Slettet = false,
                ObjektGuid = Guid.NewGuid(),
                SidstRettet = DateTime.UtcNow,
                Moduler = 0,
                KabinetModuler = 0,
                Antal = eltavle.Antal,
                NettoPris = eltavle.NettoPris,
                InitialRabat = false,
                BonusGivende = false,
                TypeID = eltavle.TypeID,
                Aar = int.Parse(DateTime.Now.ToString("yy")),
                MaerkeStroem = eltavle.MaerkeStroem,
                KapslingsKlasse = eltavle.KapslingsKlasse,
                DriftsSpaending = eltavle.DriftsSpaending,
                MaxKortslutningsStroem = eltavle.MaxKortslutningsStroem,
            };

            entity.LoebeNr = _eltavleService.NextLoebeNr(entity.Aar.Value, entity.TypeID);
            entity.TavleNr = int.Parse($"{entity.TypeID}{entity.Aar.Value}{entity.LoebeNr.Value:000}");

            _eltavleService.Insert(entity);

            return Ok(entity.Id);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EltavleConfigurationDto))]
        public IActionResult GetEltavleConfiguration(int tavleId)
        {
            EltavleConfigurationDto dto = new();

            var kabinetter = _eltavleService.GetAllKabinetter(tavleId);

            dto.Komponenter = _eltavleService.GetAllSektionElKomponents(tavleId, true);

            dto.AntalSkinner = kabinetter.Sum(s => (s.DinSkinner));
            dto.ModulPrSkinne = kabinetter.FirstOrDefault().Modul / kabinetter.FirstOrDefault().DinSkinner;

            dto.ElTavle = _eltavleService.GetDtoById(tavleId);

            return Ok(dto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EltavleLaageConfigurationDto))]
        public IActionResult GetEltavleLaageConfiguration(int laageId)
        {
            EltavleLaageConfigurationDto dto = new();

            var laage = _eltavleService.GetElTavleLaageById(laageId);

            dto.Komponenter = _eltavleService.GetAllLaageElKomponents(laage.TavleID, laageId, true);

            dto.AntalSkinner = laage.DinSkinner;
            dto.ModulPrSkinne = laage.Bredde;

            dto.ElTavle = _eltavleService.GetDtoById(laage.TavleID);

            return Ok(dto);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SektionElKomponentDto>))]
        public IActionResult GetKomponentPlaceringer(int tavleId)
        {
            var placeringer = _eltavleService.GetAllSektionElKomponents(tavleId, true);

            if (placeringer != null)
            {
                return Ok(placeringer);
            }
            else
            {
                return Ok(new List<SektionElKomponentDto>());
            }
        }

        [HttpPost]
        public IActionResult GemKomponentPlaceringer(int tavleId, [FromBody] IEnumerable<SektionElKomponentDto> komponentPlaceringer)
        {
            if (!komponentPlaceringer.Any())
            {
                return Ok();
            }
            _eltavleService.GemKomponentPlaceringer(tavleId, komponentPlaceringer);
            return Ok();
        }

        [HttpPost]
        public IActionResult GemLaageKomponentPlaceringer(int laageId, [FromBody] IEnumerable<LaageElKomponentDto> komponentPlaceringer)
        {
            if (!komponentPlaceringer.Any())
            {
                return Ok();
            }
            _eltavleService.GemLaageKomponentPlaceringer(laageId, komponentPlaceringer);
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateFrame(int tavleId, int moduler)
        {
            var entity = _eltavleService.GetById(tavleId);

            if (entity != null && moduler >= entity.Moduler)
            {
                entity.Moduler = moduler;
                _eltavleService.Update(entity);
                _eltavleService.GenberegnKabinetter(tavleId, moduler);
            }
            return Ok();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateKomponentPlaceringNavn([FromBody] SektionElKomponentDto model)
        {
            if (model == null) return BadRequest("Model is null");

            var entity = _eltavleService.GetElTavleSektionElKomponentById(model.Id);

            if (entity != null)
            {
                entity.Navn = model.Navn;
                _eltavleService.UpdateElTavleSektionElKomponent(entity);
                return Ok(entity.Id);
            }
            return BadRequest(new { ErrorMessage = "KomponentPlacering not found", Model = entity });
        }


        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ElTavleDto>))]
        public IActionResult GetEltavler(string filter, string varetype, int? countryId, int? customerId)
        {
            var customers = _eltavleService.GetAllElTavleDto(filter, varetype);
            if (customers != null)
            {
                if (countryId.HasValue)
                    customers = customers.Where(c => c.LandID == countryId.Value).ToList();

                if (customerId.HasValue)
                    customers = customers.Where(c => c.KundeID == customerId.Value).ToList();

                return Ok(customers.OrderByDescending(c => c.BestiltDato));
            }
            else
            {
                return Ok(new List<CustomerModel>());
            }
        }

        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SektionElKomponentDto>))]
        public IActionResult GetAllSektionElKomponents(int tavleId)
        {
            var komponenter = _eltavleService.GetAllSektionElKomponents(tavleId);

            if (!komponenter.Where(k => k.Navn != "").Any())
            {
                _eltavleService.GemKomponentPlaceringer(tavleId, _eltavleService.GetAllSektionElKomponents(tavleId), true);
                komponenter = _eltavleService.GetAllSektionElKomponents(tavleId);
            }

            return Ok(komponenter);
        }

        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ElKomponentItemDto>))]
        public IActionResult AlleKomponenter()
        {
            var komponenter = _eltavleService.AlleKomponenter();
            return Ok(komponenter);
        }

        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ElKomponentDto>))]
        public IActionResult AlleElKomponenter(string filter, int komponentKategoriId)
        {
            var komponenter = _komponentService.AlleElKomponenter(filter, komponentKategoriId);
            return Ok(komponenter);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateSektionKomponent([FromBody] SektionElKomponentDto model)
        {
            if (model == null) return BadRequest("Model is null");

            if (model.Id == 0)
            {
                var eltavle = _eltavleService.GetById(model.ElTavleID);

                ElTavleSektionElKomponent komponent = new ElTavleSektionElKomponent()
                {
                    KundeID = eltavle.KundeID,
                    ElTavleID = eltavle.Id,
                    ElTavleSektionID = model.ElTavleSektionID,
                    KomponentID = model.KomponentID,
                    Placering = 1,
                    Navn = model.Navn,
                    ErExtraDisp = false,
                    Line = 0,
                    SidstRettet = DateTime.UtcNow,
                };

                if (model.ElTavleSektionID != 0)
                {
                    komponent.Placering = _eltavleService.SektionKomponentMaxPlacering(model.ElTavleSektionID, model.ElTavleID);
                }
                else
                {
                    komponent.Placering = _eltavleService.SektionKomponentMaxPlacering(null, model.ElTavleID);
                }
                _eltavleService.InsertElTavleSektionKomponent(komponent);
                _eltavleService.UpdateModuler(eltavle.Id);
                _eltavleService.GenberegnKomponenterPris(eltavle.Id);
                _eltavleService.GemKomponentPlaceringer(eltavle.Id, _eltavleService.GetAllSektionElKomponents(eltavle.Id, true), false);
                return Ok();
            }

            var entity = _eltavleService.GetElTavleSektionKomponentById(model.Id);

            if (entity != null)
            {
                entity.KomponentID = model.KomponentID;
                entity.ElTavleSektionID = model.ElTavleSektionID;
                entity.Navn = model.Navn;
                entity.AngivetNavn = model.AngivetNavn;
                _eltavleService.UpdateElTavleSektionKomponent(entity);
                _eltavleService.UpdateModuler(entity.ElTavleID);
                _eltavleService.GenberegnKomponenterPris(entity.ElTavleID);
                _eltavleService.GemKomponentPlaceringer(entity.ElTavleID, _eltavleService.GetAllSektionElKomponents(entity.ElTavleID, true), false);
                return Ok(entity.Id);
            }
            return BadRequest(new { ErrorMessage = "Eltavle not found", Model = entity });
        }

        [HttpDelete, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult DeleteSektionKomponent(int id)
        {
            var entity = _eltavleService.GetElTavleSektionKomponentById(id);

            if (entity != null)
            {
                _eltavleService.DeleteElTavleSektionKomponent(id);
                _eltavleService.UpdateModuler(entity.ElTavleID);
                _eltavleService.GenberegnKomponenterPris(entity.ElTavleID);
                _eltavleService.GemKomponentPlaceringer(entity.ElTavleID, _eltavleService.GetAllSektionElKomponents(entity.ElTavleID, true), false);
            }

            return Ok();
        }

        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ElTavleSektionDto>))]
        public IActionResult AlleSektioner(int tavleId)
        {
            var komponenter = _eltavleService.AlleSektioner(tavleId);
            return Ok(komponenter);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DokumentDto>))]
        public IActionResult GetElTavleDokumenter(int tavleId)
        {
            var eltavle = _eltavleService.GetById(tavleId);

            var data = _dokumentService.GetAllElTavleDokumenter(eltavle.KundeID, (int)RefType.ElTavle, eltavle.ObjektGuid.ToString());

            return Ok(data);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GenberegnKabinetter(int tavleId)
        {
            _eltavleService.GenberegnKabinetter(tavleId, null);
            _eltavleService.GenberegnKomponenterPris(tavleId);

            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult SendTavleMail(int tavleId, string emailTemplateName)
        {
            _ = Enum.TryParse(emailTemplateName, out EmailTemplateName emailTemplate);

            EmailTemplate mailTemplate = _emailTemplateService.GetTemplateByNameEnum(CountryConstants.DanishCountryId, emailTemplate);

            var tavle = _eltavleService.GetById(tavleId);

            var kunde = _customerService.Get(tavle.KundeID);

            var bruger = _userService.Get(tavle.OprettetAfBrugerID);

            var komponenter = _eltavleService.GetAllSektionElKomponentsAntalDto(tavleId);

            var table = "<table><tr><th class='tdcenter'>Antal</th><th class='tdright'>Tekst</th></tr>";

            // skal denne være per sektion?
            foreach (var item in komponenter)
            {
                table += $"<tr><td class='tdcenter'>{item.Antal} stk.</td><td class='tdright'>{item.Navn}</td></tr>";
            }
            table += "</table>";

            var fields = new List<KeyValuePair<string, object>>()
                        {
                            new KeyValuePair<string, object>("KUNDENAVN", kunde.Kundenavn),
                            new KeyValuePair<string, object>("ORDRENR", tavle.TavleNr),
                            new KeyValuePair<string, object>("KUNDEADRESSE", tavle.Adresse),
                            new KeyValuePair<string, object>("REKVNR", tavle.Rekvisition),
                            new KeyValuePair<string, object>("KOMMENTAR", tavle.Kommentar),
                            new KeyValuePair<string, object>("KOMPONENTER", table),
                        };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            _emailService.Save(
                customerId: kunde.Id,
                fromEmail: mailTemplate.FromEmail,
                fromName: mailTemplate.FromName,
                sendTo: bruger.Brugernavn,
                sendToName: bruger.FuldtNavn,
                replyTo: mailTemplate.ReplyTo,
                subject: mailTemplate.Subject,
                body: mailTemplate.Html,
                categoryEnum: EmailCategoryEnum.OrderMails,
                refTypeID: (int)RefType.ElTavle,
                refID: tavleId,
                mailTemplate.BccEmails);
            return Ok(true);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesDefaultResponseType]
        public ActionResult<EmailModel> GetVarmeberegning(int tavleId, string emailTemplateName)
        {
            _ = Enum.TryParse(emailTemplateName, out EmailTemplateName emailTemplate);

            EmailTemplate mailTemplate = _emailTemplateService.GetTemplateByNameEnum(CountryConstants.DanishCountryId, emailTemplate, EmailTemplateName.None);

            var tavle = _eltavleService.GetById(tavleId);

            if (tavle.InstalleretEffekt == 0 || tavle.Samtidighed == 0)
            {
                _eltavleService.GenberegnVarmetab(tavleId);
                tavle = _eltavleService.GetById(tavleId);
            }

            var kunde = _customerService.Get(tavle.KundeID);

            var bruger = _userService.Get(tavle.OprettetAfBrugerID);

            var komponenter = _eltavleService.GetAllElKomponentsAntalEffektsDto(tavleId);

            var kabinetter = _eltavleService.GetAllKabinetsAntalEffektsDto(tavleId);

            var table = "<table>" +
                //"<tr>" +
                //"<th class='tdcenter' colspan='4'><H2>Komponenter</H2></th>" +
                //"</tr>" +
                "<tr>" +
                "<th class='tdleft'>Komponenter</th><th class='tdcenter'>Effekt[W]</th><th class='tdcenter'>Antal</th><th class='tdcenter'>Effekt[W]</th></tr>";

            foreach (var item in komponenter)
            {
                table += $"<tr><td class='tdleft' style='white-space:nowrap;'>{item.Navn}</td><td class='tdcenter'>{item.Effekt}</td><td class='tdcenter'>{item.Antal}</td><td class='tdright'>{item.IaltEffekt}</td></tr>";
            }

            table += $"<tr><td class='tdleft' style='white-space:nowrap;' colspan='3'>Interne ledninger (15% af total)<td class='tdright'>{komponenter.Sum(s => s.Antal * s.Effekt) * 0.15M:0.00}</td></tr>";

            table += $"<tr><td colspan='4'><hr></td></tr>";

            table += $"<tr><td class='tdleft' style='white-space:nowrap;' colspan='3'>Total effekt uden Korrektionsfaktor<td class='tdright'>{komponenter.Sum(s => s.Antal * s.Effekt) * 1.15M:0.00}</td></tr>";

            table += $"<tr><td colspan='4'>&nbsp;</td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Samtidighedsfaktor <b>{tavle.Samtidighed}</b></td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Installerede effekt (T.E.U.K. x sam.fak.&sup2;) <b>{komponenter.Sum(s => s.Antal * s.Effekt) * 1.15M * tavle.Samtidighed * tavle.Samtidighed:0.00}W</b></td></tr>";

            table += $"<tr><td colspan='4'><hr></td></tr>";

            table += $"<tr><th class='tdleft'>Kabinetter</th><th class='tdcenter'>Effekt[W]</th><th class='tdcenter'>Antal</th><th class='tdcenter'>Effekt[W]</th></tr></tr>";

            foreach (var item in kabinetter)
            {
                table += $"<tr><td class='tdleft' style='white-space:nowrap;'>{item.Navn}</td><td class='tdcenter'>{item.Effekt}</td><td class='tdcenter'>{item.Antal}</td><td class='tdright'>{item.IaltEffekt}</td></tr>";
            }

            table += $"<tr><td colspan='4'>&nbsp;</td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Maks effektab i tavlen {kabinetter.Sum(s => s.Antal * s.Effekt)}W-10% = <b>{kabinetter.Sum(s => s.Antal * s.Effekt) * 0.9M:0.00}W</b></td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Rest effekt der kan installeres i tavle <b>{(kabinetter.Sum(s => s.Antal * s.Effekt) * 0.9M) - (komponenter.Sum(s => s.Antal * s.Effekt) * 1.15M * tavle.Samtidighed * tavle.Samtidighed):0.00}W</b></td></tr>";


            table += "</table>";

            var fields = new List<KeyValuePair<string, object>>()
                        {
                            new KeyValuePair<string, object>("KUNDENAVN", kunde.Kundenavn),
                            new KeyValuePair<string, object>("ORDRENR", tavle.TavleNr),
                            new KeyValuePair<string, object>("REKVNR", tavle.Rekvisition),
                            new KeyValuePair<string, object>("KOMPONENTER", table),
                        };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            var html = new EmailModel() { Body = mailTemplate.Html };
            {
            };

            return Ok(html);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult SendVarmeberegningMail(int tavleId, string emailTemplateName)
        {
            _ = Enum.TryParse(emailTemplateName, out EmailTemplateName emailTemplate);

            EmailTemplate mailTemplate = _emailTemplateService.GetTemplateByNameEnum(CountryConstants.DanishCountryId, emailTemplate);

            var tavle = _eltavleService.GetById(tavleId);

            var kunde = _customerService.Get(tavle.KundeID);

            var bruger = _userService.Get(tavle.OprettetAfBrugerID);

            var komponenter = _eltavleService.GetAllElKomponentsAntalEffektsDto(tavleId);

            var kabinetter = _eltavleService.GetAllKabinetsAntalEffektsDto(tavleId);

            var table = "<table>" +
                //"<tr>" +
                //"<th class='tdcenter' colspan='4'><H2>Komponenter</H2></th>" +
                //"</tr>" +
                "<tr>" +
                "<th class='tdleft'>Komponenter</th><th class='tdcenter'>Effekt[W]</th><th class='tdcenter'>Antal</th><th class='tdcenter'>Effekt[W]</th></tr>";

            foreach (var item in komponenter)
            {
                table += $"<tr><td class='tdleft' style='white-space:nowrap;'>{item.Navn}</td><td class='tdcenter'>{item.Effekt}</td><td class='tdcenter'>{item.Antal}</td><td class='tdright'>{item.IaltEffekt}</td></tr>";
            }

            table += $"<tr><td class='tdleft' style='white-space:nowrap;' colspan='3'>Interne ledninger (15% af total)<td class='tdright'>{komponenter.Sum(s => s.Antal * s.Effekt) * 0.15M:0.00}</td></tr>";

            table += $"<tr><td colspan='4'><hr></td></tr>";

            table += $"<tr><td class='tdleft' style='white-space:nowrap;' colspan='3'>Total effekt uden Korrektionsfaktor<td class='tdright'>{komponenter.Sum(s => s.Antal * s.Effekt) * 1.15M:0.00}</td></tr>";

            table += $"<tr><td colspan='4'>&nbsp;</td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Samtidighedsfaktor <b>{tavle.Samtidighed}</b></td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Installerede effekt (T.E.U.K. x sam.fak.&sup2;) <b>{komponenter.Sum(s => s.Antal * s.Effekt) * 1.15M * tavle.Samtidighed:0.00}W</b></td></tr>";

            table += $"<tr><td colspan='4'><hr></td></tr>";

            table += $"<tr><th class='tdleft'>Kabinetter</th><th class='tdcenter'>Effekt[W]</th><th class='tdcenter'>Antal</th><th class='tdcenter'>Effekt[W]</th></tr></tr>";

            foreach (var item in kabinetter)
            {
                table += $"<tr><td class='tdleft' style='white-space:nowrap;'>{item.Navn}</td><td class='tdcenter'>{item.Effekt}</td><td class='tdcenter'>{item.Antal}</td><td class='tdright'>{item.IaltEffekt}</td></tr>";
            }

            table += $"<tr><td colspan='4'>&nbsp;</td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Maks effektab i tavlen {kabinetter.Sum(s => s.Antal * s.Effekt)}W-10% = <b>{kabinetter.Sum(s => s.Antal * s.Effekt) * 0.9M:0.00}W</b></td></tr>";

            table += $"<tr><td class='tdleft' colspan='4'>Rest effekt der kan installeres i tavle <b>{(kabinetter.Sum(s => s.Antal * s.Effekt) * 0.9M) - (komponenter.Sum(s => s.Antal * s.Effekt) * 1.15M * tavle.Samtidighed):0.00}W</b></td></tr>";


            table += "</table>";

            var fields = new List<KeyValuePair<string, object>>()
                        {
                            new KeyValuePair<string, object>("KUNDENAVN", kunde.Kundenavn),
                            new KeyValuePair<string, object>("ORDRENR", tavle.TavleNr),
                            new KeyValuePair<string, object>("REKVNR", tavle.Rekvisition),
                            new KeyValuePair<string, object>("KOMPONENTER", table),
                        };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            _emailService.Save(
                customerId: kunde.Id,
                fromEmail: mailTemplate.FromEmail,
                fromName: mailTemplate.FromName,
                sendTo: bruger.Brugernavn,
                sendToName: bruger.FuldtNavn,
                replyTo: mailTemplate.ReplyTo,
                subject: mailTemplate.Subject,
                body: mailTemplate.Html,
                categoryEnum: EmailCategoryEnum.OrderMails,
                refTypeID: (int)RefType.ElTavle,
                refID: tavleId,
                mailTemplate.BccEmails);
            return Ok(true);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult SendFakturaMail(int tavleId)
        {
            EmailTemplate mailTemplate = _emailTemplateService.GetTemplateByNameEnum(CountryConstants.DanishCountryId, EmailTemplateName.Faktura);

            var tavle = _eltavleService.GetById(tavleId);

            var pdfData = _invoicingService.GetBookedInvoicePdf(tavle).Result;

            var kunde = _customerService.Get(tavle.KundeID);

            var fields = new List<KeyValuePair<string, object>>()
                        {
                            new KeyValuePair<string, object>("KUNDENAVN", kunde.Kundenavn),
                            new KeyValuePair<string, object>("REKVNR", tavle.Rekvisition),
                        };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            var attachments = new List<Tuple<byte[], string>>();
            if (pdfData != null)
            {
                var tp = new Tuple<byte[], string>(pdfData, $"faktura {tavle.EconomicBookedInvoiceNumber}.pdf");
                attachments.Add(tp);
            }

            _emailService.Save(
                customerId: kunde.Id,
                fromEmail: mailTemplate.FromEmail,
                fromName: mailTemplate.FromName,
                sendTo: kunde.FakturaMail,
                sendToName: kunde.FakturaKontaktPerson ?? kunde.Kundenavn,
                replyTo: mailTemplate.ReplyTo,
                subject: mailTemplate.Subject + $" - {tavle.EconomicBookedInvoiceNumber}",
                body: mailTemplate.Html,
                categoryEnum: EmailCategoryEnum.OrderMails,
                refTypeID: (int)RefType.ElTavle,
                refID: tavleId,
                mailTemplate.BccEmails,
                attachments);
            return Ok(true);
        }

        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult FlytKunde(int tavleId, int kundeId, int brugerId)
        {
            _eltavleService.FlytKunde(tavleId, kundeId, brugerId);

            return Ok();
        }


        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult ImportKomponenter(int tavleId, string indhold)
        {
            var tavle = _eltavleService.GetById(tavleId);

            string[] lines = indhold.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            var lls = new List<Tuple<int, string>>();

            for (int i = 0; i < lines.Length; i++)
            {
                var lineArray = lines[i].Split('\t');

                if (int.TryParse(lineArray[0], out int antal))
                {
                    lls.Add(new Tuple<int, string>(antal, lineArray[1]));
                }
            }

            var komponenter = _eltavleService.GetAllElKredsKomponenter();

            //int placering = 0;

            var list = new List<ElTavleLaageElKomponent>();

            foreach (var item in lls)
            {
                var komp = komponenter.Where(v => v.VareNummer == item.Item2).FirstOrDefault();

                if (komp != null)
                {
                    for (int i = 0; i < item.Item1; i++)
                    {
                        //placering++;
                        list.Add(new ElTavleLaageElKomponent()
                        {
                            ElTavleID = tavle.Id,
                            ElTavleLaageId = null,
                            TavleSektionNr = 0,
                            KomponentID = komp.Id,
                            KundeID = tavle.KundeID,
                            Placering = 0,
                            Navn = "",
                            Line = 0,
                            ErExtraDisp = false,
                            AngivetNavn = false,
                        });
                    }
                }
            }

            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(list);

            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult<int>> CreateOrUpdateOrder(int tavleId)
        {
            try
            {
                var eltavle = _eltavleService.GetById(tavleId);
                var kunde = _customerService.Get(eltavle.KundeID);

                if (kunde.EconomicId == null)
                {
                    return BadRequest("Kunden er ikke oprettet i e-conomic");
                }

                //var economicId = _eltavleService.CreateOrUpdateOrder(eltavle);
                //var task2 = _eltavleService.CreateOrUpdateInvoiceDraft(eltavle);
                //await Task.WhenAll(economicId, task2);

                var economicId = await _eltavleService.CreateOrUpdateOrder(eltavle).ConfigureAwait(false);
                _ = await _eltavleService.CreateOrUpdateInvoiceDraft(eltavle).ConfigureAwait(false);

                return Ok(economicId);
            }
            catch (EconomicException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateInvoiceDraft(int tavleId)
        {
            try
            {
                var eltavle = _eltavleService.GetById(tavleId);
                var kunde = _customerService.Get(eltavle.KundeID);

                if (kunde.EconomicId == null)
                {
                    return BadRequest("Kunden er ikke oprettet i e-conomic");
                }

                var economicId = await _eltavleService.CreateOrUpdateInvoiceDraft(eltavle).ConfigureAwait(false);

                return Ok(economicId);
            }
            catch (EconomicException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> BookInvoice(int tavleId)
        {
            try
            {
                var eltavle = _eltavleService.GetById(tavleId);

                //#if DEBUG
                //                eltavle.EconomicBookedInvoiceNumber = 123456;
                //                _eltavleService.Update(eltavle);
                //                return Ok(eltavle.EconomicBookedInvoiceNumber);
                //#endif
                var economicId = await _eltavleService.BookInvoice(eltavle).ConfigureAwait(false);

                return Ok(economicId);
            }
            catch (EconomicException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public IActionResult GetSentOrderPdf(int orderNumber)
        {
            var data = _invoicingService.GetSentOrderPdf(orderNumber).Result;

            if (data == null)
                return NoContent();

            string filename = $"{orderNumber}.pdf";
            string mime = "application/pdf";
            return File(data, mime, filename);
        }

        [HttpGet]
        public IActionResult GetDraftInvoicePdf(int tavleId)
        {
            var eltavle = _eltavleService.GetById(tavleId);

            var data = _invoicingService.GetDraftInvoicePdf(eltavle).Result;

            if (data == null)
                return NoContent();

            string filename = $"{eltavle.EconomicDraftInvoiceNumber}.pdf";
            string mime = "application/pdf";
            return File(data, mime, filename);
        }

        [HttpGet]
        public IActionResult GetBookedInvoicePdf(int tavleId)
        {
            var eltavle = _eltavleService.GetById(tavleId);

            var data = _invoicingService.GetBookedInvoicePdf(eltavle).Result;

            if (data == null)
                return NoContent();

            string filename = $"{eltavle.EconomicBookedInvoiceNumber}.pdf";
            string mime = "application/pdf";
            return File(data, mime, filename);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateElKomponent([FromBody] ElKomponentDto model)
        {
            if (model == null || model.Id == 0) return BadRequest("Model is null");

            var entity = _komponentService.GetById(model.Id);

            bool isSuperAdmin = PermissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.SuperAdmin);

            // Only allow this if current customer is the one to update. If not, user must be super admin
            if (entity != null)
            {
                if (_workContext.CurrentCustomer.Id != entity.Id && !isSuperAdmin)
                    return ForbidWithMessage("User must be super admin to update a different customer than current!");

                var loenPrMin = decimal.Parse(_applicationSettingService.Get(AppSettingEnum.KomponentTimeLoen, "").Setting) / 60;

                entity.Navn = model.Navn;
                entity.KategoriID = model.KategoriId;
                entity.Placering = model.Placering;
                entity.Modul = model.Modul;
                entity.DinSkinner = model.DinSkinner;
                entity.Slettet = model.Slettet;
                entity.KostKomponent = model.KostKomponent;
                entity.KostHjaelpeMat = model.KostHjaelpeMat;
                entity.KostLoen = Convert.ToInt32(model.MontageMinutter * loenPrMin);
                entity.DaekningsBidrag = model.DaekningsBidrag;
                entity.HPFI = model.HPFI;
                entity.KombiRelae = model.KombiRelae;
                entity.UdenBeskyttelse = model.UdenBeskyttelse;
                entity.MontageMinutter = model.MontageMinutter;
                entity.BruttoPris = Convert.ToInt32((entity.KostKomponent + entity.KostHjaelpeMat + entity.KostLoen) + ((entity.KostKomponent + entity.KostHjaelpeMat + entity.KostLoen) * 0.01 * entity.DaekningsBidrag));
                entity.SidstRettet = DateTime.UtcNow;
                entity.Effekt = model.Effekt;

                _komponentService.Update(entity);

                return Ok(ElKomponentDto.ConvertToDTO(entity));
            }
            return BadRequest(new { ErrorMessage = "Komponent not found", Model = entity });
        }


        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult GetKomponentTimeLoen()
        {
            var loen = _applicationSettingService.Get(AppSettingEnum.KomponentTimeLoen, "").Setting;

            return Ok(int.Parse(loen));
        }

        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult GemKomponentTimeLoen(int komponentTimeLoen)
        {
            var loen = _applicationSettingService.Get(AppSettingEnum.KomponentTimeLoen, "");

            loen.Setting = komponentTimeLoen.ToString();
            _applicationSettingService.Save(loen);

            var loenPrMin = decimal.Parse(komponentTimeLoen.ToString()) / 60;

            var kom = _eltavleService.AlleElKomponenter();

            var now = DateTime.UtcNow;

            foreach (var entity in kom)
            {
                entity.KostLoen = Convert.ToInt32(entity.MontageMinutter * loenPrMin);
                entity.SidstRettet = now;
                entity.BruttoPris = Convert.ToInt32((entity.KostKomponent + entity.KostHjaelpeMat + entity.KostLoen) + ((entity.KostKomponent + entity.KostHjaelpeMat + entity.KostLoen) * 0.01 * entity.DaekningsBidrag));
                _komponentService.Update(entity);
            }

            return Ok();
        }

        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult BestilTavle(int tavleId)
        {
            var entity = _eltavleService.GetById(tavleId);
            if (entity != null && entity.BestiltDato == null)
            {
                entity.BestiltDato = DateTime.UtcNow;
                entity.Aar = int.Parse(entity.BestiltDato.Value.ToString("yy"));
                entity.LoebeNr = _eltavleService.NextLoebeNr(entity.Aar.Value, entity.TypeID);
                entity.TavleNr = int.Parse($"{entity.TypeID}{entity.Aar.Value}{entity.LoebeNr.Value:000}");
                _eltavleService.Update(entity);
                return Ok(entity.Id);
            }
            else
            {
                return BadRequest(new { ErrorMessage = "Eltavle not found", Model = entity });
            }
        }
    }
}