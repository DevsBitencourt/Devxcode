﻿using System;
using QRCoder;
using System.IO;
using System.Drawing;

namespace Devxcode
{
    public partial class Devxcodes
    {
        /// <summary>
        /// Gera imagem de qrcode
        /// </summary>
        /// <param name="value">Valor a ser convertido em qrcode</param>
        /// <returns></returns>
        public  static Bitmap Imagem(string value)
        {
            try
            {
                var delegacao = new Devxcodes.Default(Devxcodes.MessageMandatoryDefault);

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(delegacao.Invoke(nameof(value)));

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(10);
                return qrCodeImage;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Gera o qrcode em Bytes
        /// </summary>
        /// <param name="value">Valor a ser convertido em qrcode</param>
        /// <returns></returns>
        public static byte[] GenerateByteArray(string value)
        {
            var image = Imagem(value);
            return ImageToByte(image);
        }

        /// <summary>
        /// Converte uma Imagem carregada em <paramref name="img"/> em byte[]
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static byte[] ImageToByte(Bitmap img)
        {
            using var stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }

        /// <summary>
        /// Salva o Qrcode em um diretorio
        /// </summary>
        /// <param name="img">Imagem</param>
        /// <param name="diretorio">Diretorio</param>
        /// <param name="filename">Nome do Arquivo. Caso informado vazio, será gerado nome randomico</param>
        /// <returns></returns>
        public static string SaveImagem(Bitmap img, string diretorio, string filename = "", string extensao = "bmp")
        {
            try
            {
                var delegacao = new Devxcodes.Default(Devxcodes.MessageMandatoryDefault);

                if (img == null)
                    throw new ArgumentNullException(delegacao.Invoke(nameof(img)));

                if (string.IsNullOrEmpty(diretorio))
                    throw new ArgumentNullException(delegacao.Invoke(nameof(diretorio)));

                if (string.IsNullOrEmpty(extensao))
                    throw new ArgumentNullException(delegacao.Invoke(nameof(extensao)));

                if (string.IsNullOrEmpty(filename))
                    filename = new Random().Next(1, 999999999).ToString();

                try
                {
                    if (!Directory.Exists(diretorio))
                        Directory.CreateDirectory(diretorio);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new UnauthorizedAccessException($"Sistema não possui permissão para criar pasta selecionada no diretório informando em {nameof(diretorio)}");
                }

                try
                {
                    img.Save($"{diretorio}\\{filename}.{extensao}");
                }
                catch (Exception e)
                {
                    throw new Exception($"Não foi possível gravar a imagem no diretório informado. Mensagem: {e.Message}");
                }
                return $"{diretorio}\\{filename}.{extensao}";

            }
            catch (UnauthorizedAccessException e)
            {
                var ErrorDelegacao = new Devxcodes.Default(Devxcodes.ErroDefault);
                throw new UnauthorizedAccessException(ErrorDelegacao.Invoke(e.Message));
            }
            catch (ArgumentNullException e)
            {
                var ErrorDelegacao = new Devxcodes.Default(Devxcodes.ErroDefault);
                throw new ArgumentNullException(ErrorDelegacao.Invoke(e.Message));
            }
            catch (ArgumentException e)
            {
                var ErrorDelegacao = new Devxcodes.Default(Devxcodes.ErroDefault);
                throw new ArgumentException(ErrorDelegacao.Invoke(e.Message));
            }
            catch (Exception e)
            {
                var ErrorDelegacao = new Devxcodes.Default(Devxcodes.ErroDefault);
                throw new Exception(ErrorDelegacao.Invoke(e.Message));
            }
        }
    }
}
