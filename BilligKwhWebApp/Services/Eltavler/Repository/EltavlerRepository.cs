using Dapper;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using System.Collections.Generic;
using System.Linq;
using Z.Dapper.Plus;

namespace BilligKwhWebApp.Services.Eltavler.Repository
{
    public class EltavlerRepository : IEltavlerRepository
    {
        public IReadOnlyCollection<SektionElKomponentDto> GetAllSektionElKomponents(int tavleId, bool onlyPlacering = false)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<SektionElKomponentDto>(@"		
            SELECT ElTavleSektionElKomponent.ID, ElTavleSektion.Placering AS Sektion, ElKomponent.Navn AS KomponentNavn, ElKomponent.KategoriID, ElTavleSektionElKomponent.KomponentID, 
            ElTavleSektion.ID as ElTavleSektionID, ElTavleSektionElKomponent.ElTavleID, ElTavleSektionElKomponent.Line, ElKomponent.Modul, ElTavleSektionElKomponent.Navn, ElTavleSektionElKomponent.ErExtraDisp, ElTavleSektionElKomponent.Navn AS ServerNavn, ElTavleSektionElKomponent.Placering, ElTavleSektionElKomponent.AngivetNavn, ElKomponent.BruttoPris
            FROM ElTavleSektionElKomponent INNER JOIN
            ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
            ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID INNER JOIN
            ElTavleSektion ON ElTavleSektionElKomponent.ElTavleSektionID = ElTavleSektion.ID
            WHERE (ElTavleSektionElKomponent.ElTavleID = @TavleId) AND ((@OnlyPlacering = 1 AND (ElKomponent.Modul > 0 AND ElKomponent.DinSkinner = 0)) OR @OnlyPlacering = 0 )
            ORDER BY ElTavleSektion.Placering, ElTavleSektionElKomponent.Placering, ElKomponentKategori.TavlePlacering, ElKomponent.TavlePlacering",
            new { TavleId = tavleId, OnlyPlacering = onlyPlacering }).ToList();
        }

        public IReadOnlyCollection<LaageElKomponentDto> GetAllLaageElKomponents(int tavleid, int laageId, bool onlyPlacering = false)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<LaageElKomponentDto>(@"		
            SELECT ElTavleLaageElKomponent.ID, ElTavleLaage.Navn AS Sektion, ElKredsKomponent.Beskrivelse AS KomponentNavn, ElKredsKomponent.KomponentKategoriId, ElTavleLaageElKomponent.KomponentID, 
            ElTavleLaage.ID as ElTavleLaageID, ElTavleLaageElKomponent.ElTavleID, ElTavleLaageElKomponent.Line, ElKredsKomponent.Modul, ElTavleLaageElKomponent.Navn, ElTavleLaageElKomponent.ErExtraDisp, 
            ElTavleLaageElKomponent.Navn AS ServerNavn, ElTavleLaageElKomponent.Placering, ElTavleLaageElKomponent.AngivetNavn, ElTavleLaageElKomponent.Row
            FROM ElTavleLaageElKomponent INNER JOIN
            ElKredsKomponent ON ElTavleLaageElKomponent.KomponentID = ElKredsKomponent.ID INNER JOIN
            ElKredsKomponentKategori ON ElKredsKomponent.KomponentKategoriId = ElKredsKomponentKategori.ID LEFT JOIN
            ElTavleLaage ON ElTavleLaageElKomponent.ElTavleLaageID = ElTavleLaage.ID
            WHERE (ElTavleLaageElKomponent.ElTavleId = @Tavleid AND ElTavleLaageElKomponent.ElTavleLaageId IS NULL) OR (ElTavleLaageElKomponent.ElTavleLaageId = @LaageId)
            ORDER BY ElTavleLaageElKomponent.TavleSektionNr, ElTavleLaageElKomponent.Placering, ElKredsKomponentKategori.Placering",
            new { Tavleid = tavleid, LaageId = laageId, OnlyPlacering = onlyPlacering }).ToList();
        }

        public IReadOnlyCollection<ElTavleLaageElKomponent> GetAllElTavleLaageElKomponenter(int tavleid)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElTavleLaageElKomponent>(@"		
					SELECT * 
					FROM ElTavleLaageElKomponent
					WHERE ElTavleId = @Tavleid",
                   new { Tavleid = tavleid }).ToList();
        }

        public IReadOnlyCollection<ElTavleSektionElKomponent> GetAllForPlacering(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElTavleSektionElKomponent>(@"		
           	SELECT ElTavleSektionElKomponent.* 
            FROM   ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID LEFT OUTER JOIN
                         ElTavleSektion ON ElTavleSektionElKomponent.ElTavleSektionID = ElTavleSektion.ID
            WHERE (ElTavleSektionElKomponent.ElTavleID = @TavleId) AND (ElKomponent.Modul > 0 AND ElKomponent.DinSkinner = 0)
            ORDER BY ElTavleSektion.Placering, ElKomponentKategori.TavlePlacering, ElKomponent.Placering",
                    new { TavleId = tavleId }).ToList();
        }

        public IReadOnlyCollection<ElKredsKomponent> GetAllElKredsKomponenter()
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElKredsKomponent>(@"		
           	SELECT * from ElKredsKomponent", new {}).ToList();
        }

        public ElTavleDto GetDtoById(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElTavleDto>(@"
            SELECT top 1 Kunde.ID AS KundeID, Kunde.KundeNavn, Kunde.LandID, Kunde.EconomicId as EconomicCustomerNumber, ElTavle.*,
            (Select sum(OptjentBonus) -Sum(UdbetaltBonus) from ElTavle e WHERE e.Slettet=0 AND e.TavleNr is not null AND e.KundeID = ElTavle.KundeID AND e.ID <> ElTavle.ID) as BonusTilgode,
            (SELECT count(*) FROM Dokument WHERE RefTypeID = 6 AND RefGuid = ElTavle.ObjektGuid) as Billeder
            FROM ElTavle INNER JOIN
            Kunde ON ElTavle.KundeID = Kunde.ID
			WHERE 
            ElTavle.Id = @TavleId", new { TavleId = tavleId });
        }

        public IReadOnlyCollection<ElTavleSektionElKomponent> GetAllByTavle(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElTavleSektionElKomponent>(@"		
					SELECT * 
					FROM ElTavleSektionElKomponent
					WHERE ElTavleId = @TavleId",
                    new { TavleId = tavleId }).ToList();
        }


        public IReadOnlyCollection<KabinetKomponentDto> GetAllKabinetter(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<KabinetKomponentDto>(@"
            SELECT ElKomponent.Modul, ElKomponent.DinSkinner, ElKomponent.Navn AS KomponentNavn, ElKomponent.ID AS KomponentID
                        FROM   ElTavleSektionElKomponent INNER JOIN
                                     ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                                     ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID INNER JOIN
                                     ElTavleSektion ON ElTavleSektionElKomponent.ElTavleSektionID = ElTavleSektion.ID
                        WHERE (ElTavleSektionElKomponent.ElTavleID = @TavleId) AND (ElKomponentKategori.ErKabinet = 1)",
                    new { TavleId = tavleId }).ToList();
        }

        public IReadOnlyCollection<SektionKomponentAntalDto> GetAllSektionElKomponentsAntalDto(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<SektionKomponentAntalDto>(@"
            SELECT ElTavleSektion.Placering AS Sektion, ElKomponent.Navn, COUNT(*) AS Antal
            FROM   ElTavleSektionElKomponent INNER JOIN
                         ElTavleSektion ON ElTavleSektionElKomponent.ElTavleSektionID = ElTavleSektion.ID INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
            WHERE (ElTavleSektionElKomponent.ElTavleID = @TavleId)
            GROUP BY ElKomponent.Navn, ElTavleSektionElKomponent.KomponentID, ElTavleSektionElKomponent.ElTavleSektionID, ElTavleSektion.Placering, ElKomponentKategori.FakturaPlacering
            ORDER BY Sektion, ElKomponentKategori.FakturaPlacering",
                    new { TavleId = tavleId }).ToList();
        }

        public IReadOnlyCollection<KomponentAntalEffektDto> GetAllElKomponentsAntalEffektsDto(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<KomponentAntalEffektDto>(@"
            SELECT ElKomponent.Navn, ElKomponent.Effekt, COUNT(*) as Antal, COUNT(*) * ElKomponent.Effekt as IaltEffekt
            FROM ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
                    WHERE ElTavleSektionElKomponent.ElTavleID = @TavleId AND ElKomponentKategori.ErKabinet = 0
                    GROUP BY ElKomponent.Navn, ElKomponent.Effekt", new { TavleId = tavleId }).ToList();
        }

        public IReadOnlyCollection<KomponentAntalEffektDto> GetAllKabinetsAntalEffektsDto(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<KomponentAntalEffektDto>(@"
            SELECT ElKomponent.Navn, ElKomponent.Effekt, COUNT(*) as Antal, COUNT(*) * ElKomponent.Effekt as IaltEffekt
            FROM ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
                    WHERE ElTavleSektionElKomponent.ElTavleID = @TavleId AND ElKomponentKategori.ErKabinet = 1
                    GROUP BY ElKomponent.Navn, ElKomponent.Effekt", new { TavleId = tavleId }).ToList();
        }

        public IReadOnlyCollection<ElTavleDto> GetAllElTavleDto(string filter, string varetype)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElTavleDto>(@"
            SELECT Kunde.ID AS KundeID, Kunde.KundeNavn, Kunde.LandID, Kunde.EconomicId as EconomicCustomerNumber, ElTavle.*,
            (SELECT count(*) FROM Dokument WHERE RefTypeID = 6 AND RefGuid = ElTavle.ObjektGuid) as Billeder
            FROM ElTavle INNER JOIN
            Kunde ON ElTavle.KundeID = Kunde.ID
			WHERE 
            (
            (@Filter = 'Bestilte' AND ElTavle.BestiltDato is not null AND ElTavle.EconomicBookedInvoiceNumber is null AND ElTavle.Slettet = 0)
            OR
            (@Filter = 'Fakturerede' AND ElTavle.EconomicBookedInvoiceNumber is not null AND ElTavle.Slettet = 0)
            OR
            (@Filter = 'Slettede' AND ElTavle.Slettet = 1)
            OR
            (@Filter = 'Kladder' AND ElTavle.BestiltDato is null AND ElTavle.Slettet = 0)) 
            AND
            (
            (@Varetype = 'GruppeTavler' AND ElTavle.TypeID = 2)
            OR
            (@Varetype = 'FordelingsTavler' AND ElTavle.TypeID = 1)
            OR
            (@Varetype = 'Loesdele' AND ElTavle.TypeID = 9)
            )",
        new { Filter = filter, Varetype = varetype }).ToList();
        }

        public void SletExtraDispPlads(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();

            connection.Execute("DELETE FROM ElTavleSektionElKomponent WHERE ElTavleID = @TavleId AND ErExtraDisp = 1", new { TavleId = tavleId });
        }

        public IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionElKomponent(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElTavleSektionElKomponent>(@"		
					SELECT * 
					FROM ElTavleSektionElKomponent
					WHERE ElTavleId = @TavleId",
                   new { TavleId = tavleId }).ToList();
        }

        public void Update(ElTavle eltavle)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(eltavle);
        }

        public void Insert(ElTavle eltavle)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(eltavle);
        }
        public ElTavle GetById(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElTavle>(@"
            SELECT * FROM ElTavle
			WHERE Id = @TavleId", new { TavleId = tavleId });
        }

        public ElTavleLaage GetElTavleLaageById(int laageId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElTavleLaage>(@"
            SELECT * FROM ElTavleLaage
			WHERE Id = @LaageId", new { LaageId = laageId });
        }

        public IReadOnlyCollection<ElKomponentItemDto> AlleKomponenter()
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElKomponentItemDto>(@"
            SELECT ID, Navn 
            FROM ElKomponent
			WHERE Slettet = 0 Order By Navn", new { }).ToList();
        }

        public IReadOnlyCollection<ElKomponent> AlleElKomponenter()
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElKomponent>(@"
            SELECT * 
            FROM ElKomponent
			WHERE Slettet = 0", new { }).ToList();
        }

        public IReadOnlyCollection<ElTavleSektionDto> AlleSektioner(int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElTavleSektionDto>(@"
            SELECT ID, Placering, TypeID FROM ElTavleSektion
			WHERE TavleID = @TavleId", new { TavleId = tavleId }).ToList();
        }

        public ElTavleSektionElKomponent GetElTavleSektionKomponentById(int id)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElTavleSektionElKomponent>(@"
            SELECT * FROM ElTavleSektionElKomponent
			WHERE Id = @Id", new { Id = id });
        }

        public void UpdateElTavleSektionKomponent(ElTavleSektionElKomponent entity)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(entity);
        }

        public void InsertElTavleSektionKomponent(ElTavleSektionElKomponent entity)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(entity);
        }

        public void UpdateModuler(int eltavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.Execute(@"Update [dbo].[ElTavle]
            set 
            moduler = COALESCE((SELECT sum(ElKomponent.Modul) 
            FROM   ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
            WHERE (ElKomponentKategori.ErKabinet = 0) AND (ElTavleSektionElKomponent.ElTavleID = @Id)),0),
            kabinetModuler = COALESCE((SELECT sum(ElKomponent.Modul) 
            FROM   ElTavleSektionElKomponent INNER JOIN
                         ElKomponent ON ElTavleSektionElKomponent.KomponentID = ElKomponent.ID INNER JOIN
                         ElKomponentKategori ON ElKomponent.KategoriID = ElKomponentKategori.ID
            WHERE (ElKomponentKategori.ErKabinet = 1) AND (ElTavleSektionElKomponent.ElTavleID = @Id)),0)
            WHERE Id = @Id", new { Id = eltavleId });
        }

        public void DeleteElTavleSektionKomponent(int id)
        {
            using var connection = ConnectionFactory.GetOpenConnection();

            connection.Execute("DELETE FROM ElTavleSektionElKomponent WHERE ID = @Id", new { Id = id });
        }

        public int SektionKomponentMaxPlacering(int? tavleSektionId, int tavleId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();

            var maxPlacering = connection.QueryFirstOrDefault<int?>(@"select max(placering) 
                FROM ElTavleSektionElKomponent
                WHERE ElTavleID = @tavleId AND (@TavleSektionId is null OR ElTavleSektionID=@TavleSektionId)", new { TavleSektionId = tavleSektionId, TavleId = tavleId });

            if (maxPlacering == null)
                return 1;
            else
                return (int)maxPlacering + 1;
        }

        public void FlytKunde(int eltavleId, int kundeId, int brugerId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.Execute(@"Update [dbo].[ElTavle]
            set KundeID = @KundeId, OprettetAfBrugerID = @BrugerId
            WHERE ID = @eltavleId", new { KundeId = kundeId, EltavleId = eltavleId, BrugerId = brugerId });

            connection.Execute(@"Update [dbo].[ElTavleSektion]
            set KundeID = @KundeId
            WHERE TavleID = @eltavleId", new { KundeId = kundeId, EltavleId = eltavleId });

            connection.Execute(@"Update [dbo].[ElTavleSektionKomponent]
            set KundeID = @KundeId
            WHERE ElTavleID = @eltavleId", new { KundeId = kundeId, EltavleId = eltavleId });

            connection.Execute(@"Update [dbo].[ElTavleSektionElKomponent]
            set KundeID = @KundeId
            WHERE ElTavleID = @eltavleId", new { KundeId = kundeId, EltavleId = eltavleId });

            //connection.Execute(@"Update [dbo].[ElTavleSektionKomponentPlacering]
            //set KundeID = @KundeId
            //WHERE ElTavleID = @eltavleId", new { KundeId = kundeId, EltavleId = eltavleId });
        }

        public int NextLoebeNr(int aar, int typeId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            var nextLoebeNr = connection.QueryFirstOrDefault<int?>(@"select max(LoebeNr) 
                FROM ElTavle
                WHERE Aar = @Aar AND TypeId = @TypeId", new { Aar = aar, TypeId = typeId });
            if (nextLoebeNr == null)
                return 1;
            else
                return (int)nextLoebeNr + 1;
        }

        public ElTavleSektionElKomponent GetElTavleSektionElKomponentById(int id)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElTavleSektionElKomponent>(@"
            SELECT * FROM ElTavleSektionElKomponent
			WHERE Id = @Id", new { Id = id });
        }

        public void UpdateElTavleSektionElKomponent(ElTavleSektionElKomponent entity)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(entity);
        }


    }
}
