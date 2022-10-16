using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace BilligKwhWebApp.Services
{
    public static class ConnectionFactory
    {
        public static string ConnectionString = "";

  
        public static IDbConnection GetOpenConnection(string connString = "")
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConnectionString;
            }

            const int maxRetries = 4;
            int retryNo = 0;

            while (++retryNo <= maxRetries)
            {
                try
                {
                    var connection = new SqlConnection(connString);
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        return connection;
                    }
                }
                catch (Exception ex)
                {
                    if (retryNo < maxRetries)
                    {
                        Thread.Sleep(retryNo == 1 ? 1000 : (retryNo + 1) * 500);
                    }
                    if (retryNo > 3)
                    {
                        //UploadFileToAzureBlobcontainer(ex, retryNo);
                    }
                }
            }
            return null;
        }

        /* Bruges til undersøg. memory forbrug */
        public static void GetProcessInfos()
        {
            var proc = Process.GetCurrentProcess();
            var mem = proc.WorkingSet64;
            var cpu = proc.TotalProcessorTime;
            Debug.WriteLine("{0:n3} K of working set and CPU {1:n} sec. ", mem / 1024.0, cpu.TotalSeconds);
        }

    }
}
