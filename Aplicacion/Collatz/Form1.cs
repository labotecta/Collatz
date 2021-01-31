using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Collatz
{
    public partial class Form1 : Form
    {
        private long bloque = 1048576;   // 2 ^ 20
        private readonly Label[] r = new Label[64];
        private readonly Label[] d = new Label[64];
        private readonly Label[] di = new Label[64];
        private readonly Label[] dr = new Label[64];
        private int nsec;
        private StringBuilder[] sec;
        private long nciclos;
        private long nimpares;
        private int nbitmas;
        private int nbitmenos;
        private long nimpbloque;
        private long maxnum;
        private long minnum;
        private long maximpar;
        private long entran;
        private long salen;
        private long estaban;
        private long npritab;
        private long impares_nuevos;
        private long trayectorias_completas;
        private bool cancelar;
        private bool omite = false;
        private bool evitar = false;

        private HashSet<long> tablaimpares;
        private long numini;
        private StreamWriter swres;
        private StreamWriter swresaux;

        private const int MAX_HILOS = 64;
        private int numero_hilos;
        private readonly object sync = new object();
        private readonly Thread[] hiloMente = new Thread[MAX_HILOS];
        private readonly bool[] finhiloMente = new bool[MAX_HILOS];
        private readonly long[,] intentoshilo = new long[MAX_HILOS, 2];
        private delegate void DelegadoActualizaEstadisticas(int i);
        private DelegadoActualizaEstadisticas delegadoActualizaEstadisticas = null;
        private delegate void DelegadoFinHilo();
        private DelegadoFinHilo delegadoFinHilo = null;
        private delegate void DelegadoMensaje(int i, string s);
        private DelegadoMensaje delegadoMensaje = null;
        private long max_intentos;
        private long intentos;
        private Dictionary<long, long>[] ultimos_impares;
        private Dictionary<long, long>[] primer_ultimos_impares;
        private readonly Dictionary<long, long> anti = new Dictionary<long, long>();

        public Form1()
        {
            InitializeComponent();
            SuspendLayout();
            int x = numero.Location.X + numero.Size.Width + 10;
            int y = numero.Location.Y;
            int ancho = 18;
            int alto = numero.Size.Height;
            Color c;
            for (int i = 0; i < 64; i++)
            {
                // Rótulo

                c = ((i + 6) / 10) % 2 == 0 ? Color.Blue : Color.Maroon;
                r[i] = new Label()
                {
                    Bounds = new Rectangle(x + i * ancho, y, ancho, alto),
                    Text = ((63 - i) % 10).ToString(),
                    AutoSize = false,
                    Name = "rotulo" + (63 - i).ToString(),
                    TabIndex = 64 + i,
                    ForeColor = c

                };
                Controls.Add(r[i]);

                // Número

                d[i] = new Label()
                {
                    Bounds = new Rectangle(x + i * ancho, y + alto, ancho - 1, alto),
                    Text = "",
                    AutoSize = false,
                    Name = "digito" + i.ToString(),
                    Tag = i,
                    TabIndex = i,
                    BackColor = Color.LightGray
                };
                d[i].Click += new EventHandler(CambiaBit);
                Controls.Add(d[i]);

                // No se muestra

                di[i] = new Label()
                {
                    Bounds = new Rectangle(x + i * ancho, y + 2 * alto, ancho, alto),
                    Text = string.Empty,
                    AutoSize = false,
                    Name = "intermedio" + i.ToString()
                };

                // Resultado

                dr[i] = new Label()
                {
                    Bounds = new Rectangle(x + i * ancho, y + 4 * alto, ancho, alto),
                    Text = "",
                    AutoSize = false,
                    Name = "resultado" + i.ToString()
                };
                Controls.Add(dr[i]);
            }
            ResumeLayout(false);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = string.Format("Collatz [3n + 1 = (2n) + n + 1]  V:{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Location = new Point(0, 0);
            b_cancelar.Enabled = false;
            delegadoActualizaEstadisticas = new DelegadoActualizaEstadisticas(ActualizaEstadisticas);
            delegadoFinHilo = new DelegadoFinHilo(FinHilo);
            delegadoMensaje = new DelegadoMensaje(MensajeHilo);
        }
        private void CambiaBit(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            int pos = Convert.ToInt32(l.Tag.ToString());
            Console.WriteLine(pos);
            if (d[pos].Text.Length == 0)
            {
                d[pos].Text = "1";
                for (int i = pos + 1; i < 64; i++)
                {
                    if (d[i].Text.Length == 0)
                    {
                        d[i].Text = "0";
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                int v = Convert.ToInt32(d[pos].Text);
                if (v == 0)
                {
                    d[pos].Text = "1";
                }
                else
                {
                    d[pos].Text = "0";
                    if (pos == 0 || string.IsNullOrEmpty(d[pos - 1].Text))
                    {
                        for (int i = pos; i < 64; i++)
                        {
                            if (d[i].Text.Equals("0"))
                            {
                                d[i].Text = String.Empty;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            long nn = BinarioAdecimal();
            evitar = true;
            numero.Text = nn.ToString();
        }
        private long BinarioAdecimal()
        {
            if (d[63].Text.Length == 0) return 0L;
            int k = 0;
            long nn = 0;
            for (int i = 63; i >= 0; i--)
            {
                if (d[i].Text.Length == 0) break;
                int v = Convert.ToInt32(d[i].Text);
                if (v == 1) nn += (long)Math.Pow(2, k);
                k++;
            }
            return nn;
        }
        private void Habilita(bool que)
        {
            numero.Enabled = que;
            borraycalcula.Enabled = que;
            busca.Enabled = que;
            recopila_impares.Enabled = que;
            terminaciones.Enabled = que;
            n_hilos.Enabled = que;
            gen_11.Enabled = que;
            paso.Enabled = que;
            borra.Enabled = que;
            todo.Enabled = que;
            salva.Enabled = que;
            lista.Enabled = que;
            listab.Enabled = que;
            listac.Enabled = que;
            listabc.Enabled = que;
        }
        private void Paso_Click(object sender, EventArgs e)
        {
            long n;
            if (numero.Text.Trim().Length == 0)
            {
                MessageBox.Show("No hay número");
                return;
            }
            try
            {
                n = Convert.ToInt64(numero.Text.Trim());
                if (n == 0)
                {
                    MessageBox.Show("Hemos terminado");
                    return;
                }
            }
            catch
            {
                MessageBox.Show(numero.Text.Trim() + " no es un número");
                return;
            }
            nsec = 0;
            sec = new StringBuilder[1];
            Habilita(false);
            Calcula(n);
            Habilita(true);
        }
        private void Todo_Click(object sender, EventArgs e)
        {
            Todo();
        }
        private void Todo()
        {
            long n;
            if (numero.Text.Trim().Length == 0)
            {
                MessageBox.Show("No hay número");
                return;
            }
            try
            {
                n = Convert.ToInt64(numero.Text.Trim());
                if (n == 0)
                {
                    MessageBox.Show("Hemos terminado");
                    return;
                }
            }
            catch
            {
                MessageBox.Show(numero.Text.Trim() + " no es un número");
                return;
            }
            nsec = 0;
            sec = new StringBuilder[2048];
            long numt = n;
            while (numt % 2 == 0) numt /= 2;
            StringBuilder st = new StringBuilder();
            st.Append(Binario(numt, di));
            sec[nsec++] = st;
            nciclos = 0;
            nimpares = 0;
            nbitmas = 0;
            nbitmenos = 0;
            maxnum = 0;
            maximpar = 0;
            Habilita(false);
            do
            {
                n = Calcula(n);

            } while (n > 1);
            lista.Items.Add(string.Format("{0:N0}", nciclos));
            listab.Items.Add(string.Format("{0:N0} ciclos, valor máximo {1:N0}", nciclos, maxnum));
            lista.TopIndex = lista.Items.Count - 1;
            listab.TopIndex = listab.Items.Count - 1;
            listac.Items.Add(string.Format("{0:N0}", nimpares));
            listabc.Items.Add(string.Format("{0:N0} impares, valor máximo {1:N0}", nimpares, maximpar));
            listac.TopIndex = listac.Items.Count - 1;
            listabc.TopIndex = listabc.Items.Count - 1;
            Habilita(true);
            Console.Beep();
        }
        private long Calcula(long num)
        {
            int nb;
            long ni;
            string s;
            string op;
            StringBuilder st;
            s = Binario(num, d);
            if (lista.Items.Count == 0)
            {
                lista.Items.Add(num);
                listab.Items.Add(s + "Ini");

                listac.Items.Add(num);
                listabc.Items.Add(s + "Ini");
            }
            nciclos++;
            if (maxnum < num) maxnum = num;
            if (num % 2 == 0)
            {
                // Par

                num /= 2;
                s = Binario(num, dr);
                nbitmenos++;
                listab.Items.Add(s + " /2" + string.Format(" {0,5:D} {1,3:D}+ {2,3:D}- {3,3:D}", nciclos, nbitmas, nbitmenos, nbitmas - nbitmenos));
            }
            else
            {
                // Impar

                nimpares++;
                if (maximpar < num) maximpar = num;
                if (lista.Items.Count > 1)
                {
                    listac.Items.Add(num);
                    listabc.Items.Add(Binario(num, dr));
                }

                // n = n * 3 + 1;
                // n = n * 2 + n + 1
                // Como es par inmediatamente se divide por 2, n = n + n / 2

                // Tambien se cumple con 

                // n = n * 2 - n + 1 = n + 1

                nb = CuentaBits(num);
                ni = num;
                num *= 2;
                s = Binario(num, di);
                lista.Items.Add("x 2");
                listab.Items.Add(s + " x2");
                if (original.Checked)
                {
                    op = "+";
                    num += ni;
                }
                else
                {
                    op = "-";
                    num -= ni;
                }
                s = Binario(num, di);
                lista.Items.Add(op + " n");
                listab.Items.Add(s + " " + op + "n");
                num++;
                nbitmas += CuentaBits(num) - nb;
                s = Binario(num, dr);
                listab.Items.Add(s + " +1" + string.Format(" {0,5:D} {1,3:D}+ {2,3:D}- {3,3:D}", nciclos, nbitmas, nbitmenos, nbitmas - nbitmenos));
                st = new StringBuilder();
                //st.Append(Binario(num /= 2, di));
                st.Append(Binario(num / 2, di));
                sec[nsec++] = st;

                //listac.Items.Add(num);
                //listabc.Items.Add(s);
            }
            lista.Items.Add(num);
            numero.Text = num.ToString();
            Binario(num, d);
            lista.TopIndex = lista.Items.Count - 1;
            listab.TopIndex = listab.Items.Count - 1;
            listac.TopIndex = listac.Items.Count - 1;
            listabc.TopIndex = listabc.Items.Count - 1;
            return num;
        }
        private int CuentaBits(long n)
        {
            if (n == 0) return 1;

            int k = 0;
            while (n > 0)
            {
                k++;
                n /= 2;
            }
            return k;
        }
        private string Binario(long n)
        {
            if (n == 0) return "0";
            const int np = 64;
            int[] destino = new int[np];
            int i = -1;
            while (n > 0)
            {
                destino[++i] = (int)(n % 2);
                n >>= 1;
            }
            StringBuilder sb = new StringBuilder();
            for (int j = i; j >= 0; j--)
            {
                sb.Append(destino[j]);
            }
            return sb.ToString();
        }
        private string Binario(long n, Label[] destino)
        {
            int np = 63;
            int i = np;
            if (n == 0)
            {
                destino[i--].Text = "0";
            }
            else
            {
                while (n > 0)
                {
                    destino[i--].Text = (n % 2).ToString();
                    n >>= 1;
                }
            }
            for (int j = i; j >= 0; j--) destino[j].Text = string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < np + 1; j++)
            {
                if (destino[j].Text.Length > 0)
                {
                    sb.Append(destino[j].Text).Append(" ");
                }
                else
                {
                    sb.Append("  ");
                }
            }
            return sb.ToString();
        }
        private void Borra_Click(object sender, EventArgs e)
        {
            BorraListas();
        }
        private void BorraListas()
        {
            lista.Items.Clear();
            listab.Items.Clear();
            listac.Items.Clear();
            listabc.Items.Clear();
        }
        private void Salva_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog ficheroescritura = new SaveFileDialog()
            {
                Filter = "CSV (*.csv)|*.csv|TODOS (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false,
                OverwritePrompt = false,
                CheckFileExists = false,
                CheckPathExists = true
            })
            {
                if (ficheroescritura.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileStream fres = new FileStream(ficheroescritura.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                        if (fres == null || !fres.CanWrite)
                        {
                            MessageBox.Show("Error. No se puede escribir en el fichero " + ficheroescritura.FileName);
                        }
                        else
                        {
                            swres = new StreamWriter(fres, Encoding.UTF8);
                            if (original.Checked)
                            {
                                swres.WriteLine(" 3 * n + 1");
                            }
                            else
                            {
                                swres.WriteLine(" n + 1");
                            }
                            for (int i = 0; i < nsec; i++)
                            {
                                swres.WriteLine(sec[i]);
                            }
                            swres.WriteLine("");
                            for (int i = 0; i < listab.Items.Count; i++)
                            {
                                if (!lista.Items[i].ToString().StartsWith("/") && !lista.Items[i].ToString().StartsWith("x") && !lista.Items[i].ToString().StartsWith("+") && !lista.Items[i].ToString().StartsWith("-"))
                                {
                                    swres.WriteLine("{0,-16} {1}", lista.Items[i].ToString(), listab.Items[i].ToString());
                                }
                            }
                            swres.WriteLine("");
                            for (int i = 0; i < listab.Items.Count; i++)
                            {
                                swres.WriteLine("{0,-16} {1}", lista.Items[i].ToString(), listab.Items[i].ToString());
                            }
                            swres.WriteLine("");
                            for (int i = 0; i < listabc.Items.Count; i++)
                            {
                                swres.WriteLine("{0,-16} {1}", listac.Items[i].ToString(), listabc.Items[i].ToString());
                            }
                            swres.Close();
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("Error. " + ee.Message + ". Fichero " + ficheroescritura.FileName);
                    }
                }
            }
        }
        private void Lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (omite) return;
            omite = true;
            if (listab.Items.Count >= lista.TopIndex && listab.Items.Count >= lista.SelectedIndex)
            {
                listab.TopIndex = lista.TopIndex;
                listab.SelectedIndex = lista.SelectedIndex;
            }
            omite = false;
        }
        private void Listab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (omite) return;
            omite = true;
            if (lista.Items.Count >= listab.TopIndex && lista.Items.Count >= listab.SelectedIndex)
            {
                lista.TopIndex = listab.TopIndex;
                lista.SelectedIndex = listab.SelectedIndex;
            }
            omite = false;
        }
        private void Busca_Click(object sender, EventArgs e)
        {
            long n;
            if (numero.Text.Trim().Length == 0)
            {
                MessageBox.Show("No hay número");
                return;
            }
            try
            {
                n = Convert.ToInt64(numero.Text.Trim());
                if (n == 0)
                {
                    MessageBox.Show("Hemos terminado");
                    return;
                }
            }
            catch
            {
                MessageBox.Show(numero.Text.Trim() + " no es un número");
                return;
            }
            long maxiteraciones = -1;
            long nummaxiteraciones = 0;
            int maxbitmas = 0;
            int maxbitmenos = 0;
            listab.Items.Add(string.Format("{0,15} {1,5} {2,5} {3,5} {4,5} {5,5} {6,15} {7,15}", "N", "Bits", "Ciclo", "+", "-", "Dif", "Máximo", "Máximo impar"));
            Habilita(false);
            cancelar = false;
            b_cancelar.Enabled = true;
            while (!cancelar)
            {
                if (n % 65536 == 0)
                {
                    evitar = true;
                    numero.Text = string.Format("{0}", n);
                    Application.DoEvents();
                }
                nciclos = 0;
                nbitmas = 0;
                nbitmenos = 0;
                maxnum = 0;
                maximpar = 0;
                SoloCalcula(n);
                if (maxiteraciones < nciclos)
                {
                    maxiteraciones = nciclos;
                    nummaxiteraciones = n;
                    maxbitmas = nbitmas;
                    maxbitmenos = nbitmenos;
                    listab.Items.Add(string.Format("{0,15:N0} {1,5:D} {2,5:D} {3,5:D} {4,5:D} {5,5:D} {6,15:N0} {7,15:N0}", n, CuentaBits(n), nciclos, nbitmas, nbitmenos, nbitmas - nbitmenos, maxnum, maximpar));
                    listab.TopIndex = listab.Items.Count - 10;
                    Application.DoEvents();
                }
                n++;
            }
            listab.Items.Add(".");
            listab.Items.Add(string.Format("{0,15} {1,5} {2,5} {3,5} {4,5} {5,5}", "N", "Bits", "Ciclo", "+", "-", "Dif"));
            listab.Items.Add(string.Format("{0,15:N0} {1,5:D} {2,5:D} {3,5:D} {4,5:D} {5,5:D}", nummaxiteraciones, CuentaBits(nummaxiteraciones), maxiteraciones, maxbitmas, maxbitmenos, maxbitmas - maxbitmenos));
            listab.TopIndex = listab.Items.Count - 10;
            b_cancelar.Enabled = false;
            Habilita(true);
            Console.Beep();
        }
        private void SoloCalcula(long num)
        {
            int nbn;
            int nb = CuentaBits(num);
            do
            {
                nciclos++;
                if (maxnum < num) maxnum = num;
                if (num % 2 == 0)
                {
                    // Par

                    num /= 2;
                    nb--;
                    nbitmenos++;
                }
                else
                {
                    // Impar

                    if (maximpar < num) maximpar = num;
                    num = num * 3 + 1;
                    nbn = CuentaBits(num);
                    if (nbn > nb)
                    {
                        nbitmas += nbn - nb;
                        nb = nbn;
                    }
                }
            } while (num > 1 && !cancelar);
        }
        private void Cancela_Click(object sender, EventArgs e)
        {
            cancelar = true;
            b_cancelar.Enabled = false;
            Application.DoEvents();
        }
        private void Numero_TextChanged(object sender, EventArgs e)
        {
            if (evitar)
            {
                evitar = false;
                return;
            }
            if (numero.Text.Trim().Length == 0)
            {
                for (int j = 0; j < 64; j++) d[j].Text = string.Empty;
            }
            try
            {
                Binario(Convert.ToInt64(numero.Text.Trim()), d);
            }
            catch
            {
                for (int j = 0; j < 64; j++) d[j].Text = string.Empty;
            }
        }
        private void Listac_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (omite) return;
            omite = true;
            listabc.TopIndex = listac.TopIndex;
            listabc.SelectedIndex = listac.SelectedIndex;
            omite = false;

        }
        private void Listabc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (omite) return;
            omite = true;
            listac.TopIndex = listabc.TopIndex;
            listac.SelectedIndex = listabc.SelectedIndex;
            omite = false;
        }
        private void Lista_DoubleClick(object sender, EventArgs e)
        {
            if (lista.SelectedItem != null) numero.Text = lista.SelectedItem.ToString();
        }
        private void Listac_DoubleClick(object sender, EventArgs e)
        {
            if (listac.SelectedItem != null) numero.Text = listac.SelectedItem.ToString();
        }
        private void Borraycalcula_Click(object sender, EventArgs e)
        {
            BorraListas();
            Todo();
        }

        private readonly long MIN_TABLA = (long)Math.Pow(2, 30);
        private readonly long INC_MAX = (long)Math.Pow(2, 1);
        private long MAX_TABLA;
        private void Tabla_impares_Click(object sender, EventArgs e)
        {
            long tb = 0;
            if (numero.Text.Trim().Length == 0)
            {
                bloque = 4194304;   // 2 ^ 22
            }
            else
            {
                try
                {
                    tb = Convert.ToInt64(numero.Text.Trim());
                    if (tb < 1)
                    {
                        MessageBox.Show("El tamaño mínimo de bloque es 1");
                        return;
                    }
                    bloque = tb;
                }
                catch
                {
                    MessageBox.Show(numero.Text.Trim() + " no es un número");
                    return;
                }
            }
            long bloque_escritura = 1;
            if (bloque < 1000000)
            {
                bloque_escritura = 1000000 / bloque;
                bloque_escritura *= bloque;
            }
            FicheroTabla(true, "TXT (*.txt)|*.txt|TODOS (*.*)|*.*");
            if (swres == null || swresaux == null) return;
            swresaux.WriteLine(string.Format("{0};{1};{2};{3};{4}", "N dec", "N bin", "Terminación", "Iteraciones", "Impares"));
            Habilita(false);
            listab.Items.Clear();
            string formato = "{0,15:N0} {1,9:N0} {2,9:N0} {3,9:N0} {4,9:N0} {5,15:N0} {6,24:N0} {7,9:N0} {8,9:N0} {9,15:N0} {10,9:N0} {11,9:N0}";
            listab.Items.Add(string.Format(formato, "N", "Impares", "Estaban", "N_Pri_Tab", "Nuevos", "Mínimo", "Máximo", "Entran", "Salen", "Tabla", "Completas", "Tiempo"));
            swres.WriteLine(string.Format(formato, "N", "Impares", "Estaban", "N_tab", "Nuevos", "Mínimo", "Máximo", "Entran", "Salen", "Tabla", "Completas", "Tiempo"));
            tablaimpares = new HashSet<long>();
            nimpbloque = 0;
            estaban = 0;
            npritab = 0;
            impares_nuevos = 0;
            minnum = long.MaxValue;
            maxnum = 0;
            entran = 0;
            salen = 0;
            long salent;
            trayectorias_completas = 0;
            cancelar = false;
            b_cancelar.Enabled = true;
            long tiempo;
            TimeSpan diftpo;
            DateTime tiempo_inicio = DateTime.Now;
            numero.Text = "0";
            Application.DoEvents();

            //long n = 3;
            //const int incremento = 4;
            long n = 1;
            const int incremento = 1;
            MAX_TABLA = n + INC_MAX;
            if (MAX_TABLA < MIN_TABLA) MAX_TABLA = MIN_TABLA;
            long contador = 0;
            while (!cancelar)
            {
                if (contador % bloque == 0 && contador > 0)
                {
                    // La limpieza de la tabla 'RemoveWhere' es la responsable de practicamente todo el tiempo de calculo
                    // Por lo que hay que usar un 'bloque' lo mayor posible, no hay que preocuparse por el espacio
                    // de los elementos que se eliminarán la terminar el bloque porque son muchos menos que los que
                    // quedaran en la tabla, ya que el número de los que salen es practicamente el mismo en todos los bloque y la
                    // tabla crece sistemáticamente.

                    salent = tablaimpares.Count;
                    tablaimpares.RemoveWhere(t => t <= n);
                    salent -= tablaimpares.Count;
                    salen += salent;
                    if (contador % bloque_escritura == 0)
                    {
                        evitar = true;
                        numero.Text = string.Format("{0}", n);
                        diftpo = (DateTime.Now - tiempo_inicio);
                        tiempo = (long)(diftpo.TotalMilliseconds / 1000.0d);
                        swres.WriteLine(string.Format(formato, n, nimpbloque, estaban, npritab, impares_nuevos, minnum, maxnum, entran, salen, tablaimpares.Count, trayectorias_completas, tiempo));
                        listab.Items.Add(string.Format(formato, n, nimpbloque, estaban, npritab, impares_nuevos, minnum, maxnum, entran, salen, tablaimpares.Count, trayectorias_completas, tiempo));
                        listab.TopIndex = listab.Items.Count - 1;
                        nimpbloque = 0;
                        estaban = 0;
                        npritab = 0;
                        impares_nuevos = 0;
                        minnum = long.MaxValue;
                        maxnum = 0;
                        entran = 0;
                        salen = 0;
                        Application.DoEvents();
                    }
                }
                TabulaImpares(n);
                if (cancelar) break;
                contador++;
                n += incremento;
            }
            numero.Text = string.Format("{0}", n);
            salen = tablaimpares.Count;
            tablaimpares.RemoveWhere(t => t <= n);
            salen -= tablaimpares.Count;
            diftpo = (DateTime.Now - tiempo_inicio);
            tiempo = (long)(diftpo.TotalMilliseconds / 1000.0d);
            swresaux.Close();
            swres.WriteLine();
            swres.WriteLine(string.Format(formato, n, nimpbloque, estaban, npritab, impares_nuevos, minnum, maxnum, entran, salen, tablaimpares.Count, trayectorias_completas, tiempo));
            swres.Close();
            listab.Items.Add(string.Empty);
            listab.Items.Add(string.Format(formato, n, nimpbloque, estaban, npritab, impares_nuevos, minnum, maxnum, entran, salen, tablaimpares.Count, trayectorias_completas, tiempo));
            listab.TopIndex = listab.Items.Count - 1;
            b_cancelar.Enabled = false;
            Habilita(true);
            Console.Beep();
        }
        private void TabulaImpares(long num)
        {
            long penultimo = 0;
            if (num % 2 == 1) nimpbloque++;
            numini = num;
            nciclos = 0;
            nimpares = 0;
            do
            {
                nciclos++;
                if (num % 2 == 0)
                {
                    // Par

                    num /= 2;
                }
                else
                {
                    // Impar

                    nimpares++;
                    if (BuscaImpar(num))
                    {
                        if (num == numini)
                        {
                            npritab++;
                        }
                        break;
                    }
                    penultimo = num;
                    num = num * 3 + 1;
                }
            } while (num > 1 && !cancelar);
            if (!cancelar && num == 1 && !PotDos(numini))
            {
                trayectorias_completas++;
                swresaux.WriteLine(string.Format("{0};{1};{2};{3};{4}", numini, Binario(numini), penultimo, nciclos, (numini % 2 == 0) ? nimpares : nimpares - 1));
            }
        }
        private bool BuscaImpar(long num)
        {
            if (num < numini) return true;
            if (tablaimpares.Contains(num))
            {
                estaban++;
                return true;
            }
            impares_nuevos++;
            if (num < minnum) minnum = num;
            if (num > maxnum) maxnum = num;
            if (num != numini)
            {
                try
                {
                    if (num < MAX_TABLA)
                    {
                        tablaimpares.Add(num);
                        entran++;
                    }
                }
                catch
                {
                    cancelar = true;
                    return true;
                }
            }
            return false;
        }
        private bool PotDos(long num)
        {
            while (num > 1)
            {
                if (num % 2 == 1) return false;
                num >>= 1;
            }
            return true;
        }
        private void FicheroTabla(bool aux, string filtro)
        {
            using (SaveFileDialog ficheroescritura = new SaveFileDialog()
            {
                Filter = filtro,
                FilterIndex = 1,
                RestoreDirectory = false,
                OverwritePrompt = false,
                CheckFileExists = false,
                CheckPathExists = true
            })
            {
                if (ficheroescritura.ShowDialog() == DialogResult.OK)
                {
                    swres = null;
                    swresaux = null;
                    try
                    {
                        FileStream fres = new FileStream(ficheroescritura.FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                        if (fres == null || !fres.CanWrite)
                        {
                            MessageBox.Show("Error. No se puede escribir en el fichero " + ficheroescritura.FileName);
                            return;
                        }
                        swres = new StreamWriter(fres, Encoding.UTF8)
                        {
                            AutoFlush = true
                        };
                        if (aux)
                        {
                            string s = Path.Combine(Path.GetDirectoryName(ficheroescritura.FileName), Path.GetFileNameWithoutExtension(ficheroescritura.FileName)) + ".csv";
                            fres = new FileStream(s, FileMode.Create, FileAccess.Write, FileShare.Read);
                            if (fres == null || !fres.CanWrite)
                            {
                                MessageBox.Show("Error. No se puede escribir en el fichero csv" + s);
                                if (swres != null) swres.Close();
                                return;
                            }
                            swresaux = new StreamWriter(fres, Encoding.UTF8)
                            {
                                AutoFlush = true
                            };
                        }
                        return;
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("Error. " + ee.Message + ". Fichero " + ficheroescritura.FileName);
                        if (swres != null) swres.Close();
                    }
                }
            }
        }
        private void Gen_11_Click(object sender, EventArgs e)
        {
            long hasta;
            if (numero.Text.Trim().Length == 0)
            {
                hasta = 1000;
            }
            else
            {
                try
                {
                    hasta = Convert.ToInt64(numero.Text.Trim());
                    if (hasta < 10)
                    {
                        MessageBox.Show("El número máximo debe ser como mínimo 10");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(numero.Text.Trim() + " no es un número");
                    return;
                }
            }
            numero.Text = "0";
            Application.DoEvents();
            FicheroTabla(false, "TXT (*.txt)|*.txt|TODOS (*.*)|*.*");
            if (swres == null) return;
            Habilita(false);
            cancelar = false;
            b_cancelar.Enabled = true;
            long n = 3;
            long contador = 0;
            while (!cancelar && n < hasta)
            {
                contador++;
                if (contador % 1000 == 0)
                {
                    evitar = true;
                    numero.Text = string.Format("{0}", n);
                    Application.DoEvents();
                }
                Sucesion(n);
                if (cancelar) break;
                n += 4;
            }
            swres.Close();
            b_cancelar.Enabled = false;
            Habilita(true);
            Console.Beep();
        }
        private void Sucesion(long num)
        {
            long numini = num;
            swres.Write(string.Format("{0,24} -> {1,6}", Binario(numini), numini));
            do
            {
                if (num % 2 == 0)
                {
                    // Par

                    num /= 2;
                }
                else
                {
                    // Impar

                    if (num != numini) swres.Write(string.Format(" {0,6}", num));
                    num = num * 3 + 1;
                }
            } while (num > 1 && !cancelar);
            swres.WriteLine();
        }
        private void Terminaciones_Click(object sender, EventArgs e)
        {
            long hasta;
            if (numero.Text.Trim().Length == 0)
            {
                hasta = 68719476735;     // 2 ^ 36 - 1
            }
            else
            {
                try
                {
                    hasta = Convert.ToInt64(numero.Text.Trim());
                    if (hasta < 1)
                    {
                        MessageBox.Show("El número máximo debe ser como mínimo 1");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(numero.Text.Trim() + " no es un número");
                    return;
                }
            }
            FicheroTabla(false, "CSV (*.csv)|*.csv|TODOS (*.*)|*.*");
            if (swres == null) return;
            Habilita(false);
            b_cancelar.Enabled = true;
            cancelar = false;
            LanzaHilos(hasta);
        }
        private long SucesionAfin(long num)
        {
            long ul_impar = 0;
            do
            {
                if (num % 2 == 0)
                {
                    // Par

                    num /= 2;
                }
                else
                {
                    // Impar

                    ul_impar = num;
                    num = num * 3 + 1;
                }
            } while (num > 1 && !cancelar);
            return ul_impar;
        }
        private bool LanzaHilos(long hasta)
        {
            try
            {
                numero_hilos = Convert.ToInt32(n_hilos.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Número incorrecto de hilos", "Collatz", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (numero_hilos < 1 || numero_hilos > MAX_HILOS)
            {
                MessageBox.Show("Número incorrecto de hilos", "Collatz", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            ultimos_impares = new Dictionary<long, long>[numero_hilos];
            primer_ultimos_impares = new Dictionary<long, long>[numero_hilos];
            cancelar = false;
            b_cancelar.Enabled = true;
            max_intentos = hasta - 1;
            intentos = 0;
            long pri = 1;
            numero.Text = string.Format("{0}", max_intentos / 1000000);
            Application.DoEvents();
            long fraccionintentos = max_intentos / numero_hilos;
            long restointentos = max_intentos;
            for (int nh = 0; nh < numero_hilos; nh++)
            {
                intentoshilo[nh, 0] = pri;
                if (nh == numero_hilos - 1)
                {
                    intentoshilo[nh, 1] = pri + restointentos;
                }
                else
                {
                    intentoshilo[nh, 1] = pri + fraccionintentos - 1;
                }
                pri = intentoshilo[nh, 1] + 1;
                restointentos -= fraccionintentos;
                ultimos_impares[nh] = new Dictionary<long, long>();
                primer_ultimos_impares[nh] = new Dictionary<long, long>();
                finhiloMente[nh] = false;
                hiloMente[nh] = new Thread(new ParameterizedThreadStart(Hilo));
                hiloMente[nh].Start(nh);
                Application.DoEvents();
            }
            return true;
        }
        private void Hilo(object data)
        {
            int numhilo = (int)data;
            object[] objetonumhilo = new object[] { numhilo };
            long intentos_hilo = intentoshilo[numhilo, 0];
            long max_intentos_hilo = intentoshilo[numhilo, 1];
            long ut;
            try
            {
                while (intentos_hilo <= max_intentos_hilo)
                {
                    intentos_hilo++;
                    if (cancelar) break;
                    lock (sync)
                    {
                        intentos++;
                        if (intentos % 10000000 == 0)
                        {
                            try
                            {
                                this.Invoke(delegadoActualizaEstadisticas, objetonumhilo);
                            }
                            catch
                            {
                                finhiloMente[numhilo] = true;
                                try { this.Invoke(delegadoFinHilo); }
                                catch { }
                                return;
                            }
                        }
                    }
                    if (finhiloMente[numhilo] == true)
                    {
                        try { this.Invoke(delegadoActualizaEstadisticas, objetonumhilo); }
                        catch (Exception e)
                        {
                            object[] objetomensaje = new object[] { numhilo, e.Message };
                            this.Invoke(delegadoMensaje, objetomensaje);
                            return;
                        }
                        try { this.Invoke(delegadoFinHilo); }
                        catch (Exception e)
                        {
                            object[] objetomensaje = new object[] { numhilo, e.Message };
                            this.Invoke(delegadoMensaje, objetomensaje);
                            return;
                        }
                        return;
                    }
                    ut = SucesionAfin(intentos_hilo);
                    if (cancelar) break;
                    if (ultimos_impares[numhilo].ContainsKey(ut))
                    {
                        ultimos_impares[numhilo][ut]++;
                    }
                    else
                    {
                        ultimos_impares[numhilo].Add(ut, 1);
                        primer_ultimos_impares[numhilo].Add(ut, intentos_hilo);
                    }
                }

                // Hilo terminado

                finhiloMente[numhilo] = true;
                lock (sync)
                {
                    try
                    {
                        this.Invoke(delegadoActualizaEstadisticas, objetonumhilo);
                    }
                    catch (Exception e)
                    {
                        object[] objetomensaje = new object[] { numhilo, e.Message };
                        this.Invoke(delegadoMensaje, objetomensaje);
                        return;
                    }
                    try { this.Invoke(delegadoFinHilo); }
                    catch (Exception e)
                    {
                        object[] objetomensaje = new object[] { numhilo, e.Message };
                        this.Invoke(delegadoMensaje, objetomensaje);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                finhiloMente[numhilo] = true;
                object[] objetomensaje = new object[] { numhilo, e.Message };
                this.Invoke(delegadoMensaje, objetomensaje);
                try
                {
                    this.Invoke(delegadoFinHilo);
                }
                catch (Exception ee)
                {
                    object[] objetomm = new object[] { numhilo, ee.Message };
                    this.Invoke(delegadoMensaje, objetomm);
                    return;
                }
            }
        }
        private void ActualizaEstadisticas(int nh)
        {
            evitar = true;
            numero.Text = string.Format("{0}", (max_intentos - intentos) / 1000000);
        }
        private void FinHilo()
        {
            try
            {
                for (int i = 0; i < numero_hilos; i++) if (finhiloMente[i] == false) return;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fin de hilo. a", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Todos los hilos han terminado

            try
            {
                numero.Text = string.Format("{0}", cancelar ? intentos : max_intentos);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fin de hilo. b", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            b_cancelar.Enabled = false;
            Dictionary<long, long> ultimos_impares_t = new Dictionary<long, long>();
            Dictionary<long, long> primer_ultimos_impares_t = new Dictionary<long, long>();
            for (int i = 0; i < numero_hilos; i++)
            {
                foreach (KeyValuePair<long, long> par in ultimos_impares[i])
                {
                    if (ultimos_impares_t.ContainsKey(par.Key))
                    {
                        ultimos_impares_t[par.Key] += par.Value;
                        if (primer_ultimos_impares[i][par.Key] < primer_ultimos_impares_t[par.Key])
                        {
                            primer_ultimos_impares_t[par.Key] = primer_ultimos_impares[i][par.Key];
                        }
                    }
                    else
                    {
                        ultimos_impares_t.Add(par.Key, par.Value);
                        primer_ultimos_impares_t.Add(par.Key, primer_ultimos_impares[i][par.Key]);
                    }
                }
            }
            swres.WriteLine("{0};{1}", "Terminación", "Veces");
            foreach (KeyValuePair<long, long> par in ultimos_impares_t)
            {
                swres.WriteLine("{0};{1};{2}", Binario(par.Key), par.Key, par.Value);
            }
            swres.WriteLine();
            swres.WriteLine("{0};{1}", "Terminación", "Primera vez");
            foreach (KeyValuePair<long, long> par in primer_ultimos_impares_t)
            {
                swres.WriteLine("{0};{1}", par.Key, par.Value);
            }
            swres.Close();
            Habilita(true);
            if (cancelar)
            {
                MessageBox.Show("Cálculo cancelado.", "Collatz", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cálculo terminado.", "Collatz", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void MensajeHilo(int nh, string s)
        {
            MessageBox.Show(s, "Hilo:" + nh.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Prec_Click(object sender, EventArgs e)
        {
            long hasta;
            if (numero.Text.Trim().Length == 0)
            {
                hasta = 250000000000;
            }
            else
            {
                try
                {
                    hasta = Convert.ToInt64(numero.Text.Trim());
                    if (hasta < 10)
                    {
                        MessageBox.Show("El número máximo debe ser como mínimo 10");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(numero.Text.Trim() + " no es un número");
                    return;
                }
            }
            FicheroTabla(false, "TXT (*.txt)|*.txt|TODOS (*.*)|*.*");
            if (swres == null) return;
            Habilita(false);
            cancelar = false;
            b_cancelar.Enabled = true;
            long n = 3;
            long contador = 0;
            while (!cancelar && n < hasta)
            {
                contador++;
                if (contador % 1000000 == 0)
                {
                    evitar = true;
                    numero.Text = string.Format("{0}", (n + 1) / 1000000);
                    Application.DoEvents();
                }
                PreSucesion(n);
                if (cancelar) break;
                n += 4;
            }
            swres.Close();
            b_cancelar.Enabled = false;
            Habilita(true);
            Console.Beep();
        }
        private void PreSucesion(long num)
        {
            long numini = num;
            long ni = 0;
            long pi = 0;
            do
            {
                if (num % 2 == 0)
                {
                    // Par

                    num /= 2;
                }
                else
                {
                    // Impar

                    ni++;
                    pi = num;
                    num = num * 3 + 1;
                }
            } while (num > 1 && !cancelar);
            if (ni == 2)
            {
                swres.WriteLine(string.Format("{0,64} {1,64} {2,24:N0} {3,24:N0}", Binario(numini), Binario(pi), numini, pi));
            }
        }
        private void Anticipos_Click(object sender, EventArgs e)
        {
            long hasta;
            if (numero.Text.Trim().Length == 0)
            {
                hasta = 1000;
            }
            else
            {
                try
                {
                    hasta = Convert.ToInt64(numero.Text.Trim());
                    if (hasta < 10)
                    {
                        MessageBox.Show("El número máximo debe ser como mínimo 10");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(numero.Text.Trim() + " no es un número");
                    return;
                }
            }
            FicheroTabla(false, "TXT (*.txt)|*.txt|TODOS (*.*)|*.*");
            if (swres == null) return;
            Habilita(false);
            cancelar = false;
            b_cancelar.Enabled = true;
            long n = 1;
            long contador = 0;
            while (!cancelar && n < hasta)
            {
                contador++;
                if (contador % 1000000 == 0)
                {
                    evitar = true;
                    numero.Text = string.Format("{0}", (n + 1) / 1000000);
                    Application.DoEvents();
                }
                SucesionDeAnticipos(n);
                if (cancelar) break;
                n += 1;
            }
            foreach (KeyValuePair<long, long> par in anti)
            {
                swres.WriteLine("{0,16:N0} {1,16:N0}", par.Key, par.Value);
            }
            swres.Close();
            b_cancelar.Enabled = false;
            Habilita(true);
            Console.Beep();
        }
        private void SucesionDeAnticipos(long num)
        {
            long numini = num;
            do
            {
                if (num % 2 == 0)
                {
                    // Par

                    num /= 2;
                }
                else
                {
                    // Impar

                    num = num * 3 + 1;
                }
                if (num > numini && (num & 3) == 3)
                {
                    if (!anti.ContainsKey(num)) anti.Add(num, numini);
                }
            } while (num > 1 && !cancelar);
        }
    }
}