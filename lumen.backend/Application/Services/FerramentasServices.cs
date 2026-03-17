namespace Application.Services
{
    public class FerramentasService : IFerramentasService
    {
        public byte[] GerarQrCode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("A URL ou Payload é obrigatória.");

            using var qrGenerator = new QRCoder.QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCoder.QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCoder.PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }

        public string GerarLinkWhatsapp(string numero, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("O número de telefone é obrigatório.");

            var numeroLimpo = new string(numero.Where(char.IsDigit).ToArray());

            if (numeroLimpo.Length <= 11)
                numeroLimpo = "55" + numeroLimpo;

            var msgEnc = WebUtility.UrlEncode(mensagem ?? string.Empty);

            return $"https://wa.me/{numeroLimpo}?text={msgEnc}";
        }

        public string GerarPixEstatico(string chave, string nomeRecebedor, string cidade, decimal? valor, string txtId = "***")
        {
            if (string.IsNullOrWhiteSpace(chave))
                throw new ArgumentException("Chave PIX é obrigatória.");

            if (string.IsNullOrWhiteSpace(nomeRecebedor))
                throw new ArgumentException("Nome do recebedor é obrigatório.");

            var payload = new StringBuilder();

            payload.Append(FormatarCampo("00", "01")); 

            var contaInfo = FormatarCampo("00", "br.gov.bcb.pix") + FormatarCampo("01", chave);
            payload.Append(FormatarCampo("26", contaInfo));

            payload.Append(FormatarCampo("52", "0000")); 
            payload.Append(FormatarCampo("53", "986"));  

            if (valor.HasValue && valor > 0)
                payload.Append(FormatarCampo("54", valor.Value.ToString("F2", CultureInfo.InvariantCulture)));

            payload.Append(FormatarCampo("58", "BR")); 
            payload.Append(FormatarCampo("59", LimparTexto(nomeRecebedor, 25))); 
            payload.Append(FormatarCampo("60", LimparTexto(cidade, 15))); 

            var additionalData = FormatarCampo("05", LimparTexto(txtId, 25));
            payload.Append(FormatarCampo("62", additionalData));

            payload.Append("6304"); 
            var crc = CalcularCrc16(payload.ToString());
            payload.Append(crc);

            return payload.ToString();
        }

        public GerarLinkCheckoutResponse GerarLinkCheckoutWhatsapp(GerarLinkCheckoutRequest request)
        {
            var sb = new StringBuilder();
            var cultureBR = new System.Globalization.CultureInfo("pt-BR");

            sb.AppendLine("*NOVO PEDIDO*");
            sb.AppendLine();

            sb.AppendLine("*Dados do Cliente*");
            sb.AppendLine($"Nome: {request.Nome}");
            sb.AppendLine($"Contato: {request.Telefone}");
            sb.AppendLine();

            sb.AppendLine("*Detalhes da Entrega*");
            sb.AppendLine($"Tipo: {(request.TipoEntrega == "pickup" ? "Retirada na Loja" : "Entrega no Endereço")}");

            if (request.TipoEntrega == "delivery" && !string.IsNullOrWhiteSpace(request.Endereco))
            {
                sb.AppendLine($"Endereço: {request.Endereco}");
            }
            sb.AppendLine();

            sb.AppendLine("*Pagamento*");
            sb.AppendLine($"Método escolhido: {request.Pagamento.ToUpper()}");
            sb.AppendLine();

            sb.AppendLine("*Itens do Pedido*");
            foreach (var item in request.Itens)
            {
                sb.AppendLine($"- {item.Quantidade}x {item.Nome} (R$ {item.Preco.ToString("N2", cultureBR)})");
            }

            sb.AppendLine();
            sb.AppendLine($"*TOTAL: R$ {request.Total.ToString("N2", cultureBR)}*");

            if (request.Pagamento.ToLower() == "pix" && !string.IsNullOrEmpty(request.ChavePixLoja))
            {
                try
                {
                    var pixCodigo = GerarPixEstatico(request.ChavePixLoja, request.Nome ?? "Recebedor", "Brasil", request.Total);
                    sb.AppendLine();
                    sb.AppendLine("PIX Copia e Cola:");
                    sb.AppendLine($"`{pixCodigo}`");
                }
                catch
                {
                }
            }

            sb.AppendLine();
            sb.AppendLine("Olá! Gostaria de confirmar meu pedido.");

            return new GerarLinkCheckoutResponse
            {
                LinkWhatsapp = GerarLinkWhatsapp(request.NumeroLoja, sb.ToString())
            };
        }
        #region Helpers

        private string FormatarCampo(string id, string valor) => $"{id}{valor.Length:D2}{valor}";

        private string LimparTexto(string texto, int limite)
        {
            if (string.IsNullOrEmpty(texto)) return "Vendedor";

            var semAcentos = new string(texto
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return semAcentos.Length > limite ? semAcentos.Substring(0, limite) : semAcentos;
        }

        private string CalcularCrc16(string payload)
        {
            ushort crc = 0xFFFF;
            var bytes = Encoding.UTF8.GetBytes(payload);
            foreach (var b in bytes)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0) crc = (ushort)((crc << 1) ^ 0x1021);
                    else crc <<= 1;
                }
            }
            return crc.ToString("X4").ToUpper();
        }

        #endregion
    }
}