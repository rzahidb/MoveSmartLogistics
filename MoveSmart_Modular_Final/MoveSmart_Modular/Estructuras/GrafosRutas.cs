using System;
using System.Collections.Generic;
using System.Linq;
using MoveSmart_Modular.Modelos; // Importante para usar Ciudad y Ruta

namespace MoveSmart_Modular.Estructuras
{
    public class GrafoRutas
    {
        // Listas principales
        public List<Ciudad> Mapa { get; set; } = new List<Ciudad>();
        public Dictionary<Ciudad, List<Ruta>> Adyacencia { get; set; } = new Dictionary<Ciudad, List<Ruta>>();
        public Dictionary<string, List<Ciudad>> Departamentos { get; set; } = new Dictionary<string, List<Ciudad>>();

        // Singleton: Para acceder a la misma instancia desde cualquier Form
        public static GrafoRutas Instancia { get; } = new GrafoRutas();

        private GrafoRutas()
        {
            CargarMapaCalibrado();
        }

        private void CargarMapaCalibrado()
        {
            // --- PACÍFICO ---
            Crear("Chinandega", "Chinandega", 180, 400);
            Crear("Corinto", "Chinandega", 160, 420);
            Crear("Somotillo", "Chinandega", 200, 320);
            Crear("El Viejo", "Chinandega", 170, 390);
            Crear("Chichigalpa", "Chinandega", 190, 410);

            Crear("León", "León", 230, 460);
            Crear("Nagarote", "León", 260, 490);
            Crear("La Paz Centro", "León", 250, 480);
            Crear("Malpaisillo", "León", 240, 440);
            Crear("El Sauce", "León", 250, 380);

            Crear("Managua", "Managua", 300, 520);
            Crear("Tipitapa", "Managua", 320, 510);
            Crear("San Rafael del Sur", "Managua", 280, 550);
            Crear("Mateare", "Managua", 280, 500);

            Crear("Masaya", "Masaya", 330, 540);
            Crear("Nindirí", "Masaya", 325, 535);
            Crear("La Concepción", "Masaya", 320, 550);

            Crear("Granada", "Granada", 350, 550);
            Crear("Nandaime", "Granada", 340, 580);
            Crear("Diriomo", "Granada", 345, 560);

            Crear("Jinotepe", "Carazo", 310, 570);
            Crear("Diriamba", "Carazo", 305, 565);
            Crear("San Marcos", "Carazo", 310, 560);

            Crear("Rivas", "Rivas", 360, 630);
            Crear("San Juan del Sur", "Rivas", 350, 660);
            Crear("Tola", "Rivas", 340, 640);
            Crear("San Jorge", "Rivas", 370, 630);
            Crear("Moyogalpa (Ometepe)", "Rivas", 400, 610);
            Crear("Altagracia (Ometepe)", "Rivas", 420, 620);

            // --- NORTE ---
            Crear("Estelí", "Estelí", 280, 360);
            Crear("Condega", "Estelí", 290, 340);
            Crear("La Trinidad", "Estelí", 290, 380);
            Crear("Pueblo Nuevo", "Estelí", 280, 330);

            Crear("Somoto", "Madriz", 270, 320);
            Crear("Palacagüina", "Madriz", 280, 330);
            Crear("Yalagüina", "Madriz", 275, 325);
            Crear("Telpaneca", "Madriz", 300, 320);

            Crear("Ocotal", "Nueva Segovia", 280, 300);
            Crear("Jalapa", "Nueva Segovia", 300, 270);
            Crear("Dipilto", "Nueva Segovia", 270, 290);
            Crear("Mozonte", "Nueva Segovia", 285, 300);
            Crear("Wiwilí NS", "Nueva Segovia", 350, 280);

            Crear("Jinotega", "Jinotega", 350, 350);
            Crear("San Rafael del Norte", "Jinotega", 340, 340);
            Crear("Wiwilí", "Jinotega", 400, 280);
            Crear("El Cuá", "Jinotega", 410, 320);
            Crear("Pantasma", "Jinotega", 380, 310);

            Crear("Matagalpa", "Matagalpa", 380, 400);
            Crear("Sébaco", "Matagalpa", 360, 390);
            Crear("San Isidro", "Matagalpa", 340, 380);
            Crear("Ciudad Darío", "Matagalpa", 350, 420);
            Crear("Río Blanco", "Matagalpa", 520, 400);
            Crear("Matiguás", "Matagalpa", 470, 390);
            Crear("Muy Muy", "Matagalpa", 450, 410);

            // --- CENTRO ---
            Crear("Boaco", "Boaco", 420, 480);
            Crear("Camoapa", "Boaco", 450, 470);
            Crear("Teustepe", "Boaco", 380, 470);
            Crear("Santa Lucía", "Boaco", 410, 460);

            Crear("Juigalpa", "Chontales", 480, 520);
            Crear("Acoyapa", "Chontales", 510, 540);
            Crear("Santo Tomás", "Chontales", 530, 510);
            Crear("La Libertad", "Chontales", 500, 500);
            Crear("San Pedro de Lóvago", "Chontales", 520, 520);
            Crear("Villa Sandino", "Chontales", 540, 500);

            Crear("San Carlos", "Río San Juan", 550, 680);
            Crear("El Castillo", "Río San Juan", 600, 700);
            Crear("San Juan de Nicaragua", "Río San Juan", 720, 720);
            Crear("San Miguelito", "Río San Juan", 530, 620);
            Crear("Morrito", "Río San Juan", 510, 580);

            // --- CARIBE (RACCN) ---
            Crear("Bilwi (Pto Cabezas)", "RACCN", 750, 250);
            Crear("Waspam", "RACCN", 680, 180);
            Crear("Siuna", "RACCN", 520, 300);
            Crear("Rosita", "RACCN", 580, 280);
            Crear("Bonanza", "RACCN", 560, 270);
            Crear("Prinzapolka", "RACCN", 720, 320);
            Crear("Waslala", "RACCN", 480, 330);
            Crear("Mulukukú", "RACCN", 500, 320);

            // --- CARIBE (RACCS) ---
            Crear("Bluefields", "RACCS", 740, 530);
            Crear("El Rama", "RACCS", 620, 500);
            Crear("Nueva Guinea", "RACCS", 580, 560);
            Crear("Muelle de los Bueyes", "RACCS", 600, 490);
            Crear("Kukra Hill", "RACCS", 720, 510);
            Crear("Laguna de Perlas", "RACCS", 740, 480);
            Crear("Corn Island", "RACCS", 820, 520); // Isla
            Crear("El Ayote", "RACCS", 550, 450);
            Crear("Bocana de Paiwas", "RACCS", 530, 380);


            // === CONEXIONES ===
            // (false = Carretera Gris, true = Agua Azul)

            // Pacifico
            Conectar("Chinandega", "Corinto", 20, false);
            Conectar("Chinandega", "El Viejo", 10, false);
            Conectar("Chinandega", "Chichigalpa", 15, false);
            Conectar("Chichigalpa", "León", 30, false);
            Conectar("Chinandega", "Somotillo", 40, false);

            Conectar("León", "Nagarote", 35, false);
            Conectar("León", "La Paz Centro", 30, false);
            Conectar("León", "Malpaisillo", 25, false);
            Conectar("Malpaisillo", "El Sauce", 40, false);
            Conectar("Nagarote", "Mateare", 20, false);
            Conectar("Mateare", "Managua", 20, false);
            Conectar("León", "Managua", 90, false); // Carretera Vieja

            Conectar("Managua", "Tipitapa", 25, false);
            Conectar("Managua", "Masaya", 30, false);
            Conectar("Managua", "San Rafael del Sur", 45, false);

            Conectar("Masaya", "Granada", 15, false);
            Conectar("Masaya", "Nindirí", 5, false);
            Conectar("Masaya", "La Concepción", 15, false);
            Conectar("Masaya", "Jinotepe", 30, false);

            Conectar("Jinotepe", "Diriamba", 5, false);
            Conectar("Jinotepe", "San Marcos", 10, false);
            Conectar("Jinotepe", "Nandaime", 25, false);

            Conectar("Granada", "Nandaime", 20, false);
            Conectar("Granada", "Diriomo", 10, false);
            Conectar("Nandaime", "Rivas", 45, false);

            Conectar("Rivas", "San Juan del Sur", 30, false);
            Conectar("Rivas", "Tola", 15, false);
            Conectar("Rivas", "San Jorge", 10, false);
            Conectar("San Jorge", "Moyogalpa (Ometepe)", 20, true); // Ferry
            Conectar("Moyogalpa (Ometepe)", "Altagracia (Ometepe)", 20, false);

            // Norte y Centro
            Conectar("Tipitapa", "Boaco", 70, false);
            Conectar("Tipitapa", "Sébaco", 75, false);

            Conectar("Sébaco", "Matagalpa", 30, false);
            Conectar("Sébaco", "San Isidro", 20, false);
            Conectar("Sébaco", "Ciudad Darío", 15, false);
            Conectar("Sébaco", "Estelí", 45, false);

            Conectar("Estelí", "Condega", 35, false);
            Conectar("Estelí", "La Trinidad", 20, false);
            Conectar("Condega", "Pueblo Nuevo", 15, false);
            Conectar("Condega", "Yalagüina", 20, false);
            Conectar("Yalagüina", "Somoto", 15, false);

            Conectar("Somoto", "Palacagüina", 10, false);
            Conectar("Palacagüina", "Telpaneca", 15, false);
            Conectar("Palacagüina", "Ocotal", 25, false);

            Conectar("Ocotal", "Dipilto", 15, false);
            Conectar("Ocotal", "Mozonte", 10, false);
            Conectar("Ocotal", "Jalapa", 60, false);
            Conectar("Ocotal", "Wiwilí NS", 70, false);

            Conectar("Matagalpa", "Jinotega", 35, false);
            Conectar("Jinotega", "San Rafael del Norte", 25, false);
            Conectar("Jinotega", "Pantasma", 40, false);
            Conectar("Pantasma", "Wiwilí", 50, false);
            Conectar("Wiwilí", "Wiwilí NS", 10, true); // Cruzar río coco

            // Ruta Caribe Norte
            Conectar("Matagalpa", "Matiguás", 90, false);
            Conectar("Matiguás", "Muy Muy", 20, false);
            Conectar("Muy Muy", "Boaco", 50, false);
            Conectar("Matiguás", "Río Blanco", 30, false);
            Conectar("Río Blanco", "Mulukukú", 50, false);
            Conectar("Mulukukú", "Siuna", 60, false);
            Conectar("Siuna", "Rosita", 70, false);
            Conectar("Rosita", "Bonanza", 30, false);
            Conectar("Rosita", "Bilwi (Pto Cabezas)", 110, false);
            Conectar("Bilwi (Pto Cabezas)", "Waspam", 100, false);
            Conectar("Bilwi (Pto Cabezas)", "Prinzapolka", 90, true); // Costa/Mar

            // Waslala
            Conectar("Siuna", "Waslala", 90, false);
            Conectar("Waslala", "El Cuá", 60, false); // Conexión a Jinotega

            // Ruta Juigalpa - Rama
            Conectar("Boaco", "Camoapa", 30, false);
            Conectar("Boaco", "Teustepe", 20, false);
            Conectar("Boaco", "Santa Lucía", 15, false);
            Conectar("Boaco", "Juigalpa", 80, false);

            Conectar("Juigalpa", "La Libertad", 30, false);
            Conectar("Juigalpa", "Acoyapa", 40, false);
            Conectar("Juigalpa", "San Pedro de Lóvago", 30, false);
            Conectar("San Pedro de Lóvago", "Santo Tomás", 20, false);
            Conectar("Santo Tomás", "Villa Sandino", 20, false);
            Conectar("Villa Sandino", "Muelle de los Bueyes", 40, false);
            Conectar("Muelle de los Bueyes", "El Rama", 40, false);

            // Ruta Sur y Rio San Juan
            Conectar("Acoyapa", "Morrito", 50, false);
            Conectar("Morrito", "San Miguelito", 40, false);
            Conectar("San Miguelito", "San Carlos", 40, false);
            Conectar("San Carlos", "El Castillo", 60, true); // Río
            Conectar("El Castillo", "San Juan de Nicaragua", 90, true); // Río

            // Nueva Guinea y Bluefields
            Conectar("Muelle de los Bueyes", "Nueva Guinea", 60, false);
            Conectar("Nueva Guinea", "Bluefields", 110, false); // Carretera

            Conectar("El Rama", "Kukra Hill", 50, true); // Panga
            Conectar("Kukra Hill", "Laguna de Perlas", 20, false);
            Conectar("Kukra Hill", "Bluefields", 40, true);
            Conectar("Bluefields", "Corn Island", 80, true); // Barco/Avion
        }

        // Métodos Auxiliares
        private void Crear(string n, string d, float x, float y)
        {
            var c = new Ciudad { Nombre = n, Departamento = d, X = x, Y = y };
            Mapa.Add(c);
            Adyacencia[c] = new List<Ruta>();
            if (!Departamentos.ContainsKey(d)) Departamentos[d] = new List<Ciudad>();
            Departamentos[d].Add(c);
        }

        private void Conectar(string a, string b, int km, bool esAgua)
        {
            var c1 = Mapa.FirstOrDefault(x => x.Nombre == a);
            var c2 = Mapa.FirstOrDefault(x => x.Nombre == b);
            if (c1 != null && c2 != null)
            {
                Adyacencia[c1].Add(new Ruta { Destino = c2, DistanciaKm = km, EsAcuatica = esAgua });
                Adyacencia[c2].Add(new Ruta { Destino = c1, DistanciaKm = km, EsAcuatica = esAgua });
            }
        }

        // ALGORITMO DIJKSTRA
        public List<Ciudad> CalcularRuta(Ciudad inicio, Ciudad fin, out int kmTotal, out string tipo)
        {
            var dist = new Dictionary<Ciudad, int>();
            var prev = new Dictionary<Ciudad, Ciudad>();
            var rutasTipos = new Dictionary<Ciudad, bool>();
            var q = new List<Ciudad>();

            foreach (var c in Mapa) { dist[c] = int.MaxValue; q.Add(c); }
            dist[inicio] = 0;

            while (q.Count > 0)
            {
                q.Sort((x, y) => dist[x].CompareTo(dist[y]));
                var u = q[0]; q.RemoveAt(0);
                if (u == fin) break;

                foreach (var r in Adyacencia[u])
                {
                    int alt = dist[u] + r.DistanciaKm;
                    if (alt < dist[r.Destino])
                    {
                        dist[r.Destino] = alt;
                        prev[r.Destino] = u;
                        rutasTipos[r.Destino] = r.EsAcuatica;
                    }
                }
            }

            var ruta = new List<Ciudad>();
            var paso = fin;
            kmTotal = dist[fin];
            tipo = "";
            bool hayAgua = false;

            if (kmTotal == int.MaxValue) return null;

            while (paso != null)
            {
                ruta.Insert(0, paso);
                if (rutasTipos.ContainsKey(paso) && rutasTipos[paso]) hayAgua = true;
                if (prev.ContainsKey(paso)) paso = prev[paso]; else paso = null;
            }

            tipo = hayAgua ? "Mixta (Terrestre + Acuática)" : "Terrestre";
            return ruta;
        }
    }
}