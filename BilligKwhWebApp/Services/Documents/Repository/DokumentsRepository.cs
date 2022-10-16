using Dapper;
using BilligKwhWebApp.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BilligKwhWebApp.Services.Dokuments.Repository
{
    public class DokumentsRepository : IDokumentsRepository
    {
        public IReadOnlyCollection<DokumentDto> GetAllElTavleDokumenter(int custormerId, int refTypeId, string refGuid)
        {
            using var connection = ConnectionFactory.GetOpenConnection();

            var list = connection.Query<DokumentDto>(@"
            SELECT  Dokument.Oprettet, '' as Base64Data, Dokument.FilData,Bruger.FuldtNavn, Bruger.Brugernavn FROM Dokument INNER JOIN
            Bruger ON Dokument.OprettetAfBrugerID = Bruger.ID WHERE KundeID = @CustormerId AND RefTypeID = @RefTypeID AND RefGuid = @RefGuid",
                    new { CustormerId = custormerId, RefTypeID = refTypeId, RefGuid = refGuid }).ToList();

            foreach (var item in list)
            {
                item.Base64Data = Convert.ToBase64String(item.FilData);
                item.FilData = Array.Empty<byte>();
            }

            return list;
        }

    }
}
