using System.Text;

namespace TripYari.Core.Base.Providers.DIDGenerator
{
    public class DIDGeneratorProvider:IDIDGeneratorProvider
    {
        private  readonly char[] BASE31DIGITS = "0123456789BCDFGHJKLMNPQRSTVWXYZ".ToCharArray();
        private const int N_BASESIZE = 31;
        public  readonly DateTime DATE_112005 = new DateTime(2005, 1, 1);
        public  readonly DateTime DATE_2000 = new DateTime(2000, 1, 1);

        public  readonly char[] BASE32DIGITS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public const int DIDLENGTH = 20;
        public const int HASHKEYLENGTH = 30;

        public string GetNewUID()
        {
            var newGUID = Guid.NewGuid();
            var guidBytes = newGUID.ToByteArray();

            var n1 = new long();
            var didSB = new StringBuilder(20);

            n1 = n1 | guidBytes[7];
            n1 = n1 << 8;
            n1 = n1 | guidBytes[6];
            n1 = n1 << 8;
            n1 = n1 | guidBytes[5];
            n1 = n1 << 8;
            n1 = n1 | guidBytes[4];
            n1 = n1 << 8;
            n1 = n1 | guidBytes[3];
            n1 = n1 << 8;
            n1 = n1 | guidBytes[2];
            n1 = n1 << 8;
            n1 = n1 | guidBytes[1];
            n1 = n1 << 8;
            n1 = n1 | guidBytes[0];


            long longResult = 0;
            while (n1 > 0)
            {
                Math.DivRem(n1, Convert.ToInt64(N_BASESIZE), out longResult);
                didSB.Insert(0, Convert.ToString(BASE31DIGITS[Convert.ToInt32(longResult)]));
                n1 /= N_BASESIZE;
            }

            while (didSB.Length < 13)
            {
                didSB.Insert(0, "0");
            }

            n1 = guidBytes[8];
            n1 = n1 & 31;
            n1 <<= 8;
            n1 = n1 | guidBytes[9];

            //convert to base31
            while (n1 > 0)
            {
                Math.DivRem(n1, Convert.ToInt64(N_BASESIZE), out longResult);
                didSB.Insert(0, Convert.ToString(BASE31DIGITS[Convert.ToInt32(longResult)]));
                n1 /= N_BASESIZE;
            }

            while (didSB.Length < 16)
            {
                didSB.Insert(0, "0");
            }

            var smachID = "00";
            didSB.Insert(0, smachID);
            return didSB.ToString();
        }

        public string GenerateMachineSpecificKey(string prefix)
        {
            return GenerateMachineSpecificKey(prefix, false);
        }

        public string GenerateMachineSpecificKey(string prefix, bool useTimeCode)
        {
            if (useTimeCode)
            {
                return
                    $"{prefix}{((uint)Environment.MachineName.GetHashCode()):X}Z{TimeCodeReversed()}ABCDEFGHIJKLMNOPQRS"
                        .Substring(0, 20);
            }
            else
            {
                return $"{prefix}{((uint)Environment.MachineName.GetHashCode()):X}ABCDEFGHIJKLMNOPQRST".Substring(0, 20);
            }
        }

        private  string TimeCodeReversed()
        {
            var timeCodeArr = $"{DateTime.Now.Ticks:X}".ToCharArray();
            Array.Reverse(timeCodeArr);
            return new string(timeCodeArr);
        }
    }
}
