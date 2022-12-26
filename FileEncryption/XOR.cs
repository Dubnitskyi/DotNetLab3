using System.ComponentModel;

namespace FileEncryption
{
    public class XOR
    {
        private string GetRepeatKey(string s, int n)
        {
            var r = s;
            while (r.Length < n)
            {
                r += r;
            }

            return r.Substring(0, n);
        }

        private string Cipher(string text, string secretKey, BackgroundWorker worker)
        {
            var currentKey = GetRepeatKey(secretKey, text.Length);
            var res = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                res += ((char)(text[i] ^ currentKey[i])).ToString();
                int percent = (int)(((i + 1.0) / text.Length) * 100);
                if (percent % 5 == 0) worker.ReportProgress(percent);
            }

            return res;
        }
        
        public string Encrypt(string plainText, string password, BackgroundWorker worker)
            => Cipher(plainText, password, worker);

        public string Decrypt(string encryptedText, string password, BackgroundWorker worker)
            => Cipher(encryptedText, password, worker);
    }
}
