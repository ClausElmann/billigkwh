using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace BilligKwhWebApp.Core.Toolbox
{
    public static class RSAKeyHelper
    {
        /// <summary>
        /// generate a random key for Authorization
        /// </summary>
        /// <returns></returns>
        public static RSAParameters GenerateKey()
        {
            using var key = new RSACryptoServiceProvider(2048);
            try
            {
                return key.ExportParameters(true);
            }
            finally
            {
                key.PersistKeyInCsp = false;
            }
        }

        /// <summary>
        /// Save and generate a random key for Authorization
        /// </summary>
        /// <param name="file">File name of the key</param>
        public static void GenerateKeyAndSave(string file)
        {
            var p = GenerateKey();
            RSAParametersWithPrivate t = new RSAParametersWithPrivate();
            t.SetParameters(p);
            File.WriteAllText(file, JsonSerializer.Serialize(t));
        }

        /// <summary>
        /// Read authorization key
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static RSAParameters GetKeyParameters(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("Check configuration - cannot find auth key file: " + file);
            RSAParametersWithPrivate keyParams = JsonSerializer.Deserialize<RSAParametersWithPrivate>(File.ReadAllText(file));
            return keyParams.ToRSAParameters();
        }

        private class RSAParametersWithPrivate
        {
            public byte[] D { get; set; }
            public byte[] DP { get; set; }
            public byte[] DQ { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] Modulus { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }

            public void SetParameters(RSAParameters p)
            {
                D = p.D;
                DP = p.DP;
                DQ = p.DQ;
                Exponent = p.Exponent;
                InverseQ = p.InverseQ;
                Modulus = p.Modulus;
                P = p.P;
                Q = p.Q;
            }

            public RSAParameters ToRSAParameters()
            {
                return new RSAParameters()
                {
                    D = this.D,
                    DP = this.DP,
                    DQ = this.DQ,
                    Exponent = this.Exponent,
                    InverseQ = this.InverseQ,
                    Modulus = this.Modulus,
                    P = this.P,
                    Q = this.Q

                };
            }
        }
    }

}
