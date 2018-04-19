using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Run
{
    public partial class FrmRun : Form
    {
        public FrmRun()
        {
            InitializeComponent();
        }

        Thread hiloHora;
        Thread hiloMinuto;
        Thread hiloSegundo;

        int iHora = 0;
        int iMinuto = 0;
        int iSegundo = 0;

        private void btnRun_Click(object sender, EventArgs e)
        {

            hiloSegundo = new Thread(new ThreadStart(this.Segundo));

            hiloSegundo.Start();
        }

        public void Hora()
        {

            iHora++;

            if (iHora > 12)
            {
                iHora = 0;
            }

            this.Invoke((MethodInvoker)delegate
            {
                DibujarReloj(iSegundo, iMinuto, iHora);
            });
        }

        public void Minuto()
        {
            iMinuto++;

            if (iMinuto == 60)
            {
                iMinuto = 0;
                hiloHora = new Thread(new ThreadStart(this.Hora));
                hiloHora.Start();
            }

            this.Invoke((MethodInvoker)delegate
            {
                DibujarReloj(iSegundo, iMinuto, iHora);
            });
        }

        public void Segundo()
        {
            while (true)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    DibujarReloj(iSegundo, iMinuto, iHora);
                });

                Thread.Sleep(1000);

                iSegundo++;

                if (iSegundo == 60)
                {
                    iSegundo = 0;
                    hiloMinuto = new Thread(new ThreadStart(this.Minuto));
                    hiloMinuto.Start();
                }
            }
        }

        private void FrmRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hiloSegundo != null)
            {
                hiloSegundo.Abort();
            }
            if (hiloMinuto != null)
            {
                hiloMinuto.Abort();
            }
            if (hiloHora != null)
            {
                hiloHora.Abort();
            }

            Application.Exit();
        }

        int iManecillaSegundo = 140, iManecillaMinuto = 110, iManecillaHora = 80;
        int iCoordenadaY, iCoordenadaX;
        Bitmap bmpReloj = new Bitmap(303, 303);
        Graphics graficoReloj;  

        private void DibujarReloj(int ss, int mm, int hh)
        {
            iCoordenadaX = 150;
            iCoordenadaY = 150;

            //Crea un grafico a partir de una imagen BMP
            graficoReloj = Graphics.FromImage(bmpReloj);

            //Arreglo para la posicion de las manecillas
            int[] iCoordenadasManecillas = new int[2];

            //Limpia el grafico de manera trasparente
            graficoReloj.Clear(Color.Transparent);

            //Dibuja los numeros en el grafico
            graficoReloj.DrawString("01", new Font("Arial", 12), Brushes.Black, new PointF(218, 22));
            graficoReloj.DrawString("02", new Font("Arial", 12), Brushes.Black, new PointF(263, 70));
            graficoReloj.DrawString("03", new Font("Arial", 12), Brushes.Black, new PointF(278, 140));
            graficoReloj.DrawString("04", new Font("Arial", 12), Brushes.Black, new PointF(263, 212));
            graficoReloj.DrawString("05", new Font("Arial", 12), Brushes.Black, new PointF(218, 259));
            graficoReloj.DrawString("06", new Font("Arial", 12), Brushes.Black, new PointF(142, 279));
            graficoReloj.DrawString("07", new Font("Arial", 12), Brushes.Black, new PointF(70, 259));
            graficoReloj.DrawString("08", new Font("Arial", 12), Brushes.Black, new PointF(22, 212));
            graficoReloj.DrawString("09", new Font("Arial", 12), Brushes.Black, new PointF(1, 140));
            graficoReloj.DrawString("10", new Font("Arial", 12), Brushes.Black, new PointF(22, 70));
            graficoReloj.DrawString("11", new Font("Arial", 12), Brushes.Black, new PointF(70, 22));
            graficoReloj.DrawString("12", new Font("Arial", 12), Brushes.Black, new PointF(140, 3));
            
            //Dibuja Manecilla Segundos 
            iCoordenadasManecillas = CoordenadasMinutoSegundo(ss, iManecillaSegundo);
            graficoReloj.DrawLine(new Pen(Color.Red, 2f), new Point(iCoordenadaX, iCoordenadaY), new Point(iCoordenadasManecillas[0], iCoordenadasManecillas[1]));

            //Dibuja Manecilla Minutos 
            iCoordenadasManecillas = CoordenadasMinutoSegundo(mm, iManecillaMinuto);
            graficoReloj.DrawLine(new Pen(Color.Black, 3f), new Point(iCoordenadaX, iCoordenadaY), new Point(iCoordenadasManecillas[0], iCoordenadasManecillas[1]));

            //Dibuja Manecilla Horas
            iCoordenadasManecillas = CoordenadasHora(hh % 12, mm, iManecillaHora);
            graficoReloj.DrawLine(new Pen(Color.Black, 3f), new Point(iCoordenadaX, iCoordenadaY), new Point(iCoordenadasManecillas[0], iCoordenadasManecillas[1]));

            //Carga la imagen del relog en el PictureBox
            pictureBox1.Image = bmpReloj;

            //Libera la imagen para el cambio por segundo 
            graficoReloj.Dispose();
        }

        private int[] CoordenadasMinutoSegundo(int iValor, int iLongitudHora)
        {
            int[] iCoordenadas = new int[2];
            iValor *= 6;

            if (iValor >= 0 && iValor <= 100)
            {
                iCoordenadas[0] = iCoordenadaX + (int)(iLongitudHora * Math.Sin(Math.PI * iValor / 180));
                iCoordenadas[1] = iCoordenadaY - (int)(iLongitudHora * Math.Cos(Math.PI * iValor / 180));
            }
            else
            {
                iCoordenadas[0] = iCoordenadaX - (int)(iLongitudHora * -Math.Sin(Math.PI * iValor / 180));
                iCoordenadas[1] = iCoordenadaY - (int)(iLongitudHora * Math.Cos(Math.PI * iValor / 180));
            }
            return iCoordenadas;
        }

        private int[] CoordenadasHora(int iValorHora, int iValorMinuto, int iLongitudHora)
        {
            int[] iCoordenadas = new int[2];
            int iValor = (int)((iValorHora * 30) + (iValorMinuto * 0.5));

            if (iValor >= 0 && iValor <= 180)
            {
                iCoordenadas[0] = iCoordenadaX + (int)(iLongitudHora * Math.Sin(Math.PI * iValor / 180));
                iCoordenadas[1] = iCoordenadaY - (int)(iLongitudHora * Math.Cos(Math.PI * iValor / 180));
            }
            else
            {
                iCoordenadas[0] = iCoordenadaX - (int)(iLongitudHora * -Math.Sin(Math.PI * iValor / 180));
                iCoordenadas[1] = iCoordenadaY - (int)(iLongitudHora * Math.Cos(Math.PI * iValor / 180));
            }
            return iCoordenadas;
        }
    }
}
