using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Calculadora
{
    public partial class MainPage : ContentPage
    {
        double numeroActual = 0;
        double numeroAnterior = 0;
        string operadorActual;
        bool operadorPresionado = false;
        bool resultadoMostrado = false; // Variable para controlar si ya se mostró un resultado

        public MainPage()
        {
            InitializeComponent();
        }

        // Limpiar pantalla y reiniciar variables
        void Limpiar(object sender, EventArgs e)
        {
            numeroActual = 0;
            numeroAnterior = 0;
            Pantalla.Text = "0";
            operadorActual = null;
            resultadoMostrado = false; // Reiniciar el control de resultado mostrado
        }

        // Borrar el último dígito
        void Borrar(object sender, EventArgs e)
        {
            if (Pantalla.Text.Length > 1)
            {
                Pantalla.Text = Pantalla.Text.Substring(0, Pantalla.Text.Length - 1);
                numeroActual = double.Parse(Pantalla.Text);
            }
            else
            {
                Pantalla.Text = "0";
                numeroActual = 0;
            }
        }

        // Procesar número presionado
        void Numero(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            string numero = boton.Text;

            // Si se mostró un resultado previo, limpiar la pantalla antes de agregar el nuevo número
            if (Pantalla.Text == "0" || operadorPresionado || resultadoMostrado)
            {
                Pantalla.Text = numero;
                operadorPresionado = false;
                resultadoMostrado = false; // El nuevo número ya fue ingresado, dejar de mostrar el resultado anterior
            }
            else
            {
                Pantalla.Text += numero;
            }

            // Validar que no haya más de un punto decimal
            if (!double.TryParse(Pantalla.Text, out numeroActual))
            {
                Pantalla.Text = "0";
            }
        }

        // Manejar los operadores (+, -, *, /)
        void Operador(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            if (operadorActual != null && !operadorPresionado)
            {
                // Si ya hay un operador, calcular el resultado para encadenar operaciones
                Calcular(sender, e);
            }

            operadorActual = boton.Text;
            numeroAnterior = numeroActual;
            operadorPresionado = true;
        }

        // Realizar el cálculo cuando se presiona "="
        void Calcular(object sender, EventArgs e)
        {
            double resultado = 0;

            try
            {
                switch (operadorActual)
                {
                    case "+":
                        resultado = numeroAnterior + numeroActual;
                        break;
                    case "−":
                        resultado = numeroAnterior - numeroActual;
                        break;
                    case "×":
                        resultado = numeroAnterior * numeroActual;
                        break;
                    case "÷":
                        if (numeroActual == 0)
                        {
                            Pantalla.Text = "Error";
                            return;
                        }
                        resultado = numeroAnterior / numeroActual;
                        break;
                }
            }
            catch (Exception ex)
            {
                Pantalla.Text = "Error";
                return;
            }

            // Mostrar el resultado con punto decimal cuando sea necesario
            if (resultado % 1 == 0)
            {
                // Si el resultado es entero, mostrarlo sin decimales
                Pantalla.Text = resultado.ToString("F0");
            }
            else
            {
                // Si el resultado tiene decimales, mostrarlos
                Pantalla.Text = resultado.ToString("F2"); // Mostrar hasta 2 decimales
            }

            numeroActual = resultado;
            resultadoMostrado = true; // Indicar que ya se mostró un resultado
            operadorActual = null;
        }


        // Agregar un decimal
        void Decimal(object sender, EventArgs e)
        {
            if (!Pantalla.Text.Contains(","))
            {
                Pantalla.Text += ",";
            }
        }

        // Cambiar signo
        void CambiarSigno(object sender, EventArgs e)
        {
            numeroActual = -numeroActual;
            Pantalla.Text = numeroActual.ToString();
        }

        // Porcentaje
        void Porcentaje(object sender, EventArgs e)
        {
            numeroActual = numeroActual / 100;
            Pantalla.Text = numeroActual.ToString();
        }
    }
}
