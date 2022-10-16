using Dapper;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using BilligKwhWebApp.Services.Komponenter.Dto;
using System.Collections.Generic;
using System.Linq;
using Z.Dapper.Plus;

namespace BilligKwhWebApp.Services.Komponenter.Repository
{
    public class KomponenterRepository : IKomponenterRepository
    {
        public IReadOnlyCollection<ElKomponentDto> GetAllKomponentDto(string filter, int komponentKategoriId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElKomponentDto>(@"
            SELECT *
            FROM ElKomponent
            WHERE 
            (
             (@Filter = 'Slettede' AND Slettet = 1) OR (@Filter = 'Aktive' AND Slettet=0)
            ) 
            AND
            (
                (@KomponentKategoriId = 0 AND KategoriID <> @KomponentKategoriId OR KategoriID = @KomponentKategoriId )
            )",
        new { Filter = filter, KomponentKategoriId = komponentKategoriId }).ToList();
        }

        public ElKomponent GetById(int komponentId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElKomponent>(@"
            SELECT * FROM ElKomponent
			WHERE Id = @KomponentId", new { KomponentId = komponentId });
        }

        public ElKomponentDto GetDtoById(int komponentId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElKomponentDto>(@"
            SELECT top 1 *
            FROM ElKomponent
			WHERE Id = @KomponentId", new { komponentId = komponentId });
        }

        public void Insert(ElKomponent entity)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(entity);

        }

        public void Update(ElKomponent entity)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(entity);

        }
    }
}
