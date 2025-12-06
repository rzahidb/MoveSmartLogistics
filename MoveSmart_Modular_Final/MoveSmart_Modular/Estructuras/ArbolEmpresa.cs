using System;
using System.Collections.Generic;
using System.IO; // Necesario para BinaryWriter y BinaryReader
using ArbolEmpresaMudanzas.Modulo;

namespace ArbolEmpresaMudanzas.Estructuras
{
    public class ArbolEmpresa
    {
        public Empleado Raiz { get; private set; }
        private const string RUTA_ARCHIVO = "datos_empresa.bin";

        public ArbolEmpresa(Empleado ceo) { Raiz = ceo; }

        // =========================================================
        // MANEJO DE ARCHIVOS BINARIOS (MANUAL Y SEGURO)
        // Usamos BinaryWriter/Reader
        // =========================================================

        public void GuardarEnArchivo()
        {
            try
            {
                using (FileStream fs = new FileStream(RUTA_ARCHIVO, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    // Iniciamos la cadena de guardado recursivo desde la raíz
                    if (Raiz != null)
                    {
                        writer.Write(true); // Marca: "Sí hay datos"
                        GuardarNodoRecursivo(writer, Raiz);
                    }
                    else
                    {
                        writer.Write(false); // Marca: "Árbol vacío"
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar binario: " + ex.Message);
            }
        }

        // Método auxiliar que escribe los datos bit a bit
        private void GuardarNodoRecursivo(BinaryWriter writer, Empleado nodo)
        {
            // 1. Escribimos los datos simples
            writer.Write(nodo.Nombre);
            writer.Write(nodo.Cargo);
            writer.Write(nodo.Cedula);
            writer.Write(nodo.FechaIngreso.ToBinary()); // Convertimos fecha a numero largo

            // 2. Escribimos cuántos hijos tiene
            writer.Write(nodo.Subordinados.Count);

            // 3. Guardamos a cada hijo (Recursividad)
            foreach (var sub in nodo.Subordinados)
            {
                GuardarNodoRecursivo(writer, sub);
            }
        }

        public bool CargarDesdeArchivo()
        {
            if (!File.Exists(RUTA_ARCHIVO)) return false;

            try
            {
                using (FileStream fs = new FileStream(RUTA_ARCHIVO, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    bool hayDatos = reader.ReadBoolean();
                    if (hayDatos)
                    {
                        Raiz = CargarNodoRecursivo(reader);
                        return true;
                    }
                }
            }
            catch { return false; }

            return false;
        }

        // Método auxiliar que lee los datos en el mismo orden que se escribieron
        private Empleado CargarNodoRecursivo(BinaryReader reader)
        {
            // 1. Leemos los datos simples
            string nombre = reader.ReadString();
            string cargo = reader.ReadString();
            string cedula = reader.ReadString();
            long fechaBin = reader.ReadInt64();
            DateTime fecha = DateTime.FromBinary(fechaBin);

            // 2. Creamos el empleado
            Empleado empleado = new Empleado(nombre, cargo, cedula, fecha);

            // 3. Leemos cuántos hijos tenía
            int cantidadHijos = reader.ReadInt32();

            // 4. Leemos a cada hijo (Recursividad)
            for (int i = 0; i < cantidadHijos; i++)
            {
                Empleado hijo = CargarNodoRecursivo(reader);
                empleado.Subordinados.Add(hijo);
            }

            return empleado;
        }

        // ==========================================
        // RESTO DE LA LÓGICA (VALIDACIONES Y CRUD)
        // (Esto sigue igual que siempre)
        // ==========================================

        public bool ExisteCedula(string cedula) { return BuscarCedulaRecursivo(Raiz, cedula); }
        private bool BuscarCedulaRecursivo(Empleado actual, string cedula)
        {
            if (actual == null) return false;
            if (actual.Cedula.Equals(cedula, StringComparison.OrdinalIgnoreCase)) return true;
            foreach (var sub in actual.Subordinados) if (BuscarCedulaRecursivo(sub, cedula)) return true;
            return false;
        }

        public bool AgregarSubordinado(string nombreJefe, string nombre, string cargo, string cedula, DateTime fecha)
        {
            Empleado jefe = BuscarEmpleado(nombreJefe);
            if (jefe != null)
            {
                jefe.Subordinados.Add(new Empleado(nombre, cargo, cedula, fecha));
                GuardarEnArchivo();
                return true;
            }
            return false;
        }

        public Empleado BuscarEmpleado(string busqueda) { return BuscarRecursivo(Raiz, busqueda); }
        private Empleado BuscarRecursivo(Empleado actual, string busqueda)
        {
            if (actual == null) return null;
            bool n = actual.Nombre.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0;
            bool c = actual.Cedula.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0;
            bool k = actual.Cargo.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0;
            if (n || c || k) return actual;
            foreach (var sub in actual.Subordinados)
            {
                Empleado enc = BuscarRecursivo(sub, busqueda);
                if (enc != null) return enc;
            }
            return null;
        }

        public bool EliminarEmpleado(string nombre)
        {
            if (Raiz.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)) return false;
            if (EliminarRecursivo(Raiz, nombre)) { GuardarEnArchivo(); return true; }
            return false;
        }
        private bool EliminarRecursivo(Empleado jefe, string busqueda)
        {
            if (jefe == null) return false;
            for (int i = 0; i < jefe.Subordinados.Count; i++)
            {
                if (jefe.Subordinados[i].Nombre.Equals(busqueda, StringComparison.OrdinalIgnoreCase))
                {
                    jefe.Subordinados.RemoveAt(i); return true;
                }
                if (EliminarRecursivo(jefe.Subordinados[i], busqueda)) return true;
            }
            return false;
        }
    }
}